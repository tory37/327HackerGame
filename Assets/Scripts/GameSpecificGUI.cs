using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameSpecificGUI : MonoBehaviour {

	public Text inGameGuiUsername;

	public Image powerupDisplay;
	public Text powerupCount;

	public Color invisibilityColor;
	public Color stunColor;

	public static GameSpecificGUI Instance
	{
		get
		{
			return instance;
		}
		set
		{
			if ( instance != null )
				Destroy( value.gameObject );
			else
				instance = value;
		}
	}
	private static GameSpecificGUI instance = null;

	private void Awake()
	{
		Instance = this;
	}

	public void UpdateInGameUsername(string text)
	{
		inGameGuiUsername.text = text;
	}
	
	public void CyclePUP(PowerUpTypes type, int number)
	{
		if ( type == PowerUpTypes.Invisibility )
			powerupDisplay.color = invisibilityColor;
		else if ( type == PowerUpTypes.Stun )
			powerupDisplay.color = stunColor;

		powerupCount.text = "x" + number;
	}

	public void UpdatePUPCount(int number)
	{
		powerupCount.text = "x" + number;
	}

}
