using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameSpecificGUI : MonoBehaviour {

	public Text inGameGuiUsername;

	public void UpdateInGameUsername(string text)
	{
		inGameGuiUsername.text = text;
	}
	
	public void CyclePUP()
	{

	}

}
