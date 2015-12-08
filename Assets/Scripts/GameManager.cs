using UnityEngine;
using System.Collections;
using System;

enum direction { up, down, left, right };

public class GameManager : MonoBehaviour 
{
    [SerializeField]
    private GameObject PlayerRef, mazeGenerator, enemyPrefab;

    private MazeGenerator mazeGeneratorRef;
    
    private int numEnemies;

    private GameObject[] enemies;

    private Cell[,] mazeRef;
    private int xMax, zMax;

    private static GameManager instance;

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
        instance = this;
        
    }

	// Use this for initialization
	void Start () 
    {
        mazeGeneratorRef = mazeGenerator.GetComponent<MazeGenerator>();
        while (mazeRef == null)
        {
            mazeGeneratorRef.getMaze(ref mazeRef, ref xMax, ref zMax);
            
        }
        numEnemies = Convert.ToInt32((xMax * zMax) / 50 * 1.5);
        for(int i = 0; i < numEnemies; i++)
        {
            Instantiate(enemyPrefab);
        }
	}
	
	// Update is called once per frame
	void Update () 
    {
	    //Debug.Log("Player in Cell: " + this.GetCellPlayerIsIn().x + ", " +this.GetCellPlayerIsIn().z);
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

    public Cell getCell(int x, int z)
    {
        return instance.mazeRef[x, z];
    }

    /// <summary>
    /// Returns the cell that the player is currently in
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
    




}
