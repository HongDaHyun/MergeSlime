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

    private SpawnManager spawnManager;
    private DataManager dataManager;

    public void OnCreatedInPool()
    {
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

    private void SetState(State _state)
    {
        state = _state;

        switch(state)
        {
            case State.Idle:
                break;
        }
    }
}
