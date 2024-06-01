using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mining : MonoBehaviour
{
    private float maxMiningT;
    [HideInInspector] public int miningAmount;

    private Slime slime;

    private void Awake()
    {
        slime = GetComponent<Slime>();
    }

    public void ReSet()
    {
        maxMiningT = slime.dataManager.MINING_CYCLE - (slime.dataManager.MINING_CYCLE / slime.dataManager.SLIME_LENGTH * (slime.level - 1));
        miningAmount = (int)Mathf.Pow(slime.level, 2);

        StopAllCoroutines();
        StartCoroutine(MiningRoutine());
    }

    private IEnumerator MiningRoutine()
    {
        yield return new WaitForSeconds(maxMiningT);

        slime.dataManager.coin.GainCoin(miningAmount);
        slime.spawnManager.SpawnMoneyTxt(slime.body.transform, miningAmount);

        StartCoroutine(MiningRoutine());
    }
}
