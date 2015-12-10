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
        StartCoroutine(HidePlayer());
    }

    //stun the enemies
    private void Stun()
    {
        GameManager.Instance.StunAllEnemies();
    }

    private IEnumerator HidePlayer()
    {
        //hide the player for 5 seconds by setting a boolean to true. if an enemy comes within sight range of the player the boolean is checked
        //to know if they can actually see the player or not
        GameManager.Instance.PlayerHidden = true;
        yield return new WaitForSeconds(5);
        GameManager.Instance.PlayerHidden = false;

    }
}
