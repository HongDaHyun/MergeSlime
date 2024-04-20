using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnManager : Singleton<BtnManager>
{
    public void SpawnSlimeBtn()
    {
        SpawnManager sm = SpawnManager.Instance;
        CameraBound camBound = sm.camBound;

        Vector2 ranVec = new Vector2(Random.Range(camBound.Left + 1, camBound.Right - 1), Random.Range(camBound.Bottom + 1, camBound.Top - 1));

        sm.SpawnSlime(0, ranVec);
    }
}
