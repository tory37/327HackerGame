using UnityEngine;
using System.Collections;
using System.Collections.Generic;
​
public class EnemyChasingState : State
{
    
    
    
    private Vector3 movement;
    private EnemyFSM enemyfsm;
​
    public override void Initialize(MonoFSM callingfsm)
    {
        enemyfsm = (EnemyFSM)callingfsm;
    }
​
    public override void OnEnter()
    {
        enemyfsm.CurrentCell = GameManager.Instance.GetCellPositionIsIn(transform.position);
        updateGoalCell();
        enemyfsm.enemySpeed = 8;
        enemyfsm.enemyMat.startColor = enemyfsm.chasingColor;
        
    }
​
    public override void OnUpdate()
    {
        if (enemyfsm.CanMove)
        {
            enemyfsm.CurrentCell = GameManager.Instance.GetCellPositionIsIn(transform.position);
            //checkWallBetweenPlayer();

            Vector3 playerCellPos = GameManager.Instance.GetCellPositionIsIn(GameManager.Instance.GetPlayerPosition()).cellCenter;
            movement = (playerCellPos - transform.position).normalized;
            Vector3 toMove = transform.position + movement * Time.deltaTime * enemyfsm.enemySpeed;
            enemyfsm.rb.MovePosition(toMove);


            transform.position += movement * Time.deltaTime * enemyfsm.enemySpeed;
        }
        
​
    }
​
    public override void CheckTransitions()
    {
        //if(checkWallBetweenPlayer())
        //{
        //    return;
        //}
        Collider[] col = Physics.OverlapSphere(transform.position, 12, 1024);
        if (col.Length == 0)
        {
            enemyfsm.AttemptTransition(EnemyStates.Wandering);
        }
        col = Physics.OverlapSphere(transform.position, 5, 1024);
        if(col.Length == 1)
        {
            enemyfsm.AttemptTransition(EnemyStates.Pursuit);
        }
    }
​
    public override void OnExit()
    {
        enemyfsm.GoalCell = GameManager.Instance.GetCellPositionIsIn(transform.position);
        enemyfsm.Goal = new Vector3(enemyfsm.GoalCell.cellCenter.x, 1f, enemyfsm.GoalCell.cellCenter.z);
    }
​
​
    private void updateGoalCell()
    {
        enemyfsm.GoalCell = GameManager.Instance.GetCellPositionIsIn(GameManager.Instance.GetPlayerPosition());
​
        
​
        enemyfsm.Goal = new Vector3(enemyfsm.GoalCell.cellCenter.x, 1f, enemyfsm.GoalCell.cellCenter.z);
        
​
        
        
    }
​
​
    
​
    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "wall")
        {
            enemyfsm.AttemptTransition(EnemyStates.Wandering);
        }
        
    }
​
    
​
    //private bool checkWallBetweenPlayer()
    //{
    //    int x = enemyfsm.CurrentCell.x;
    //    int z = enemyfsm.CurrentCell.z;
    //    Cell playerIsIn = GameManager.Instance.GetCellPositionIsIn(GameManager.Instance.GetPlayerPosition());
    //    //if the player is in the right cell
    //    if(playerIsIn.x == x + 1)
    //    {
    //        if(enemyfsm.CurrentCell.rightBlocked)
    //        {
    //            enemyfsm.AttemptTransition(EnemyStates.Wandering);
    //            return true;
    //        }
    //    }
    //    //if the player is in the left cell
    //    if(playerIsIn.x == x - 1)
    //    {
    //        if(playerIsIn.rightBlocked)
    //        {
    //            enemyfsm.AttemptTransition(EnemyStates.Wandering);
    //            return true;
    //        }
    //    }
    //    //if the player is in the down cell
    //    if(playerIsIn.z == z + 1)
    //    {
    //        if(enemyfsm.CurrentCell.downBlocked)
    //        {
    //            enemyfsm.AttemptTransition(EnemyStates.Wandering);
    //            return true;
    //        }
    //    }
    //    //if the player is in the up cell
    //    if(playerIsIn.z == z - 1)
    //    {
    //        if(playerIsIn.downBlocked)
    //        {
    //            enemyfsm.AttemptTransition(EnemyStates.Wandering);
    //            return true;
    //        }
    //    }
    //    return false;
        
        
    //}
​
​
    private void updateDirection()
    {
        
    }
​
    private void checkPlayerPos()
    {
        
    }
}