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

        maxMiningT = data.GetMiningCool(slime.isSpecial);
        miningAmount = data.GetMiningAmount(slime.isSpecial);
    }

    private IEnumerator MiningRoutine()
    {
        yield return new WaitForSeconds(maxMiningT);

        SetMining();
        slime.dataManager.coin.GainCoin(miningAmount);
        slime.spawnManager.SpawnMoneyTxt(slime.body.transform, miningAmount);

        StartCoroutine(MiningRoutine());
    }
}
