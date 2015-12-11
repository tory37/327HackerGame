using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerManager : MonoBehaviour {

    Dictionary<PowerUpTypes, int> powerUpDict = new Dictionary<PowerUpTypes, int>();

    PowerUpTypes currentPowerUp = PowerUpTypes.Invisibility;

    GameManager manager;

    void Awake()
    {
        powerUpDict.Add(PowerUpTypes.Invisibility, 0);
        powerUpDict.Add(PowerUpTypes.Stun, 0);
    }

	// Use this for initialization
	void Start () 
    {
        manager = GameManager.Instance;
	}
	
	// Update is called once per frame
	void Update () 
    {
	    //logic for choosing which powerup to use and when to use it here
        if (Input.GetKeyDown("e"))
        {
            if (powerUpDict[currentPowerUp] > 0)
            {
                powerUpDict[currentPowerUp]--;
            }
        }

        if (Input.GetKeyDown("q"))
        {
            Debug.Log("Changing the powerup type");
            //change the powerup to be the other one
            if (currentPowerUp == PowerUpTypes.Stun)
                currentPowerUp = PowerUpTypes.Invisibility;
            else
                currentPowerUp = PowerUpTypes.Stun;
        }
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PowerUp")
        {
            PowerUP pu = other.GetComponent<PowerUP>();
            if (pu != null)
            {
                powerUpDict[pu.Type]++;
            }
            GameObject.Destroy(other.gameObject);
        }
    }
}
