using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class UIManager : MonoBehaviour {

	[SerializeField] private List<Canvas> canvases;

	public static UIManager Instance
	{
		get {
			return instance;
		}
		set {
			if (instance == null)
				Destroy(value.gameObject);
			else
				instance = value;
		}
	}
	private static UIManager instance;

	public static Canvas CurrentCanvas
	{
		get
		{
			return currentCanvas;
		}
		private set
		{
			currentCanvas = value;
		}
	}
	private static Canvas currentCanvas;

	void Awake()
	{
		DontDestroyOnLoad( transform.gameObject );
		instance = this;
	}

	public void Transition(string sceneName)
	{
		Application.LoadLevel( sceneName );
	}

	public void ChangeMenu (string toCanvas)
	{
		Canvas first  = canvases.First( c => c.name == toCanvas );
		currentCanvas = first;
		Show( currentCanvas );
	}

	public void Show(string canvasName)
	{
		Canvas first  = canvases.FirstOrDefault( c => c.name == canvasName );
		if (first != null)
			first.gameObject.SetActive( true );
	}

	public void Show( Canvas canvas )
	{
		canvas.gameObject.SetActive(true);
	}

	public void Hide( string canvasName )
	{
		Canvas first  = canvases.FirstOrDefault( c => c.name == canvasName );
		if ( first != null )
			first.gameObject.SetActive( false );
	}

	public void Hide( Canvas canvas )
	{
		canvas.gameObject.SetActive( false );
	}

	public void SetTimeScale( float time )
	{
		Time.timeScale = time;
	}

	public void ResumeGame()
	{
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	public void QuitGame()
	{
		Application.Quit();
	}
}
