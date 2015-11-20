using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

    public Transform target;
	
	// Update is called once per frame
	void LateUpdate () {
        transform.position = new Vector3(target.position.x, target.position.y + 50, target.position.z);
	}
}
