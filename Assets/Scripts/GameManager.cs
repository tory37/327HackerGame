using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour 
{
    [SerializeField]
    private GameObject PlayerRef, mazeGenerator;

    private MazeGenerator mazeGeneratorRef;

    [SerializeField]
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
        mazeGeneratorRef.getMaze(ref mazeRef, out xMax, out zMax);
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

    public Cell getCell(int x, int z)
    {
        return instance.mazeRef[x, z];
    }
    




}
