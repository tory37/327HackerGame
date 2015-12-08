using UnityEngine;
using System.Collections;

public class PlayerTeleport : MonoBehaviour {

    GameManager theManager = GameManager.Instance;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () 
    {
        //Will check to see if the user has pressed E
        if(Input.GetKeyDown(KeyCode.E))
        {

            

        }
	
	}
    /*
    Cell GetAimedCell()
    {
        Vector3 playerFacing = transform.forward;
        Ray checkWall = new Ray(transform.position, playerFacing);
        RaycastHit wallMaybe;
        if(Physics.Raycast(checkWall, out Wall, 5,LayerMask.NameToLayer("Walls")))
        {
            wallMaybe.distance
        }
    }
    */
    void GetCellThroughWall(Cell playerCell)
    {

    }

    void getCellAbovePlayer(Cell playerCell)
    {

    }




}
