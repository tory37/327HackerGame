using UnityEngine;
using System.Collections;

public enum PowerUpTypes { Stun, Invisibility }

public class PowerUpManager : MonoBehaviour {

    public static PowerUpManager Instance { get { return instance; } set { instance = value; } }
    private static PowerUpManager instance;

    void Awake()
    {
        Instance = this;
    }

    public void DoPowerUp(PowerUpTypes type)
    {
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
        StartCoroutine(HidePlayer());
    }



    private void Stun()
    {
        GameManager.Instance.StunAllEnemies();
    }

    private IEnumerator HidePlayer()
    {
        GameManager.Instance.PlayerHidden = true;
        yield return new WaitForSeconds(5);
        GameManager.Instance.PlayerHidden = false;

    }
}
