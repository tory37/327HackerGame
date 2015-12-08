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
            Cell toTeleport = GetTeleport();
            if(toTeleport.ID != -1)
            {
                transform.position = toTeleport.cellCenter + new Vector3(0,.5f,0);
            }
        }
	
	}
    Cell GetTeleport()
    {
        Debug.Log("Attempting Teleport!");
        Vector3 playerFacing = transform.forward;
        Ray checkWall = new Ray(transform.position, playerFacing);
        RaycastHit wallMaybe;
        Debug.Log("");
        if(Physics.Raycast(checkWall, out wallMaybe, 5))
        {
            Debug.Log("Something was hit");
            Vector3 teleportFromWallDistance = wallMaybe.transform.position
                - transform.position;
            Vector3 teleportTarget = wallMaybe.transform.position 
                + teleportFromWallDistance;

            Cell toGoTo = theManager.GetCellPositionIsIn(teleportTarget);
            Debug.Log("Jump from: "+
                theManager.GetCellPositionIsIn(transform.position)+ " to "+ toGoTo.ID);
            return toGoTo;
        }
        return new Cell(-1, -1, -1);
    }




}
