using UnityEngine;
using System.Collections;

public class MapReveal : MonoBehaviour {

    MazeGenerator MG;

    public void OnTriggerEnter(Collider other)
    { 
        //GameObject ceiling = (GameObject)other.gameObject;
        
        if(other.tag == "ceiling")
        {
            Debug.Log(other.gameObject.ToString());
            other.gameObject.layer = 8;
            foreach(Transform child in other.transform)
            {
                child.gameObject.layer = 8;
            }
            
            //Debug.Log("Entered a new floor panel");
            //if (MG.ceilingPieces.Contains(ceiling))
           // {
            //    MG.ceilingPieces.Remove(ceiling);
            //    ceiling.layer = 8;
           // }
        }
    }
}
