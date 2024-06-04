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
        maxMiningT = slime.dataManager.MINING_CYCLE - (slime.dataManager.MINING_CYCLE / (float)(slime.dataManager.SLIME_LENGTH * slime.level));

        StopAllCoroutines();
        StartCoroutine(MiningRoutine());
    }

    private void SetAmount()
    {
        int increase = slime.isSpecial ? slime.level + slime.dataManager.SLIME_LENGTH : slime.level;
        miningAmount = (int)Mathf.Pow(increase, 3) + slime.dataManager.upgrades[1].amount;
    }

    private IEnumerator MiningRoutine()
    {
        yield return new WaitForSeconds(maxMiningT);

        SetAmount();
        slime.dataManager.coin.GainCoin(miningAmount);
        slime.spawnManager.SpawnMoneyTxt(slime.body.transform, miningAmount);

        StartCoroutine(MiningRoutine());
    }
}
