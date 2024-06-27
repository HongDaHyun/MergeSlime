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
        StopAllCoroutines();
        StartCoroutine(MiningRoutine());
    }

    private void SetMining()
    {
        SlimeData data = slime.dataManager.Find_SlimeData_level(slime.level, slime.isSpecial);

        maxMiningT = data.GetMiningCool();
        miningAmount = data.GetMiningAmount();
    }

    private IEnumerator MiningRoutine()
    {
        yield return new WaitForSeconds(maxMiningT + Random.Range(-0.1f, 0.1f));

        SetMining();
        slime.dataManager.coin.GainCoin((ulong)miningAmount);
        slime.spawnManager.SpawnMoneyTxt(slime.body.transform, miningAmount);

        StartCoroutine(MiningRoutine());
    }
}
