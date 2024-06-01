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
    public Upgrade[] upgradeLv;

    [Title("난이도 변수", "난이도 관련 조정 가능")]
    public int MINING_CYCLE;
    public float SPECIAL_INCREASE, SPECIAL_INCREASE_LIMIT;

    [Title("중복 변수", "시작 시 자동 처리")]
    [ReadOnly] public int SLIME_LENGTH;
    [ReadOnly] public int SLIME_S_LENGTH;
    [ReadOnly] public float LUCK_PERCENT;

    [Title("기타 변수")]
    public float SLIME_SCALE;

    [Title("스프라이트")]
    public SlimeSprite[] slimeSprites;
    public SlimeSprite[] specialSlimeSprites;

    protected override void Awake()
    {
        base.Awake();

        SetData();
    }

    private void SetData()
    {
        SLIME_LENGTH = slimeSprites.Length;
        SLIME_S_LENGTH = specialSlimeSprites.Length;
    }

    public void SetPrice()
    {
        int upPrice = Mathf.RoundToInt(spawnPrice * 1.2f);

        spawnPrice = spawnPrice != upPrice ? upPrice : spawnPrice + 1;
        UIManager.Instance.moneyUI.SetPrice(spawnPrice);
    }

    public void IncreaseUpgradeLv(int id)
    {
        switch(id)
        {
            case 0:
                if (upgradeLv[id].level > SPECIAL_INCREASE_LIMIT && !coin.LoseCoin(upgradeLv[id].cost))
                    return;

                upgradeLv[id].UpLevel(SPECIAL_INCREASE);
                break;
            case 1:
                break;
            case 2:
                break;
        }

        UIManager.Instance.SetUpgradeUI(id);
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
    public int level;
    public int cost;
    public float amount;

    public void UpLevel(float increase)
    {
        level++;
        cost = Math.Max(1, 2 * level);
        SetAmount(increase);
    }

    public void SetAmount(float increase)
    {
        amount = increase * level;
    }
}

public enum State { Idle = 0, Pick, Merge }
public enum Face { Cute, Idle, Surprise }