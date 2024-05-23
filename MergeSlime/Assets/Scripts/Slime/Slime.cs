using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Redcode.Pools;

public class Slime : MonoBehaviour, IPoolObject
{
    public SpriteRenderer shadow, body, face;
    public int level;
    public State state;

    [HideInInspector] public Rigidbody2D rigid;
    [HideInInspector] public CircleCollider2D col;
    [HideInInspector] public SpawnManager spawnManager;
    [HideInInspector] public DataManager dataManager;

    private Movement movement;

    public void OnCreatedInPool()
    {
        rigid = GetComponent<Rigidbody2D>();
        col = GetComponentInChildren<CircleCollider2D>();
        spawnManager = SpawnManager.Instance;
        dataManager = DataManager.Instance;
        movement = GetComponent<Movement>();
    }

    public void OnGettingFromPool()
    {
    }

    private void ReSet()
    {
        SetState(State.Idle);
        movement.ReSet();
    }

    private void FixedUpdate()
    {
        if (state == State.Idle)
        {
            movement.Move();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        movement.MoveReflect(collision);
        // 머지 체크 (공식 추가 해야함)
        if (movement.isDrop && collision.gameObject.CompareTag("Slime"))
        {
            movement.Merge(collision.gameObject.GetComponent<Slime>());
        }
    }

    private void OnMouseDown()
    {
        SetState(State.Pick);
    }

    private void OnMouseDrag()
    {
        movement.Drag();
    }

    public void OnMouseUp()
    {
        SetState(State.Idle);
        StartCoroutine(movement.DropRoutine());
    }

    public void SetSlime(int _level)
    {
        level = _level;

        SlimeSprite slimeSprite = dataManager.slimeSprites[level];

        body.sprite = slimeSprite.bodySprite;
        face.sprite = slimeSprite.faceSprites[1];

        ReSet();
    }

    public void SetState(State _state)
    {
        state = _state;

        switch(state)
        {
            case State.Idle:
                rigid.simulated = true;
                break;
            case State.Pick:
                rigid.simulated = false;
                break;
            case State.Merge:
                break;
        }
    }
}
