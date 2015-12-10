using UnityEngine;
using System.Collections;

public enum PowerUpTypes { Stun, Invisibility }

public class PowerUpManager : MonoBehaviour {

    //make sure there is only one instance of this script (via a jank singleton pattern)
    public static PowerUpManager Instance { get { return instance; } set { instance = value; } }
    private static PowerUpManager instance;

    void Awake()
    {
        Instance = this;
    }

    public void DoPowerUp(PowerUpTypes type)
    {
        //check the type passed and call the proper powerup function based on the parameter
        if (type == PowerUpTypes.Invisibility)
        {
            Invisibility();
        }
        else if (type == PowerUpTypes.Stun)
        {
            Stun();
        }
    }

    private void Invisibility()
    {
        //start a coroutine to hide the player for a set amount of item
        
        //GameManager.Instance.HidePlayer();
    }

    //stun the enemies
    private void Stun()
    {
        GameManager.Instance.StunAllEnemies();
    }
}
