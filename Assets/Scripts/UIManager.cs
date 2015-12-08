using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	void Awake()
	{
		DontDestroyOnLoad( transform.gameObject );
	}

	public void Transition(string sceneName)
	{
		Application.LoadLevel( sceneName );
	}


}
