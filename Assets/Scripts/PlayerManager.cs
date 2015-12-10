using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour {

    private int StunCount, InvisibilityCount;
    [SerializeField]
    private GameObject Stun, Invisibility;

    public enum powerUps { Stun, Invisibility };
    powerUps currentPowerUp = powerUps.Stun;

    GameManager manager;


	// Use this for initialization
	void Start () 
    {
        manager = GameManager.Instance;
        StunCount = 0;
	}
	
	// Update is called once per frame
	void Update () 
    {
	    //logic for choosing which powerup to use and when to use it here
        if (Input.GetKeyDown("e"))
        {
            //use the selected powerup
            if (currentPowerUp == powerUps.Stun)
            {
                //stun the enemies
                if (StunCount > 0)
                {
                    Debug.Log("Stunning all enemies");
                    manager.StunAllEnemies();
                    StunCount--;
                }
            }
            else
            {
                if (InvisibilityCount > 0)
                {
                    Debug.Log("Turning Invisible!");
                    manager.HidePlayer();
                    InvisibilityCount--;
                }
            }

        }

        if (Input.GetKeyDown("q"))
        {
            Debug.Log("Changing the powerup type");
            //change the powerup to be the other one
            if (currentPowerUp == powerUps.Stun)
                currentPowerUp = powerUps.Invisibility;
            else
                currentPowerUp = powerUps.Stun;
        }
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PowerUp")
        {
            Debug.Log("Entered a power up");
            //if (other.gameObject == Invisibility) { }   //update Invisibility count (or activate immediately)
            if (other.gameObject == Stun) 
            { 
                //update stun count
                Debug.Log("You have picked up a stun power up"); 
                StunCount++;
                //destroy the powerup so that it cannot be picked up again
                GameObject.Destroy(other.gameObject);
            }           
            if (other.gameObject == Invisibility)
            {
                //update invisibility count
                Debug.Log("You have picked up an invisibility pwer up");
                InvisibilityCount++;
                //destroy the powerup so that it cannot be picked up again
                GameObject.Destroy(other.gameObject);
            }
            
        }
    }
}
