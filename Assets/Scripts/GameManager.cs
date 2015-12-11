using UnityEngine;
using System.Collections;
using System;

public enum direction { up, down, left, right };

public class GameManager : MonoBehaviour
{
	#region Editor Interface

	[SerializeField]
    private GameObject PlayerRef, mazeGenerator, enemyPrefab, goalToken, invisPowerup, stunPowerup;

	#endregion

	private MazeGenerator mazeGeneratorRef;
    
    private int numEnemies;

    private GameObject[] enemies;

    private Cell[,] mazeRef;
    private int xMax, zMax;

    private int goalX, goalZ;

    private static GameManager instance;

    private bool playerHasToken;

    private bool playerHidden;
    public bool PlayerHidden { get{return playerHidden;} set{playerHidden = value;} }

    public static GameManager Instance
    {
        get
        {
            return instance;
        }

        set
        {
            if (instance != null)
                Destroy(value.gameObject);
            else
            {
                instance = value;
            }

        }
    }

    void Awake()
    {
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;

        instance = this;
        playerHasToken = false;

		playerHidden = false;

		UIManager.Instance.Show( "InGameGui" );
    }

	// Use this for initialization
	void Start () 
    {
        //Wait for the maze to generate
        mazeGeneratorRef = mazeGenerator.GetComponent<MazeGenerator>();
        while (mazeRef == null)
        {
            mazeGeneratorRef.getMaze(ref mazeRef, ref xMax, ref zMax);
            
        }
        //start placing enemies
        numEnemies = Convert.ToInt32((xMax * zMax) / 50 * 1.5);
        int numPowerUps = Convert.ToInt32((xMax * zMax) / 100);
        enemies = new GameObject[numEnemies];
        int powerUpX;
        int powerUpZ;
        GameObject powerUp;

        for(int i = 0; i < numPowerUps; i++)
        {
            powerUpX = UnityEngine.Random.Range(0, xMax);
            powerUpZ = UnityEngine.Random.Range(0, zMax);
            powerUp = Instantiate(invisPowerup);
            Vector3 placement = getCell(powerUpX, powerUpZ).cellCenter;
            placement = new Vector3(placement.x, 1f, placement.z);

            powerUp.transform.position = placement;
            
            powerUpX = UnityEngine.Random.Range(0, xMax);
            powerUpZ = UnityEngine.Random.Range(0, zMax);
            powerUp = Instantiate(stunPowerup);
            placement = getCell(powerUpX, powerUpZ).cellCenter;
            placement = new Vector3(placement.x, 1f, placement.z);

            powerUp.transform.position = placement;
            
        }

        for(int i = 0; i < numEnemies; i++)
        {
            enemies[i] = Instantiate(enemyPrefab);

        }
        //place the goal token
        goalX = UnityEngine.Random.Range(0, xMax);
        if(goalX > xMax / 2)
        {
            goalZ = UnityEngine.Random.Range(0, zMax);
        }
        else
        {
            goalZ = UnityEngine.Random.Range(zMax / 2, zMax);
        }

        GameObject goal = Instantiate(goalToken);
        goal.transform.position = getCell(goalX, goalZ).cellCenter;

        //place the poewerUps

	}
	
	// Update is called once per frame
	void Update () 
    {
		CheckForPause();
        if(playerHasToken)
        {
            if (GetCellPositionIsIn(PlayerRef.transform.position).ID == 0)
            {
				UIManager.Instance.WinGame();

                //The Game has been won
				//switch(Application.loadedLevelName)
				//{
				//	case "Level1":
				//	{
				//		Application.LoadLevel("Level2");
				//		break;
				//	}case "Level2":
				//	{
				//		Application.LoadLevel("Level3");
				//		break;
				//	}case "Level3":
				//	{
				//		Application.Quit();
				//		break;
				//	}

				//}

            }

        }
	}

    // A way for every object in the scene to access the maze if need be in the future.
    public void getMaze(out Cell[,] theMaze, out int x, out int z)
    {
        theMaze = mazeRef;
        x = xMax;
        z = zMax;
    }

    public void getMazeSize(out int x, out int z)
    {
        x = xMax;
        z = zMax;
    }

    /// <summary>
    /// Returns the cell whose x and z values are the passed values
    /// </summary>
    /// <param name="x"></param>
    /// <param name="z"></param>
    /// <returns>A cell in the maze, or a cell with ID == -1 otherwise</returns>
    public Cell getCell(int x, int z)
    {
        if(!(x < 0 || z < 0 || x >= xMax || z >= zMax))
            return instance.mazeRef[x, z];
        return new Cell(-1, -1, -1);
    }

    /// <summary>
    /// Returns the cell that the given position is in, or a cell with ID == -1
    /// if the position is not in the maze.
    /// </summary>
    /// <returns>a Cell in the maze</returns>
    public Cell GetCellPositionIsIn(Vector3 position)
    {
        float positionX, positionZ;
        int cellX, cellZ;

        positionX = position.x + 2.5f;
        positionZ = position.z + 2.5f;

        cellX = (int) positionX / 10;
        cellZ = (int) positionZ / 10;
        return this.getCell(cellX, cellZ);

    }

	void CheckForPause()
	{
		if (Input.GetButtonDown("Pause"))
		{
			UIManager.Instance.Show("PauseMenu");
			UIManager.Instance.SetTimeScale( 0 );
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
			//Time.timeScale = 0;
		}
	}

    public Vector3 GetPlayerPosition()
    {
        return PlayerRef.transform.position;
    }

    public Cell GetCellGoalIsIn()
    {
        return getCell(goalX, goalZ);
    }

    public void NotifyPlayerHasToken()
    {
        this.playerHasToken = true;
    }

    //need to get a list of the enemies and set their speed to 0 for a few seconds
    public void StunAllEnemies()
    {
        StartCoroutine(stunEnemies());
    }

    public void HidePlayer()
    {
        StartCoroutine(invisibility());
    }

    IEnumerator invisibility()
    {
        PlayerRef.layer = 0;
        float time = 0.0f;
        while(time < 5.0f)
        {
            yield return new WaitForSeconds(.5f);
            time += .5f;
        }
        PlayerRef.layer = 10;
    }

    IEnumerator stunEnemies()
    {
        float time = 0.0f;
        for(int i = 0; i < numEnemies; i++)
        {
            enemies[i].GetComponent<EnemyFSM>().CanMove = false;
        }
        while(time < 5.0f)
        {
            yield return new WaitForSeconds(.5f);
            time += .5f;
        }
        for(int i = 0; i < numEnemies; i++)
        {
            enemies[i].GetComponent<EnemyFSM>().CanMove = true;
        }

    }
}
