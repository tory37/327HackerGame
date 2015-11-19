using UnityEngine;
using System.Collections;




public class EnemyAI : MonoBehaviour 
{
    enum direction {up, down, left, right};
    enum state { wandering, chasing };

    direction currentDirection;
    state currentState;

    Cell currentCell;
    private int xSize, zSize;

    void Awake()
    {

    }

	// Use this for initialization
	void Start () 
    {
        
        int randomCellX = Random.Range(0, xSize);
        int randomCellZ = Random.Range(0, zSize);

        // Initializing to 0, just for testing purposes. Will be random for the real game.
        currentCell = GameManager.Instance.getCell(0,0);

        //Set the initial state to wandering
        currentState = state.wandering;

        // Set the initial Direction of the enemy.
        setInitialDirection();
	}
	
	// Update is called once per frame
    void Update()
    {
        // Before enemy moves, check to see if it needs to change direction
        if (needToChangeDirection())
            changeDirection();

        // Move enemy to adjacent cell


    }


    private bool needToChangeDirection()
    {
        if(currentDirection == direction.down)
        {
            if (currentCell.downBlocked)
                return true;
        }
        if(currentDirection == direction.right)
        {
            if (currentCell.rightBlocked)
                return true;
        }
        if(currentDirection == direction.up)
        {
            if (GameManager.Instance.getCell(currentCell.x, currentCell.z - 1).downBlocked)
                return true;
        }
        if(currentDirection == direction.left)
        {
            if (GameManager.Instance.getCell(currentCell.x - 1, currentCell.z).rightBlocked)
                return true;
        }
        return false;
    }

    void changeDirection()
    {
        
    }

    // This is called only once in Start(). Putting it in a function to make Start() look more readable.
    void setInitialDirection()
    {
        if (!currentCell.downBlocked)
        {
            currentDirection = direction.down;
        }
        else if (!currentCell.rightBlocked)
        {
            currentDirection = direction.right;
        }
        else if (!GameManager.Instance.getCell(currentCell.x, currentCell.z - 1).downBlocked)
        {
            currentDirection = direction.up;
        }
        else
        {
            currentDirection = direction.left;
        }
    }


}
