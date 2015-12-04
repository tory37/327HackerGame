using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// This struct is not actually a gameObject or anything. it will be used for
/// the representation of the maze prior to the generation of the appropriate
/// walls in the scene.
/// </summary>
public struct Cell
{
    public bool downBlocked, rightBlocked;

    public int ID;
    public int x, z;

    public Vector3 cellCenter;

    public Cell(int ID, int x, int z)
    {
        this.ID = ID;
        this.x = x;
        this.z = z;
        downBlocked = true;
        rightBlocked = true;
        //This value will need to be reassigned in cell Generation
        this.cellCenter = Vector3.zero;
    }
    

}

public class MazeGenerator : MonoBehaviour {

    /// <summary>
    /// xSize = the width of the maze
    /// zSize = the height of the maze
    /// cellWidth = the length of a side of a cell Keep it even.
    /// </summary>
    [SerializeField]
    private int xSize, zSize, cellWidth, wallHeight;

    /// <summary>
    /// This is the actual array of Cells that the maze generation algorithm yields
    /// 
    /// </summary>
    private Cell [,] theMaze;

	[SerializeField]
	private Camera miniMapCamera;

    /// <summary>
    /// the preFab that the walls will be generated from. It is suggested that the
    /// geometry of the wall is able to be stretched with minimal trouble, so that
    /// we may be able to tweak the cell size.
    /// </summary>
    [SerializeField]
    private GameObject wallPrefab, floorPrefab, ceilingPrefab;

    //lists holding the floor and ceiling prefabs used in the mini-map revelation
    public List<GameObject> ceilingPieces;

    //This bool will be set to true when the maze is done constructing and is ready to be returned
    //to the GameManager
    private bool readyToReturn = false;

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

	void Awake () 
    {
        wallPrefab.transform.localScale = new Vector3
        (
            cellWidth,
            wallHeight,
            cellWidth
        );

        floorPrefab.transform.localScale = new Vector3
        (
            cellWidth,
            wallHeight,
            cellWidth
        );

        theMaze = new Cell[xSize, zSize];
        generateMaze(ref theMaze,xSize, zSize);
        breakAdditionalWalls(ref theMaze);
        constructMaze(ref theMaze, xSize, zSize, cellWidth,1);

		placeCamera();
        //done with maze construction, so we're ready to return the maze to the Game Manager.
        readyToReturn = true;
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
                theMaze[x, z] = new Cell(currentCellID, x, z);
                
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


        //We're going to remove the bottom and right sides in the maze to allow for
        //placement of the walls
        for(int i = 0; i < xSize; i++)
        {
            theMaze[i, zSize - 1].downBlocked = true;
        }

        for(int i = 0; i < zSize; i++)
        {
            theMaze[xSize - 1, i].rightBlocked = true;
        }

    }

    /// <summary>
    /// This will actually construct the Maze within the Unity3D scene
    /// </summary>
    /// <param name="theMaze"></param>
    /// <param name="xSize"></param>
    /// <param name="zSize"></param>
    /// <param name="cellWidth"></param>
    void constructMaze(ref Cell[,] theMaze, int xSize, int zSize, int cellWidth,
        int wallWidth)
    {
        Vector3 currentCellCenter;
        int totalCellWidth = cellWidth + wallWidth;
        int totalMazeWidth = totalCellWidth * xSize;
        int totalMazeHeight = totalCellWidth * zSize;

        
        //Add the prefabs for the external walls
        for(int i = -1; i < xSize*2; i += 1)
        {
            GameObject wall = (GameObject)Instantiate(wallPrefab);
            wall.transform.position = new Vector3(i* cellWidth, 0, -cellWidth);
            GameObject ceiling = (GameObject)Instantiate(ceilingPrefab);
            ceiling.transform.position = new Vector3(i * cellWidth, wallHeight, -cellWidth);
        }
        /*
        for(int i = -1; i <= xSize*2; i += 1)
        {
            GameObject wall = (GameObject)Instantiate(wallPrefab);
            wall.transform.position = new Vector3(i, 0, 2*zSize);
        }
        */

        for(int i = 0; i < zSize*2; i += 1)
        {
            GameObject wall = (GameObject)Instantiate(wallPrefab);
            wall.transform.position = new Vector3(-cellWidth, 0, i * cellWidth);
            GameObject ceiling = (GameObject)Instantiate(ceilingPrefab);
            ceiling.transform.position = new Vector3(-cellWidth, wallHeight, i * cellWidth);
        }
        /*
        for(int i = 0; i < zSize*2; i += 1)
        {
            GameObject wall = (GameObject)Instantiate(wallPrefab);
            wall.transform.position = new Vector3(2*xSize, 0, i);
        }
        */

        //The position of the cell will be iteratively determined, starting in 
        //the first cell
        currentCellCenter = new Vector3(0, 0, 0);


        //Add the prefabs for the internal "support beams"
        for(int z = 0; z < zSize; z++)
        {
            currentCellCenter.x = 0;

            for(int x = 0; x < xSize; x++)
            {

                theMaze[x, z].cellCenter = currentCellCenter;
                /*Debug.Log(
                    "Current Cell ID: " + theMaze[x, z].ID +
                    "\n X = " + theMaze[x, z].cellCenter.x +
                    "\n Z = " + theMaze[x, z].cellCenter.z + "\n"
                    );*/

                GameObject cellFloor = (GameObject)Instantiate(floorPrefab);
                cellFloor.transform.position = new Vector3(x*2*cellWidth, 0, z*2*cellWidth);
                GameObject downWall, rightWall, centerPiece;

                //instantiate the ceiling prefab here at the same location as the floor just higher (wallHeight) 
                GameObject ceiling = (GameObject)Instantiate(ceilingPrefab);
                ceiling.transform.position = new Vector3(x*2*cellWidth, wallHeight, z*2* cellWidth);
                ceilingPieces.Add(ceiling);

                //placeing the cell's down wall
                if (theMaze[x,z].downBlocked)
                {
                    downWall = (GameObject)Instantiate(wallPrefab);
                    downWall.transform.position = new Vector3(x * 2 * cellWidth, 0, z * 2 * cellWidth + cellWidth);
                    ceiling = (GameObject)Instantiate(ceilingPrefab);
                    ceiling.transform.position = new Vector3(x * 2 * cellWidth, wallHeight, z * 2 * cellWidth + cellWidth);
                }
                else
                {
                    downWall = (GameObject)Instantiate(floorPrefab);
                    downWall.transform.position = new Vector3(x * 2 * cellWidth, 0, z * 2 * cellWidth + cellWidth);

                    //instantiate the ceiling prefab here at the same location as the floor just higher (wallHeight) 
                    ceiling = (GameObject)Instantiate(ceilingPrefab);
                    ceiling.transform.position = new Vector3(x * 2 * cellWidth, wallHeight, z * 2 * cellWidth + cellWidth);
                    ceilingPieces.Add(ceiling);
                }


                //placing the cell's right wall
                if (theMaze[x,z].rightBlocked)
                {
                    rightWall = (GameObject)Instantiate(wallPrefab);
                    rightWall.transform.position = new Vector3(x * 2 * cellWidth + cellWidth, 0, z * 2 * cellWidth);
                    ceiling = (GameObject)Instantiate(ceilingPrefab);
                    ceiling.transform.position = new Vector3(x * 2 * cellWidth + cellWidth, wallHeight, z * 2 * cellWidth);
                }
                else
                {
                    rightWall = (GameObject)Instantiate(floorPrefab);
                    rightWall.transform.position = new Vector3(x * 2 * cellWidth + cellWidth, 0, z * 2 * cellWidth);
                    //instantiate the ceiling prefab here at the same location as the floor just higher (wallHeight)
                    ceiling = (GameObject)Instantiate(ceilingPrefab);
                    ceiling.transform.position = new Vector3(x * 2 * cellWidth + cellWidth, wallHeight, z * 2 * cellWidth);
                    ceilingPieces.Add(ceiling);
                }


                centerPiece = (GameObject)Instantiate(wallPrefab);
                centerPiece.transform.position = new Vector3(x * 2 * cellWidth + cellWidth, 0, z * 2 * cellWidth + cellWidth);
                ceiling = (GameObject)Instantiate(ceilingPrefab);
                ceiling.transform.position = new Vector3(x * 2 * cellWidth + cellWidth, wallHeight, z * 2 * cellWidth + cellWidth);

                //We need to account for the width of the walls, this should do
                currentCellCenter.x += cellWidth * 2;
            }

            currentCellCenter.z += cellWidth * 2;
        }

        this.theMaze = theMaze;
        //Add the prefabs for the walls left in theMaze

        //possibly return the resultling maze so it can be referenced
        Debug.Log("Number of Ceiling Pieces: " + ceilingPieces.Count);
    }
    /// <summary>
    /// Breaks additional walls based on the size of the maze. The reason for this is we want there
    /// to be more than one way to get from one cell to another.
    /// </summary>
    public void breakAdditionalWalls(ref Cell[,] theMaze)
    {
        int numRandomWallsToBreak = (xSize + zSize) / 4;
        for (int i = 0; i < numRandomWallsToBreak; i++)
        {
            int randomX = Random.Range(1, xSize - 1);
            int randomZ = Random.Range(1, zSize - 1);
            int randomWall = Random.Range(0, 2);
            //check the down wall first
            if (randomWall == 0)
            {
                if (theMaze[randomX, randomZ].downBlocked)
                {
                    theMaze[randomX, randomZ].downBlocked = false;
                    continue;
                }
                else if (theMaze[randomX, randomZ].rightBlocked)
                {
                    theMaze[randomX, randomZ].rightBlocked = false;
                    continue;
                }
            }
            //check the right wall first
            else
            {
                if (theMaze[randomX, randomZ].rightBlocked)
                {
                    theMaze[randomX, randomZ].rightBlocked = false;
                    continue;
                }
                else if (theMaze[randomX, randomZ].downBlocked)
                {
                    theMaze[randomX, randomZ].downBlocked = false;
                    continue;
                }
            }
            i--;

        }
    }

    public void getMaze(ref Cell[,] maze, ref int x, ref int z)
    {
        //If the maze is not done constructing, don't return it.
        if (!readyToReturn)
            return;

        maze = this.theMaze;
        x = this.xSize;
        z = this.zSize;
        
            
    }

	private void placeCamera()
	{
		miniMapCamera.transform.position = new Vector3( (xSize-1) * cellWidth, 500f, (zSize-1) * cellWidth );
		//miniMapCamera.orthographicSize = xSize * zSize / Mathf.Log(zSize);
        miniMapCamera.orthographicSize = zSize * 5;

	}

    

    void consoleDebugMaze(ref Cell[,] theMaze, int xSize, int ySize)
    {
        //not necessary, methinks
    }
	
}
