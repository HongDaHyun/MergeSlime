using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;

public class DataManager : Singleton<DataManager>
{
    [Title("저장 변수")]
    public Coin coin;

    [Title("스프라이트")]
    public SlimeSprite[] slimeSprites;
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
    public int coin;

    public void GainCoin(int amount)
    {
        coin += amount;
        UIManager.Instance.moneyUI.SetMoney(coin);
    }

    public bool LoseCoin(int amount)
    {
        if (coin < amount)
            return false;

        coin -= amount;
        UIManager.Instance.moneyUI.SetMoney(coin);
        return true;
    }
}

public enum State { Idle = 0, Pick, Merge }