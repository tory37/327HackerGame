using UnityEngine;
using System.Collections;

public class MapReveal : MonoBehaviour {

    MazeGenerator MG;

    public void OnTriggerEnter(Collider other)
    { 
        //GameObject ceiling = (GameObject)other.gameObject;
        
        if(other.tag == "ceiling")
        {
            other.gameObject.layer = 8;
            foreach(Transform child in other.transform)
            {
                child.gameObject.layer = 8;
            }
        }
    }
}
