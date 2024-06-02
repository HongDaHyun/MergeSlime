using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;

public class DataManager : Singleton<DataManager>
{
    [Title("저장 변수")]
    public Coin coin;
    public int spawnPrice;
    public string lastConnect;
    public Upgrade[] upgradeLv;

    [Title("난이도 변수", "난이도 관련 조정 가능")]
    public int MINING_CYCLE;
    public int MAX_CONNECT_MINUTES;

    [Title("중복 변수", "시작 시 자동 처리")]
    [ReadOnly] public int SLIME_LENGTH;
    [ReadOnly] public int SLIME_S_LENGTH;

    [Title("기타 변수")]
    public float SLIME_SCALE;

    [Title("스프라이트")]
    public SlimeSprite[] slimeSprites;
    public SlimeSprite[] specialSlimeSprites;

    protected override void Awake()
    {
        base.Awake();

        // 데이터 로드 들어가야 함

        SetData();
    }

    private void SetData()
    {
        SLIME_LENGTH = slimeSprites.Length;
        SLIME_S_LENGTH = specialSlimeSprites.Length;

        ConnectReward();
    }

    private void ConnectReward()
    {
        if (lastConnect == "")
            return;

        TimeSpan timespan = DateTime.Now - Convert.ToDateTime(lastConnect);

        double proportion = Math.Min(1.0, timespan.TotalMinutes / MAX_CONNECT_MINUTES);

        coin.GainCoin((int)(upgradeLv[2].amount * proportion));
    }

    public void SetPrice()
    {
        int upPrice = Mathf.RoundToInt(spawnPrice * 1.2f);

        spawnPrice = spawnPrice != upPrice ? upPrice : spawnPrice + 1;
        UIManager.Instance.moneyUI.SetPrice(spawnPrice);
    }
}

[Serializable]
public struct SlimeSprite
{
    public Sprite bodySprite;
    public Sprite[] faceSprites;

    public Sprite FindFace(Face face)
    {
        return faceSprites[(int)face];
    }
}

[Serializable]
public struct Coin
{
    public int amount;

    public void GainCoin(int _amount)
    {
        amount += _amount;
        UIManager.Instance.moneyUI.SetMoney(amount);
    }

    public bool LoseCoin(int _amount)
    {
        if (amount < _amount)
            return false;

        amount -= _amount;
        UIManager.Instance.moneyUI.SetMoney(amount);
        return true;
    }
}

[Serializable]
public struct Upgrade
{
    [Title("저장 변수")]
    public int level;
    public int cost;
    public int amount;

    [Title("난이도 변수")]
    public int levelLimit; // -1이면 무한
    public int costIncrease;
    public int amountIncrease;

    public void UpLevel()
    {
        DataManager dataManager = DataManager.Instance;

        if ((level >= levelLimit && levelLimit != -1) || !dataManager.coin.LoseCoin(cost))
            return;

        level++;
        cost += Math.Max(1, costIncrease * level);
        SetAmount();
    }

    public void SetAmount()
    {
        amount = amountIncrease * level;
    }
}

public enum State { Idle = 0, Pick, Merge }
public enum Face { Cute, Idle, Surprise }