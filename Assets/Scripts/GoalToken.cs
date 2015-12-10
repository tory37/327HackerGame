using UnityEngine;
using System.Collections;

public class GoalToken : MonoBehaviour {


    GameManager theManager;
	// Use this for initialization
	void Start () {
        theManager = GameManager.Instance;
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate (new Vector3 (0, 80, 0) * Time.deltaTime);
	}

    void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Player")
        {
            Debug.Log("Pop!");
            theManager.NotifyPlayerHasToken();

            //We need to tell the player to get out (Get to (0,0))!
            Destroy(gameObject);
        }
    }
}
