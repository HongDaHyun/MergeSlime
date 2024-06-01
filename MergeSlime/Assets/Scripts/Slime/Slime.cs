using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Redcode.Pools;

public class Slime : MonoBehaviour, IPoolObject
{
    public SpriteRenderer shadow, body, face;
    public int level;
    public State state;
    public bool isSpecial;

    [HideInInspector] public Rigidbody2D rigid;
    [HideInInspector] public CircleCollider2D col;
    [HideInInspector] public SpawnManager spawnManager;
    [HideInInspector] public DataManager dataManager;

    private Movement movement;
    [HideInInspector] public Mining mining;
    [HideInInspector] public Expression expression;

    public void OnCreatedInPool()
    {
        rigid = GetComponent<Rigidbody2D>();
        col = GetComponentInChildren<CircleCollider2D>();
        spawnManager = SpawnManager.Instance;
        dataManager = DataManager.Instance;
        movement = GetComponent<Movement>();
        mining = GetComponent<Mining>();
        expression = GetComponent<Expression>();
    }

    public void OnGettingFromPool()
    {
    }

    private void ReSet()
    {
        SetState(State.Idle);
        movement.ReSet();
        mining.ReSet();
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
        // 1 ~ 16 레벨의 슬라임 예정
        level = _level;

        // 특별한 슬라임 인가!!?
        isSpecial = Random.Range(0f, 1f) < dataManager.upgradeLv[0].amount;

        SetSprite();
        SetScale();

        ReSet();
    }

    private void SetSprite()
    {
        SlimeSprite slimeSprite = isSpecial ? dataManager.specialSlimeSprites[level - 1] : dataManager.slimeSprites[level - 1];
        body.sprite = slimeSprite.bodySprite;
    }
    private void SetScale()
    {
        float increase = isSpecial ? 0.5f : 0.1f;
        float scale = dataManager.SLIME_SCALE + ((level - 1) * increase);
        transform.localScale = new Vector3(scale, scale, scale);
    }

    public void SetState(State _state)
    {
        state = _state;

        switch(state)
        {
            case State.Idle:
                rigid.simulated = true;
                expression.SetFace(Face.Idle);
                break;
            case State.Pick:
                rigid.simulated = false;
                expression.SetFace(Face.Cute);
                break;
            case State.Merge:
                break;
        }
    }
}
