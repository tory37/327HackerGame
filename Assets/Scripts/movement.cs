using UnityEngine;
using System.Collections;

public class movement : MonoBehaviour {

    public float speed = 15f;
    Rigidbody playerRigidbody;
    public Vector3 mover;


    // Use this for initialization
    void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Move(h, v);
    }

    void Move(float h, float v)
    {
        mover.Set(h, 0.0f, v);
        mover = mover.normalized * speed * Time.deltaTime;
        playerRigidbody.MovePosition(transform.position + mover);
    }
}
