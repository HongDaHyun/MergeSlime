using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Redcode.Pools;

public class Slime : MonoBehaviour, IPoolObject
{
    public SpriteRenderer shadow, body, face;
    public int level;

    private SpawnManager spawnManager;
    private DataManager dataManager;

    private State state;

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
                Move();
                StartCoroutine(Spin());
                break;
        }
    }

    private void Move()
    {
        Vector2 target = new Vector2(spawnManager.camBound.RanWidthBoundary(), spawnManager.camBound.RanHeightBoudnary());
        float timeToReachTarget = Vector3.Distance(transform.position, target) / 0.3f;

        transform.DOMove(target, timeToReachTarget).SetEase(Ease.Linear).SetUpdate(true).OnComplete(() => Move());
    }

    private IEnumerator Spin()
    {
        while (state == State.Idle)
        {
            body.transform.Rotate(Vector3.forward * 50f * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }
}
