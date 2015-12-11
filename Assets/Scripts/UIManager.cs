﻿using UnityEngine;
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

	private Dictionary<int, List<string>> startingMenus;

	void Awake()
	{
		Instance = this;

		startingMenus = new Dictionary<int, List<string>>();
		startingMenus.Add( 0, new List<string> { "MainMenu" } );
		startingMenus.Add( 1, new List<string> { "InGameGui" } );
		startingMenus.Add( 2, new List<string> { "InGameGui" } );
		startingMenus.Add( 3, new List<string> { "InGameGui" } );
		startingMenus.Add( 4, new List<string> { "GameWon" } );
		startingMenus.Add( 5, new List<string> { "OnWinMenu", "OnDeathMenu",  } );
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
		foreach (Canvas canvas in canvases.Where(c => !startingMenus[level].Contains(c.name)))
		{
			canvas.gameObject.SetActive( false );
		}
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

	public void SetCurrentLevel()
	{
		currentLevel = Application.loadedLevel;
	}

	public void LoadNextLevel()
	{
		Transition( currentLevel + 1 );
	}

	public void ReloadCurrentLevel()
	{
		Transition( currentLevel );
	}

	public void ReloadLevel()
	{
		Transition( Application.loadedLevel );
	}

	public void WinGame()
	{
		currentLevel = Application.loadedLevel;
		Show( "OnWinMenu" );
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
		Application.LoadLevel( "EmptyMenuScene" );

	}

	public void LoseGame()
	{
		currentLevel = Application.loadedLevel;
		Show( "OnDeathMenu" );
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
		Application.LoadLevel( "EmptyMenuScene" );
	}
}
