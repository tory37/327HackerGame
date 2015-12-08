﻿using UnityEngine;
using System.Collections;

public class EnemyWanderingState : State
{

    
    enum state { wandering, chasing };

    direction currentDirection;
    state currentState;

    Cell currentCell, goalCell;
    private int xSize, zSize, countSinceTurn;
    Vector3 movement, goal;

    EnemyFSM enemyfsm;

    void Awake()
    {
        GameManager.Instance.getMazeSize(out xSize, out zSize);

    }

    // Use this for initialization
    public override void Initialize(MonoFSM callingfsm)
    {
        enemyfsm = (EnemyFSM)callingfsm;

        //Set the initial location (random)
        setInitialLocation();

        //Set the initial state to wandering
        currentState = state.wandering;

        // Set the initial Direction of the enemy.
        setInitialDirection();
    }

    // Update is called once per frame
    public override void OnUpdate()
    {





        // Move enemy towards the goal
        movement = (goal - transform.position).normalized;

        transform.position += movement * Time.deltaTime * enemyfsm.enemySpeed;
        if ((transform.position - goal).magnitude < .1)
        {

            transform.position = goal;
            currentCell = goalCell;
            // After enemy moves, check to see if it needs to change direction
            if (needToChangeDirection())
            {
                changeDirection();
            }
            else
            {
                checkVoluntaryChangeDirection();
            }


            // Update the goal cell according to the update position.
            updateGoalCell();
        }





    }

    public override void CheckTransitions()
    {
        bool condition = false;

        if (condition)
        {
            enemyfsm.AttemptTransition(EnemyStates.Chasing);
        }
    }

    /// <summary>
    /// This function updates the goal cell.
    /// </summary>
    private void updateGoalCell()
    {
        int x = currentCell.x;
        int z = currentCell.z;
        switch (currentDirection)
        {
            case direction.left:
                goalCell = GameManager.Instance.getCell(x - 1, z);
                break;
            case direction.down:
                goalCell = GameManager.Instance.getCell(x, z + 1);
                break;
            case direction.right:
                goalCell = GameManager.Instance.getCell(x + 1, z);
                break;
            case direction.up:
                goalCell = GameManager.Instance.getCell(x, z - 1);
                break;


        }

        goal = new Vector3(goalCell.cellCenter.x, 1f, goalCell.cellCenter.z);
    }
    /// <summary>
    /// determines if the enemy HAS to change direction. (ran into a wall)
    /// </summary>
    private bool needToChangeDirection()
    {
        if (currentDirection == direction.down)
        {
            if (currentCell.downBlocked)
                return true;
        }
        if (currentDirection == direction.right)
        {
            if (currentCell.rightBlocked)
                return true;
        }
        if (currentDirection == direction.up)
        {
            if (currentCell.z == 0)
                return true;
            if (GameManager.Instance.getCell(currentCell.x, currentCell.z - 1).downBlocked)
                return true;
        }
        if (currentDirection == direction.left)
        {
            if (currentCell.x == 0)
                return true;
            if (GameManager.Instance.getCell(currentCell.x - 1, currentCell.z).rightBlocked)
                return true;
        }
        return false;
    }

    /// <summary>
    /// Method for the enemy to change direction if it is FORCED to. (run into a wall)
    /// </summary>
    void changeDirection()
    {
        while (true)
        {
            int x = currentCell.x;
            int z = currentCell.z;
            int num = Random.Range(0, 2);
            switch (currentDirection)
            {
                case direction.down:
                    //try to go left first
                    if (num == 0)
                    {
                        if (x != 0)
                        {
                            if (!GameManager.Instance.getCell(x - 1, z).rightBlocked)
                            {
                                currentDirection = direction.left;
                                return;
                            }
                        }
                        //try to go right
                        if (!currentCell.rightBlocked)
                        {
                            currentDirection = direction.right;
                            return;
                        }

                    }
                    //try to go right first
                    else
                    {
                        if (!currentCell.rightBlocked)
                        {
                            currentDirection = direction.right;
                            return;
                        }
                        //try to go left
                        if (x != 0)
                        {
                            if (!GameManager.Instance.getCell(x - 1, z).rightBlocked)
                            {
                                currentDirection = direction.left;
                                return;
                            }
                        }

                    }

                    //go up
                    currentDirection = direction.up;
                    return;


                case direction.left:
                    //try to go up first
                    if (num == 0)
                    {
                        if (z != 0)
                        {
                            if (!GameManager.Instance.getCell(x, z - 1).downBlocked)
                            {
                                currentDirection = direction.up;
                                return;
                            }
                        }
                        //try to go down
                        if (!currentCell.downBlocked)
                        {
                            currentDirection = direction.down;
                            return;
                        }

                    }
                    //try to go down first
                    else
                    {
                        if (!currentCell.downBlocked)
                        {
                            currentDirection = direction.down;
                            return;
                        }

                        //try to go up
                        if (z != 0)
                        {
                            if (!GameManager.Instance.getCell(x, z - 1).downBlocked)
                            {
                                currentDirection = direction.up;
                                return;
                            }
                        }

                    }

                    //go right
                    currentDirection = direction.right;
                    return;


                case direction.right:
                    //try to go up first
                    if (num == 0)
                    {
                        if (z != 0)
                        {
                            if (!GameManager.Instance.getCell(x, z - 1).downBlocked)
                            {
                                currentDirection = direction.up;
                                return;
                            }
                        }

                        //try to go down
                        if (!currentCell.downBlocked)
                        {
                            currentDirection = direction.down;
                            return;
                        }
                    }
                    //try to go down first
                    else
                    {
                        if (!currentCell.downBlocked)
                        {
                            currentDirection = direction.down;
                            return;
                        }

                        //try to go up
                        if (z != 0)
                        {
                            if (!GameManager.Instance.getCell(x, z - 1).downBlocked)
                            {
                                currentDirection = direction.up;
                                return;
                            }
                        }
                    }
                    //go left
                    currentDirection = direction.left;
                    return;


                case direction.up:
                    //try to go left first
                    if (num == 0)
                    {
                        if (x != 0)
                        {
                            if (!GameManager.Instance.getCell(x - 1, z).rightBlocked)
                            {
                                currentDirection = direction.left;
                                return;
                            }
                        }

                        //try to go right
                        if (!currentCell.rightBlocked)
                        {
                            currentDirection = direction.right;
                            return;
                        }
                    }
                    //try to go right first
                    else
                    {
                        if (!currentCell.rightBlocked)
                        {
                            currentDirection = direction.right;
                            return;
                        }

                        //try to go left
                        if (x != 0)
                        {
                            if (!GameManager.Instance.getCell(x - 1, z).rightBlocked)
                            {
                                currentDirection = direction.left;
                                return;
                            }
                        }
                    }

                    //go down
                    currentDirection = direction.down;
                    return;







            }
        }

    }

    /// <summary>
    /// This is called only once in Start(). Putting it in a function to make Start() look more readable.
    /// </summary>
    void setInitialDirection()
    {


        if (currentCell.z != 0)
        {
            if (!GameManager.Instance.getCell(currentCell.x, currentCell.z - 1).downBlocked)
            {
                currentDirection = direction.up;
                goalCell = GameManager.Instance.getCell(currentCell.x, currentCell.z - 1);

            }
        }
        if (currentCell.x != 0)
        {
            if (!GameManager.Instance.getCell(currentCell.x - 1, currentCell.z).rightBlocked)
            {
                currentDirection = direction.left;
                goalCell = GameManager.Instance.getCell(currentCell.x - 1, currentCell.z);

            }
        }
        if (!currentCell.rightBlocked)
        {
            currentDirection = direction.right;
            goalCell = GameManager.Instance.getCell(currentCell.x + 1, currentCell.z);
        }
        else if (!currentCell.downBlocked)
        {
            currentDirection = direction.down;
            goalCell = GameManager.Instance.getCell(currentCell.x, currentCell.z + 1);
        }

        goal = new Vector3(goalCell.cellCenter.x, 1f, goalCell.cellCenter.z);

    }

    void checkVoluntaryChangeDirection()
    {

        int random1 = Random.Range(0, 2);
        int random2 = Random.Range(0, 2);
        int x = currentCell.x;
        int z = currentCell.z;
        //if the enemy hasn't turned in 3 times, TURN.
        if (countSinceTurn == 3)
        {
            switch (currentDirection)
            {
                case direction.down:
                    //try to turn right
                    if (!currentCell.rightBlocked)
                    {
                        currentDirection = direction.right;
                        countSinceTurn = 0;
                        return;
                    }
                    //try to turn left
                    if (x != 0)
                    {
                        if (!GameManager.Instance.getCell(x - 1, z).rightBlocked)
                        {
                            currentDirection = direction.left;
                            countSinceTurn = 0;
                            return;
                        }
                    }

                    break;
                case direction.up:
                    //try to turn right
                    if (!currentCell.rightBlocked)
                    {
                        currentDirection = direction.right;
                        countSinceTurn = 0;
                        return;
                    }
                    //try to turn left
                    if (x != 0)
                    {
                        if (!GameManager.Instance.getCell(x - 1, z).rightBlocked)
                        {
                            currentDirection = direction.left;
                            countSinceTurn = 0;
                            return;
                        }
                    }

                    break;
                case direction.left:
                    //try to turn up
                    if (z != 0)
                    {
                        if (!GameManager.Instance.getCell(x, z - 1).downBlocked)
                        {
                            currentDirection = direction.up;
                            countSinceTurn = 0;
                            return;
                        }
                    }
                    //try to turn down
                    if (!currentCell.downBlocked)
                    {
                        currentDirection = direction.down;
                        countSinceTurn = 0;
                        return;
                    }

                    break;
                case direction.right:
                    //try to turn up
                    if (z != 0)
                    {
                        if (!GameManager.Instance.getCell(x, z - 1).downBlocked)
                        {
                            currentDirection = direction.up;
                            countSinceTurn = 0;
                            return;
                        }
                    }
                    //try to turn down
                    if (!currentCell.downBlocked)
                    {
                        currentDirection = direction.down;
                        countSinceTurn = 0;
                        return;
                    }

                    break;

            }
        }
        switch (currentDirection)
        {
            case direction.down:
                //check to see if the right cell is open first
                if (random1 == 0)
                {
                    if (!currentCell.rightBlocked)
                    {
                        if (random2 == 1)
                        {
                            currentDirection = direction.right;
                            countSinceTurn = 0;
                            return;
                        }
                    }
                    //check to see if the left cell is open
                    if (x != 0)
                    {
                        if (!GameManager.Instance.getCell(x - 1, z).rightBlocked)
                        {
                            if (random2 == 1)
                            {
                                currentDirection = direction.left;
                                countSinceTurn = 0;
                                return;
                            }
                        }
                    }
                }
                //check to see if the left cell is open first.
                else
                {
                    if (x != 0)
                    {
                        if (!GameManager.Instance.getCell(x - 1, z).rightBlocked)
                        {
                            if (random2 == 1)
                            {
                                currentDirection = direction.left;
                                countSinceTurn = 0;
                                return;
                            }
                        }
                    }
                    //check to see if the right cell is open
                    if (!currentCell.rightBlocked)
                    {
                        if (random2 == 1)
                        {
                            currentDirection = direction.right;
                            countSinceTurn = 0;
                            return;
                        }
                    }
                }
                break;

            case direction.up:
                //check to see if the right cell is open first
                if (random1 == 0)
                {
                    if (!currentCell.rightBlocked)
                    {
                        if (random2 == 1)
                        {
                            currentDirection = direction.right;
                            countSinceTurn = 0;
                            return;
                        }
                    }
                    //check to see if the left cell is open
                    if (x != 0)
                    {
                        if (!GameManager.Instance.getCell(x - 1, z).rightBlocked)
                        {
                            if (random2 == 1)
                            {
                                currentDirection = direction.left;
                                countSinceTurn = 0;
                                return;
                            }
                        }
                    }
                }
                //check to see if the left cell is open first.
                else
                {
                    if (x != 0)
                    {
                        if (!GameManager.Instance.getCell(x - 1, z).rightBlocked)
                        {
                            if (random2 == 1)
                            {
                                currentDirection = direction.left;
                                countSinceTurn = 0;
                                return;
                            }
                        }
                    }
                    //check to see if the right cell is open
                    if (!currentCell.rightBlocked)
                    {
                        if (random2 == 1)
                        {
                            currentDirection = direction.right;
                            countSinceTurn = 0;
                            return;
                        }
                    }
                }
                break;

            case direction.left:
                //check to see if the up cell is open first
                if (random1 == 0)
                {
                    if (z != 0)
                    {
                        if (!GameManager.Instance.getCell(x, z - 1).downBlocked)
                        {

                            if (random2 == 1)
                            {
                                currentDirection = direction.up;
                                countSinceTurn = 0;
                                return;
                            }
                        }
                    }

                    //check to see if the down cell is open
                    if (!currentCell.downBlocked)
                    {
                        if (random2 == 1)
                        {
                            currentDirection = direction.down;
                            countSinceTurn = 0;
                            return;
                        }
                    }
                }
                //Check to see if the down cell is open first
                else
                {
                    if (!currentCell.downBlocked)
                    {
                        if (random2 == 1)
                        {
                            currentDirection = direction.down;
                            countSinceTurn = 0;
                            return;
                        }
                    }

                    //Check to see if the up cell is open
                    if (z != 0)
                    {
                        if (!GameManager.Instance.getCell(x, z - 1).downBlocked)
                        {

                            if (random2 == 1)
                            {
                                currentDirection = direction.up;
                                countSinceTurn = 0;
                                return;
                            }
                        }
                    }

                }
                break;

            case direction.right:
                //check to see if the up cell is open first
                if (random1 == 0)
                {
                    if (z != 0)
                    {
                        if (!GameManager.Instance.getCell(x, z - 1).downBlocked)
                        {

                            if (random2 == 1)
                            {
                                currentDirection = direction.up;
                                countSinceTurn = 0;
                                return;
                            }
                        }
                    }

                    //check to see if the down cell is open
                    if (!currentCell.downBlocked)
                    {
                        if (random2 == 1)
                        {
                            currentDirection = direction.down;
                            countSinceTurn = 0;
                            return;
                        }
                    }
                }
                //Check to see if the down cell is open first
                else
                {
                    if (!currentCell.downBlocked)
                    {
                        if (random2 == 1)
                        {
                            currentDirection = direction.down;
                            countSinceTurn = 0;
                            return;
                        }
                    }

                    //Check to see if the up cell is open
                    if (z != 0)
                    {
                        if (!GameManager.Instance.getCell(x, z - 1).downBlocked)
                        {

                            if (random2 == 1)
                            {
                                currentDirection = direction.up;
                                countSinceTurn = 0;
                                return;
                            }
                        }
                    }

                }
                break;



        }
        countSinceTurn++;


    }

    void setInitialLocation()
    {
        int randomNum = Random.Range(0, 2);
        int randomX, randomZ;
        if (randomNum == 0)
        {
            randomX = Random.Range(0, xSize);
            randomZ = Random.Range(zSize / 2, zSize);
        }
        else
        {
            randomX = Random.Range(xSize / 2, xSize);
            randomZ = Random.Range(0, zSize);
        }

        float XInUnity = GameManager.Instance.getCell(randomX, randomZ).cellCenter.x;
        float ZInUnity = GameManager.Instance.getCell(randomX, randomZ).cellCenter.z;
        transform.position = new Vector3(XInUnity, 1f, ZInUnity);
        currentCell = GameManager.Instance.getCell(randomX, randomZ);
    }

}