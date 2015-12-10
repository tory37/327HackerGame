using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum EnemyStates
{
    Wandering,
    Chasing,
    Pursuit
}

public class EnemyFSM : MonoFSM {

    public float enemySpeed;
    public Cell CurrentCell { get; set; }
    public Cell GoalCell { get; set; }
    public Vector3 Goal { get; set; }
    public int XSize { get; set; }
    public int ZSize { get; set; }

    public Rigidbody rb;

    
    public Color wanderingColor, chasingColor;

    public ParticleSystem enemyMat;

    public direction CurrentDirection { get; set; }

    protected override void Initialize()
    {
        int x, z;
        GameManager.Instance.getMazeSize(out x, out z);
        XSize = x;
        ZSize = z;
        rb = GetComponent<Rigidbody>();
        setInitialLocation();

        setInitialDirection();

    }

    void setInitialLocation()
    {
        int randomNum = Random.Range(0, 2);
        int randomX, randomZ;
        if (randomNum == 0)
        {
            randomX = Random.Range(0, XSize);
            randomZ = Random.Range(ZSize / 2, ZSize);
        }
        else
        {
            randomX = Random.Range(ZSize / 2, XSize);
            randomZ = Random.Range(0, ZSize);
        }

        float XInUnity = GameManager.Instance.getCell(randomX, randomZ).cellCenter.x;
        float ZInUnity = GameManager.Instance.getCell(randomX, randomZ).cellCenter.z;
        transform.position = new Vector3(XInUnity, 1f, ZInUnity);
        CurrentCell = GameManager.Instance.getCell(randomX, randomZ);
    }

    void setInitialDirection()
    {


        if (CurrentCell.z != 0)
        {
            if (!GameManager.Instance.getCell(CurrentCell.x, CurrentCell.z - 1).downBlocked)
            {
                CurrentDirection = direction.up;
                GoalCell = GameManager.Instance.getCell(CurrentCell.x, CurrentCell.z - 1);

            }
        }
        if (CurrentCell.x != 0)
        {
            if (!GameManager.Instance.getCell(CurrentCell.x - 1, CurrentCell.z).rightBlocked)
            {
                CurrentDirection = direction.left;
                GoalCell = GameManager.Instance.getCell(CurrentCell.x - 1, CurrentCell.z);

            }
        }
        if (!CurrentCell.rightBlocked)
        {
            CurrentDirection = direction.right;
            GoalCell = GameManager.Instance.getCell(CurrentCell.x + 1, CurrentCell.z);
        }
        else if (!CurrentCell.downBlocked)
        {
            CurrentDirection = direction.down;
            GoalCell = GameManager.Instance.getCell(CurrentCell.x, CurrentCell.z + 1);
        }

        Goal = new Vector3(GoalCell.cellCenter.x, 1f, GoalCell.cellCenter.z);
        
    }


}
