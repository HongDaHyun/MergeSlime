using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mining : MonoBehaviour
{
    private const int MAXMINE = 16;
    private int maxMining;

    private Slime slime;

    private void Awake()
    {
        slime = GetComponent<Slime>();
    }

    public void ReSet()
    {
        maxMining = MAXMINE - slime.level;

        StopAllCoroutines();
        StartCoroutine(MiningRoutine());
    }

    private IEnumerator MiningRoutine()
    {
        while(true)
        {
            yield return new WaitForSeconds(maxMining);

            int amount = slime.level + 1;

            slime.dataManager.coin.GainCoin(amount);
            slime.spawnManager.SpawnMoneyTxt(slime.body.transform, amount);
        }
    }
}
