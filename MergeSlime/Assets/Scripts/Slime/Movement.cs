using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float moveSpeed;
    public State state;
    private Vector2 moveDir;

    private Slime slime;

    private void Awake()
    {
        slime = GetComponent<Slime>();
    }

    private void Start()
    {
        moveDir = Random.insideUnitCircle.normalized;
    }

    private void FixedUpdate()
    {
        if(slime.state == State.Idle)
        {
            // 회전
            slime.body.transform.Rotate(Vector3.forward * 50f * Time.fixedDeltaTime);

            slime.rigid.MovePosition(slime.rigid.position + moveDir * moveSpeed * Time.fixedDeltaTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 reflectNormal = collision.collider.ClosestPoint(slime.rigid.position) - slime.rigid.position;
        moveDir = Vector2.Reflect(moveDir, reflectNormal.normalized);
    }

    private void OnMouseDown()
    {
        slime.SetState(State.Pick);
    }

    private void OnMouseDrag()
    {
        Drag();
    }

    private void OnMouseUp()
    {
        // 수정 필요 (머지)
        slime.SetState(State.Idle);
    }

    private void Drag()
    {
        if(slime.state == State.Pick)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.x = Mathf.Clamp(mousePos.x, slime.spawnManager.camBound.Left, slime.spawnManager.camBound.Right);
            mousePos.y = Mathf.Clamp(mousePos.y, slime.spawnManager.camBound.Bottom, slime.spawnManager.camBound.Top);
            mousePos.z = 0;

            transform.position = mousePos;
        }
    }
}
