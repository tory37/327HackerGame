using UnityEngine;
using System.Collections;

public class EnemyChasingState : State
{
    direction currentDirection;

    
    private int xSize, zSize, countSinceTurn;
    private Vector3 movement;
    private bool playerInCell = false;
    private EnemyFSM enemyfsm;

    public override void Initialize(MonoFSM callingfsm)
    {
        enemyfsm = (EnemyFSM)callingfsm;
    }

    public override void OnEnter()
    {
        GetComponent<Renderer>().material.color = Color.red;
        updateGoalCell();
    }

    public override void OnUpdate()
    {
        getMovement();

        

        transform.position += movement * Time.deltaTime * enemyfsm.enemySpeed;
        if ((transform.position - enemyfsm.Goal).magnitude < .1)
        {
            transform.position = enemyfsm.Goal;
            enemyfsm.CurrentCell = enemyfsm.GoalCell;


            updateGoalCell();

        }
    }

    public override void CheckTransitions()
    {
        Collider[] col = Physics.OverlapSphere(transform.position, 12, 1024);
        if (col.Length == 0)
        {
            enemyfsm.AttemptTransition(EnemyStates.Wandering);
        }
    }


    private void updateGoalCell()
    {
        Cell buffer = enemyfsm.GoalCell;
        enemyfsm.GoalCell = GameManager.Instance.GetCellPositionIsIn(GameManager.Instance.GetPlayerPosition());

        if(buffer.ID == enemyfsm.GoalCell.ID)
        {
            playerInCell = true;
        }

        enemyfsm.Goal = new Vector3(enemyfsm.GoalCell.cellCenter.x, 1f, enemyfsm.GoalCell.cellCenter.z);
        
    }


    private void getMovement()
    {
        if(playerInCell)
        {
            
            movement = (GameManager.Instance.GetPlayerPosition() - transform.position).normalized;
            if(enemyfsm.CurrentCell.ID != GameManager.Instance.GetCellPositionIsIn(GameManager.Instance.GetPlayerPosition()).ID)
            {
                playerInCell = false;
                updateGoalCell();
            }
        }
        else
        {
            movement = (enemyfsm.Goal - transform.position).normalized;
        }
    }
}
