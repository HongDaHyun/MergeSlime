using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;
using System.Linq;

public class DataManager : Singleton<DataManager>
{
    [Title("저장 변수")]
    public Coin coin;
    public Luck luck;
    public SpawnPrice spawnPrice;
    public string lastConnect;
    public Upgrade[] upgrades;

    [Title("난이도 변수", "난이도 관련 조정 가능")]
    public float INCREASE_SPAWNPRICE; // 스폰가격 증가폭 (~배 / ex. 1.2f -> 1.2배)
    public int MAX_MINING_CYCLE; // 최대 마이닝 쿨다운(레벨 1일 때)
    public float LEVEL_PER_MINING_REDUCE_COOLDOWN; // 레벨 당 쿨다운 감소량
    public float LEVEL_PER_MINING_INCREASE_AMOUNT; // 레벨 당 마이닝 증가량
    public float SPECIAL_MINING_COOLDOWN_INCEASE_AMOUNT; // 스페셜 슬라임 쿨다운 감소량의 증가배수 (~배 / ex. 2배, 2.5배 등)
    public int MAX_CONNECT_MINUTES; // 최대 접속 보상 시간
    public int MIN_CONNECT_MINUTES; // 최소 접속 보상 시간
    public float MAX_LUCK; // 최대 운 (0.7 -> 70%)

    [Title("중복 변수", "시작 시 자동 처리")]
    [ReadOnly] public int SLIME_LENGTH;
    [ReadOnly] public int SLIME_S_LENGTH;

    [Title("기타 변수")]
    public float SLIME_SCALE;
    public ulong DEFAULT_COIN;

    [Title("슬라임 변수")]
    public SlimeData[] slimeDatas;
    public SlimeData[] slimeDatas_S;

    [Title("스프라이트")]
    public SpriteData[] spriteDatas;

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

        luck.UpdateLevel();
    }

    public ulong ConnectReward()
    {
        if (lastConnect == "")
        {
            SetLastConnect();
            return 0;
        }

        TimeSpan timespan = DateTime.Now - Convert.ToDateTime(lastConnect);
        double totalMin = timespan.TotalMinutes;
        SetLastConnect();

        if (totalMin < MIN_CONNECT_MINUTES)
            return 0;

        double proportion = Math.Min(1.0, totalMin / MAX_CONNECT_MINUTES);
        ulong reward = (ulong)(upgrades[2].amount * proportion);

        coin.GainCoin(reward);

        return reward;
    }

    private void SetLastConnect()
    {
        lastConnect = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        ES3Manager.Instance.Save(SaveType.LastConnect);
    }

    public SlimeData Find_SlimeData(int id)
    {
        SlimeData slimeData = Array.Find(slimeDatas_S, slime => slime.ID == id);
        if (slimeData != null)
        {
            return slimeData;
        }
        return Array.Find(slimeDatas, slime => slime.ID == id);
    }
    public ref SlimeData Find_SlimeData_Ref(int id)
    {
        int index = Array.FindIndex(slimeDatas_S, slime => slime.ID == id);
        if (index != -1)
        {
            return ref slimeDatas_S[index];
        }

        index = Array.FindIndex(slimeDatas, slime => slime.ID == id);
        if (index != -1)
        {
            return ref slimeDatas[index];
        }

        throw new KeyNotFoundException($"SlimeData with ID {id} not found.");
    }
    public SlimeData Find_SlimeData_level(int level, bool isSpecial)
    {
        if (isSpecial)
            return Array.Find(slimeDatas_S, slime => slime.level == level);
        else
            return Array.Find(slimeDatas, slime => slime.level == level);
    }

    public Sprite Find_Sprite(string name)
    {
        return Array.Find(spriteDatas, data => data.name == name).sprite;
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
    public ulong amount;

    public void GainCoin(ulong _amount)
    {
        amount += _amount;
        UIManager.Instance.moneyUI.SetMoney(amount);
        ES3Manager.Instance.Save(SaveType.Coin);
    }

    public bool LoseCoin(ulong _amount)
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
public struct SpawnPrice
{
    [Title("저장 데이터")]
    public ulong spawnLevel;

    [Title("난이도 데이터")]
    public uint DEFAULT_SPAWNPRICE;
    public float INCREASE_SPAWNPRICE;

    public ulong GetPrice()
    {
        return (ulong)Mathf.RoundToInt(DEFAULT_SPAWNPRICE * Mathf.Pow(INCREASE_SPAWNPRICE, spawnLevel));
    }

    public void UpLevel()
    {
        spawnLevel++;
        UIManager.Instance.moneyUI.SetPrice(GetPrice());
        ES3Manager.Instance.Save(SaveType.SpawnLevel);
    }
}

[Serializable]
public struct Upgrade
{
    [Title("저장 변수")]
    public int level;

    [Title("난이도 변수")]
    public int cost;
    public int amount;
    public int costIncrease;
    public int amountIncrease;

    public void UpLevel()
    {
        level++;
        SetCost();
        SetAmount();
        ES3Manager.Instance.Save(SaveType.Upgrades);
    }

    public void SetCost()
    {
        cost += costIncrease * level;
    }
    public void SetAmount()
    {
        amount += amountIncrease * level;
    }

    public int NextAmountIncrease()
    {
        return amountIncrease * (level + 1);
    }
}

[Serializable]
public struct Luck
{
    public int level; // 찾은 슬라임 수

    public void UpdateLevel()
    {
        level = 0;

        DataManager dataManager = DataManager.Instance;

        foreach (SlimeData data in dataManager.slimeDatas)
            if (data.isCollect)
                level++;
        foreach (SlimeData data in dataManager.slimeDatas_S)
            if (data.isCollect)
                level++;
    }

    public float GetAmount()
    {
        DataManager dataManager = DataManager.Instance;
        return Mathf.Floor(dataManager.MAX_LUCK / (dataManager.SLIME_LENGTH + dataManager.SLIME_S_LENGTH) * level * 100f) / 100f;
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
    [ReadOnly] public bool isSpecial;
    public Color color;
    public SlimeSprite sprite;

    [Title("저장 변수")]
    public bool isCollect;
    public int spawnCount;

    public void FindCollect()
    {
        if (isCollect)
            return;

        UIManager uiManager = UIManager.Instance;

        isCollect = true;
        DataManager.Instance.luck.UpdateLevel();
        uiManager.collectionPannel.SetPannel_NewCollection(level, isSpecial);
        uiManager.collectionUI.UpdateUI(ID);
    }

    public void IncreaseSpawnCount()
    {
        spawnCount++;
    }
    public void DecreaseSpawnCount()
    {
        spawnCount--;
    }

    public int GetMiningAmount()
    {
        DataManager dataManager = DataManager.Instance;

        float baseAmount = isSpecial ? Mathf.Pow(level + dataManager.SLIME_LENGTH, dataManager.LEVEL_PER_MINING_INCREASE_AMOUNT) : 
            Mathf.Pow(level, dataManager.LEVEL_PER_MINING_INCREASE_AMOUNT);
        return Mathf.RoundToInt(baseAmount + baseAmount * dataManager.upgrades[1].amount / 100);
    }
    public float GetMiningCool()
    {
        DataManager dataManager = DataManager.Instance;

        float reduceAmount = isSpecial ? dataManager.LEVEL_PER_MINING_REDUCE_COOLDOWN * dataManager.SPECIAL_MINING_COOLDOWN_INCEASE_AMOUNT
            : dataManager.LEVEL_PER_MINING_REDUCE_COOLDOWN;

        return dataManager.MAX_MINING_CYCLE - reduceAmount * level;
    }
}

[Serializable]
public struct SpriteData
{
    public string name;
    public Sprite sprite;
}

public enum State { Idle = 0, Pick, Merge }
public enum Face { Cute, Idle, Surprise }
public enum SaveType { Coin, SpawnLevel, LastConnect, Upgrades, SlimeData, SlimeData_S}