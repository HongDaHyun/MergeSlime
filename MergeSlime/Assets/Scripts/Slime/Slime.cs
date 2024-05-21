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

    public void OnCreatedInPool()
    {
        rigid = GetComponent<Rigidbody2D>();
        col = GetComponentInChildren<CircleCollider2D>();
        spawnManager = SpawnManager.Instance;
        dataManager = DataManager.Instance;
    }

    public void OnGettingFromPool()
    {
        SetState(State.Idle);
    }

    public void SetSlime(int _level)
    {
        level = _level;

        SlimeSprite slimeSprite = dataManager.slimeSprites[level];

        body.sprite = slimeSprite.bodySprite;
        face.sprite = slimeSprite.faceSprites[1];
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
