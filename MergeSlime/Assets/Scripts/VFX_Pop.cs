using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Redcode.Pools;

public class VFX_Pop : MonoBehaviour, IPoolObject
{
    SpawnManager sm;

    public void OnCreatedInPool()
    {
        sm = SpawnManager.Instance;
    }

    public void OnGettingFromPool()
    {
    }

    public void PoolFalse()
    {
        sm.DeSpawnPop(this);
    }
}
