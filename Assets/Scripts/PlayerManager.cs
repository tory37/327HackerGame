using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour {

    private int InvisibilityCount, SpeedBoostCount, StunCount, TeleportCount;
    [SerializeField]
    private GameObject Invisibility, SpeedBoost, Stun, Teleport;
	// Use this for initialization
	void Start () 
    {
	    
	}
	
	// Update is called once per frame
	void Update () 
    {
	    //logic for choosing which powerup to use and when to use it here
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PowerUp")
        {
            if (other.gameObject == Invisibility) { }   //update Invisibility count (or activate immediately)
            if (other.gameObject == SpeedBoost) { }     //update SpeedBoost count (or activate immediately)
            if (other.gameObject == Stun) { }           //update Stun count
            if (other.gameObject == Teleport) { }       //update Teleport count
        }

        if (other.tag == "ceiling")
        {
            other.gameObject.layer = 8;
            foreach (Transform child in other.transform)
            {
                child.gameObject.layer = 8;
            }
        }
    }
}
