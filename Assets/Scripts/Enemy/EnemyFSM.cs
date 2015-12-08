using UnityEngine;
using System.Collections;

public enum EnemyStates
{
    Wandering,
    Chasing
}

public class EnemyFSM : MonoFSM {

    public float enemySpeed;

}
