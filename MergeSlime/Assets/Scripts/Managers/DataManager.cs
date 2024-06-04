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
    public Upgrade[] upgrades;

    [Title("난이도 변수", "난이도 관련 조정 가능")]
    public int MINING_CYCLE;
    public int MAX_CONNECT_MINUTES;

    [Title("중복 변수", "시작 시 자동 처리")]
    [ReadOnly] public int SLIME_LENGTH;
    [ReadOnly] public int SLIME_S_LENGTH;

    [Title("기타 변수")]
    public float SLIME_SCALE;
    public int DEFAULT_COIN; public int DEFAULT_SPAWNPRICE;

    [Title("슬라임 변수")]
    public SlimeData[] slimeDatas;
    public SlimeData[] slimeDatas_S;

    protected override void Awake()
    {
        base.Awake();

        ES3Manager.Instance.LoadAll();

        SetData();
    }

    private void SetData()
    {
        SLIME_LENGTH = slimeDatas.Length;
        SLIME_S_LENGTH = slimeDatas_S.Length;

        ConnectReward();
    }

    private void ConnectReward()
    {
        if (lastConnect == "")
        {
            SetLastConnect();
            return;
        }

        TimeSpan timespan = DateTime.Now - Convert.ToDateTime(lastConnect);

        double proportion = Math.Min(1.0, timespan.TotalMinutes / MAX_CONNECT_MINUTES);

        coin.GainCoin((int)(upgrades[2].amount * proportion));
        SetLastConnect();
    }

    private void SetLastConnect()
    {
        lastConnect = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        ES3Manager.Instance.Save(SaveType.LastConnect);
    }

    public void SetPrice()
    {
        int upPrice = Mathf.RoundToInt(spawnPrice * 1.2f);

        spawnPrice = spawnPrice != upPrice ? upPrice : spawnPrice + 1;
        UIManager.Instance.moneyUI.SetPrice(spawnPrice);
        ES3Manager.Instance.Save(SaveType.SpawnPrice);
    }

    public SlimeData Find_SlimeData(int id, bool isSpecial)
    {
        if (isSpecial)
            return Array.Find(slimeDatas_S, slime => slime.ID == id);
        else
            return Array.Find(slimeDatas, slime => slime.ID == id);
    }
    public SlimeData Find_SlimeData_level(int level, bool isSpecial)
    {
        if (isSpecial)
            return Array.Find(slimeDatas_S, slime => slime.level == level);
        else
            return Array.Find(slimeDatas, slime => slime.level == level);
    }
    public ref SlimeData Find_SlimeData_Ref(int id, bool isSpecial)
    {
        if (isSpecial)
            return ref slimeDatas_S[Array.FindIndex(slimeDatas_S, slime => slime.ID == id)];
        else
            return ref slimeDatas[Array.FindIndex(slimeDatas, slime => slime.ID == id)];
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
        ES3Manager.Instance.Save(SaveType.Coin);
    }

    public bool LoseCoin(int _amount)
    {
        if (amount < _amount)
            return false;

        amount -= _amount;
        UIManager.Instance.moneyUI.SetMoney(amount);
        ES3Manager.Instance.Save(SaveType.Coin);
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
        ES3Manager.Instance.Save(SaveType.Upgrades);
    }

    public void SetAmount()
    {
        amount = amountIncrease * level;
    }
}

[Serializable]
public class SlimeData
{
    [Title("고유 변수", "ID 제외 수정 가능")]
    public int ID;
    public int level;
    public string name;
    public string explain;
    public Color color;
    public SlimeSprite sprite;

    [Title("저장 변수")]
    public bool isCollect;
    public int spawnCount;

    [Title("난이도 변수")]
    public int miningAmount;
    public float miningMaxCool;

    public void FindCollect()
    {
        if (isCollect)
            return;

        isCollect = true;
        UIManager.Instance.collecionPannel.SetPannel_NewCollection(level, false);
    }

    public void IncreaseSpawnCount()
    {
        spawnCount++;
    }
    public void DecreaseSpawnCount()
    {
        spawnCount--;
    }
}

public enum State { Idle = 0, Pick, Merge }
public enum Face { Cute, Idle, Surprise }
public enum SaveType { Coin, SpawnPrice, LastConnect, Upgrades, SlimeData, SlimeData_S}