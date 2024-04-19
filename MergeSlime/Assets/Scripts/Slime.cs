using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Redcode.Pools;

public class Slime : MonoBehaviour, IPoolObject
{
    public SpriteRenderer shadow, body, face;

    private State state;

    public void OnCreatedInPool()
    {

    }

    public void OnGettingFromPool()
    {
    }
}
