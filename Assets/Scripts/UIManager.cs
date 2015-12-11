using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class UIManager : MonoBehaviour {

	[SerializeField] private List<Canvas> canvases;

	private int currentLevel = 0;

	public static UIManager Instance
	{
		get {
			return instance;
		}
		set {
			if (instance != null)
				Destroy(value.gameObject);
			else
				instance = value;
		}
	}
	private static UIManager instance = null;

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
		Instance = this;
	}

	public void Transition(int sceneIndex)
	{
		StartCoroutine( ITransition( sceneIndex ) );
	}

	IEnumerator ITransition( int sceneIndex )
	{
		Show( "Curtain" );
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		currentLevel = sceneIndex;
		Application.LoadLevel( sceneIndex );
	}

	void OnLevelWasLoaded( int level )
	{
		if ( level == currentLevel )
			Hide( "Curtain" );
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

	public void LoadNextLevel()
	{
		Transition( GameManager.Instance.NextLevel );
	}
}
