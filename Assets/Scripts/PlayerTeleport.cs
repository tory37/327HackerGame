﻿using UnityEngine;
using System.Collections;

public class PlayerTeleport : MonoBehaviour {

    GameManager theManager;
    [SerializeField]
    private Camera PlayerCam;
    [SerializeField]
    private AudioClip [] teleportSounds;
    private AudioSource sound;

    private float cooldownTime;
    private bool canTeleport;


	// Use this for initialization
	void Start () {
        theManager = GameManager.Instance;
        sound = gameObject.GetComponent<AudioSource>();
        cooldownTime = 5.0f;
        canTeleport = true;
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
            if(canTeleport && toTeleport.ID != -1  )
            {
                //The teleport has succeeded, move to next cell
                transform.position = toTeleport.cellCenter + new Vector3(0,.5f,0);
                //play the success sound

                sound.clip = teleportSounds[1];
                sound.Play();
                StartCoroutine(countdowntimer());
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
        Vector3 lookingFlat = new Vector3(PlayerCam.transform.forward.x, 0, PlayerCam.transform.forward.z);
        Ray checkWall = new Ray(transform.position, PlayerCam.transform.forward);
        RaycastHit wallMaybe;
        if(Physics.Raycast(checkWall, out wallMaybe, 7))
        {
            if (wallMaybe.transform.tag == "wall")
            {
                Vector3 teleportFromWallDistance = transform.position - wallMaybe.transform.position;
                Vector3 teleportTarget = wallMaybe.transform.position
                    - teleportFromWallDistance;

                Cell toGoTo = theManager.GetCellPositionIsIn(teleportTarget);
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

    IEnumerator countdowntimer()
    {
        float counter = 0f;
        canTeleport = false;

        while(counter < cooldownTime)
        {
            //Tory do your stuff here
            yield return new WaitForSeconds(.05f);
            counter += .05f;
        }

        canTeleport = true;
                
    }




}
