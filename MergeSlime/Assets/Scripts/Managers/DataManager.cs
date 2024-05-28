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

    [Title("난이도 변수", "난이도 관련 조정 가능")]
    public int MAX_MINING;

    [Title("중복 변수", "중복되어 처리될 수 있는 변수를 처리함.")]
    public int SLIME_LENGTH;
    public float SLIME_SCALE;

    [Title("스프라이트")]
    public SlimeSprite[] slimeSprites;

    protected override void Awake()
    {
        base.Awake();

        SetData();
    }

    private void SetData()
    {
        SLIME_LENGTH = slimeSprites.Length;
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

public enum State { Idle = 0, Pick, Merge }