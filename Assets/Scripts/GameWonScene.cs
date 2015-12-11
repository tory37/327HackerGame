using UnityEngine;
using System.Collections;

public class GameWonScene : MonoBehaviour {

	void Start()
	{
		UIManager.Instance.Show( "GameWon" );
	}
}
