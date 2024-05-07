using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float moveSpeed;
    private Vector2 moveDir;

    private Slime slime;

    private Rigidbody2D rigid;

    private void Awake()
    {
        slime = GetComponent<Slime>();
        rigid = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        moveDir = Random.insideUnitCircle.normalized;
    }

    private void FixedUpdate()
    {
        if(slime.state == State.Idle)
        {
            // È¸Àü
            slime.body.transform.Rotate(Vector3.forward * 50f * Time.fixedDeltaTime);

            rigid.MovePosition(rigid.position + moveDir * moveSpeed * Time.fixedDeltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Border"))
        {
            Vector2 wallNormal = collision.ClosestPoint(rigid.position) - rigid.position;
            moveDir = Vector2.Reflect(moveDir, wallNormal.normalized);
        }
    }
}
