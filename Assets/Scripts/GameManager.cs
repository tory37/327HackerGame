using UnityEngine;
using System.Collections;

enum direction { up, down, left, right };

public class GameManager : MonoBehaviour 
{
    [SerializeField]
    private GameObject PlayerRef, mazeGenerator, enemyPrefab;

    private MazeGenerator mazeGeneratorRef;
    [SerializeField]
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
        for(int i = 0; i < numEnemies; i++)
        {
            Instantiate(enemyPrefab);
        }
	}
	
	// Update is called once per frame
	void Update () 
    {
	
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
    




}
