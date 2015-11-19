using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour 
{
    [SerializeField]
    private GameObject PlayerRef, mazeGenerator;

    private MazeGenerator mazeGeneratorRef;

    [SerializeField]
    private GameObject[] enemies;

    Cell[,] mazeRef;
    int xMax, zMax;

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
        mazeGeneratorRef = mazeGenerator.GetComponent<MazeGenerator>();
        
        mazeGeneratorRef.getMaze(out mazeRef, out xMax, out zMax);
    }

	// Use this for initialization
	void Start () 
    {
	    
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

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
