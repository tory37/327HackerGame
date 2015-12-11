using UnityEngine;
using System.Collections;

public class DDOL : MonoBehaviour {

	private void Awake()
	{
		DontDestroyOnLoad( transform.gameObject );
	}

}
