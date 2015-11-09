using UnityEngine;
using System.Collections;

public class MazeGenerator : MonoBehaviour {

    /// <summary>
    /// xSize = the width of the maze
    /// zSize = the height of the maze
    /// cellWidth = the length of a side of a cell Keep it even.
    /// </summary>
    [SerializeField]
    private int xSize, zSize, cellWidth;

    /// <summary>
    /// This is the actual array of Cells that the maze generation algorithm yields
    /// 
    /// </summary>
    private Cell [,] theMaze;

    /// <summary>
    /// the preFab that the walls will be generated from. It is suggested that the
    /// geometry of the wall is able to be stretched with minimal trouble, so that
    /// we may be able to tweak the cell size.
    /// </summary>
    [SerializeField]
    private Transform wallPrefab;


    /// <summary>
    /// This struct is not actually a gameObject or anything. it will be used for
    /// the representation of the maze prior to the generation of the appropriate
    /// walls in the scene.
    /// </summary>
    public struct Cell
    {
        public bool downBlocked, rightBlocked;

        public int ID;

        public Cell(int ID)
        {
            this.ID = ID;
            downBlocked = true;
            rightBlocked = true;
        }
    }
    
    /// <summary>
    /// This Disjoint Set implements the Union/ Find algorithms with path 
    /// compression
    /// </summary>
    public class DisjointSet
    {
        int size;
        int numberOfPartitions;
        int[] theSet;

        /// <summary>
        /// Creates a disjoint set of size size, with each element initially in its
        /// own unique partition.
        /// </summary>
        /// <param name="size">the number of elements in the set</param>
        public DisjointSet(int size)
        {
            this.size = size;
            this.numberOfPartitions = size;
            theSet = new int[size];
            for(int i = 0; i < size; i++)
            {
                theSet[i] = -1;
            }
        }

        public int Partitions
        {
            get { return numberOfPartitions; }
        }

        public int Size
        {
            get { return size; }
        }

        public void union(int part1, int part2)
        {
            int part1Root = this.find(part1);
            int part2Root = this.find(part2);

            if (part1Root == part2Root)
                return;

            if(theSet[part1Root] <= theSet[part2Root])
            {
                theSet[part1Root] += theSet[part2Root];
                theSet[part2Root] = part1Root;
            }
            else
            {
                theSet[part2Root] += theSet[part1Root];
                theSet[part1Root] = part2Root;
            }

            numberOfPartitions--;
        }

        public int find(int element)
        {
            if (theSet[element] < 0)
            {
                return element;
            }
            else
            {
                //This indicates the code has broken horribly and execution should
                //not be allowed to continue
                if (element == theSet[element]) throw new System.Exception();

                theSet[element] = find(theSet[element]);
                return theSet[element];
            }
        }

        /// <summary>
        /// Returns whether all items in the set are in the same partition
        /// </summary>
        /// <returns>boolean signifying whether condition is met</returns>
        public bool allInOne()
        {
            int first = find(this.theSet[0]);
            for(int i = 1; i < size; i++)
            {
                if (this.find(theSet[i]) != first) return false;
            }
            return true;
        }
    }

	// Use this for initialization
	void Start () 
    {
        theMaze = new Cell[xSize, zSize];
        generateMaze(ref theMaze,xSize, zSize);
        constructMaze(ref theMaze, xSize, zSize, cellWidth);
	}

    /// <summary>
    /// This generates a 2D array of cells representing the actual position of walls
    /// within the maze.
    /// </summary>
    /// <param name="theMaze">The 2D array of Cells to be used</param>
    /// <param name="xSize">The </param>
    /// <param name="zSize"></param>
    void generateMaze(ref Cell[,] theMaze, int xSize, int zSize)
    {
        //Generate the Disjoint set for the maze
        int totalCells = xSize * zSize;
        DisjointSet mazePartitions = new DisjointSet(totalCells);
        int currentCellID = 0;
        
        //These will be used in the maze generation loop.
        int randomCellX;
        int randomCellZ;
        int randomCellID;


        //Generate the default "All walls filled" maze
        for(int z = 0; z < zSize; z++)
        {
            for(int x = 0; x < xSize; x++)
            {
                theMaze[x, z] = new Cell(currentCellID);
                currentCellID++;
            }
        }

        //randomly fuze two adjacent cells until the maze is fully constructed
        while (mazePartitions.Partitions != 1)
        {
            randomCellX = Random.Range(0, xSize);
            randomCellZ = Random.Range(0, zSize);
            randomCellID = theMaze[randomCellX, randomCellZ].ID;

            int directionPicked = Random.Range(0, 4);

            //If 0, we break the right wall
            if(directionPicked == 0 
                && randomCellX < xSize -1 
                && theMaze[randomCellX,randomCellZ].rightBlocked)
            {
                int otherCell = theMaze[randomCellX + 1, randomCellZ].ID;
                if (mazePartitions.find(randomCellID) 
                    != mazePartitions.find(otherCell))
                {
                    theMaze[randomCellX, randomCellZ].rightBlocked = false;
                    mazePartitions.union(randomCellID, otherCell);
                }
            }
            //If 1, we break the down wall
            else if (directionPicked == 1 
                && randomCellZ < zSize -1
                && theMaze[randomCellX, randomCellZ].downBlocked)
            {
                int otherCell = theMaze[randomCellX, randomCellZ + 1].ID;
                if (mazePartitions.find(randomCellID) 
                    != mazePartitions.find(otherCell))
                {
                    theMaze[randomCellX, randomCellZ].downBlocked = false;
                    mazePartitions.union(randomCellID, otherCell);
                }
            }
            //If 2, we break the left wall
            else if(directionPicked == 2 
                && randomCellX > 0
                && theMaze[randomCellX -1, randomCellZ].rightBlocked)
            {
                int otherCell = theMaze[randomCellX - 1, randomCellZ].ID;
                if (mazePartitions.find(randomCellID) 
                    != mazePartitions.find(otherCell))
                {
                    theMaze[randomCellX - 1, randomCellZ].rightBlocked = false;
                    mazePartitions.union(randomCellID, otherCell);
                }
            }
            //If 3, we break the up wall
            else if(directionPicked == 3 
                && randomCellZ > 0
                && theMaze[randomCellX, randomCellZ -1].downBlocked)
            {
                int otherCell = theMaze[randomCellX, randomCellZ -1 ].ID;
                if (mazePartitions.find(randomCellID) 
                    != mazePartitions.find(otherCell))
                {
                    theMaze[randomCellX, randomCellZ - 1].downBlocked = false;
                    mazePartitions.union(randomCellID, otherCell);
                }
            }
        
        }

    }

    /// <summary>
    /// This will actually construct the Maze within the Unity3D scene
    /// </summary>
    /// <param name="theMaze"></param>
    /// <param name="xSize"></param>
    /// <param name="zSize"></param>
    /// <param name="cellWidth"></param>
    void constructMaze(ref Cell[,] theMaze, int xSize, int zSize, int cellWidth)
    {
        //Add the prefabs for the external walls

        //Add the prefabs for the internal "support beams"

        //Add the prefabs for the walls left in theMaze

        //possibly return the resultling maze so it can be referenced
    }

    void consoleDebugMaze(ref Cell[,] theMaze, int xSize, int ySize)
    {

    }
	
}
