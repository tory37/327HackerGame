using UnityEngine;
using System.Collections;

public class EnemyChasingState : State
{
    direction currentDirection;

    
    private int xSize, zSize, countSinceTurn;
    private Vector3 movement;

    private EnemyFSM enemyfsm;

    public override void Initialize(MonoFSM callingfsm)
    {

    }

    public override void OnEnter()
    {
        GetComponent<Renderer>().material.color = Color.red;
    }

    public override void OnUpdate()
    {

    }
}