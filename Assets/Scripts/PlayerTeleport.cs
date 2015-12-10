using UnityEngine;
using System.Collections;

public class PlayerTeleport : MonoBehaviour {

    GameManager theManager;
    [SerializeField]
    private Camera PlayerCam;
    [SerializeField]
    private AudioClip [] teleportSounds;
    private AudioSource sound;

	// Use this for initialization
	void Start () {
        theManager = GameManager.Instance;
        sound = gameObject.GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        //Will check to see if the user has pressed E
        if(Input.GetMouseButtonDown(0))
        {
            if(sound.isPlaying)
            {
                sound.Stop();
            }
            Cell toTeleport = GetTeleport();
            if(toTeleport.ID != -1)
            {
                //The teleport has succeeded, move to next cell
                transform.position = toTeleport.cellCenter + new Vector3(0,.5f,0);
                //play the success sound
                sound.clip = teleportSounds[1];
                sound.Play();
            }
            else
            {
                //failed, play the fail sound
                sound.clip = teleportSounds[0];
                sound.Play();
            }
        }
	
	}
    Cell GetTeleport()
    {
        Debug.Log("Attempting Teleport!");
        Vector3 lookingFlat = new Vector3(PlayerCam.transform.forward.x, 0, PlayerCam.transform.forward.z);
        Ray checkWall = new Ray(transform.position, PlayerCam.transform.forward);
        RaycastHit wallMaybe;
        Debug.Log("Attempting to look for a wall!");
        if(Physics.Raycast(checkWall, out wallMaybe, 7))
        {
            Debug.Log("Something was hit");
            Debug.Log("Hit object center at: " + wallMaybe.transform.position);
            Debug.Log("Object was: " + wallMaybe.transform.name);
            if (wallMaybe.transform.tag == "wall")
            {
                Vector3 teleportFromWallDistance = transform.position - wallMaybe.transform.position;
                Vector3 teleportTarget = wallMaybe.transform.position
                    - teleportFromWallDistance;

                Cell toGoTo = theManager.GetCellPositionIsIn(teleportTarget);
                Debug.Log("Jump from: " +
                    theManager.GetCellPositionIsIn(transform.position) + " to " + toGoTo.ID);
                return toGoTo;
            }
        }
        return new Cell(-1, -1, -1);
        

        //We are going to figure out which direction the player is looking
        //Vector3 looking = transform.forward - new Vector3(0,transform.forward.y,0);
        //looking = looking.normalized;
        //float dotProductOfLookingAndX = Vector3.Dot(looking, Vector3.right);
        //if(looking.z > 0 && dotProductOfLookingAndX < )


    }




}
