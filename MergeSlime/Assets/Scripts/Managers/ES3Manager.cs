using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

public class ES3Manager : Singleton<ES3Manager>
{
    public string DATA_PATH;
    public string SLIME_PATH;

    public void SaveAll()
    {
        DataManager data = DataManager.Instance;

        ES3.Save<int>(SaveType.Coin.ToString(), data.coin.amount, DATA_PATH);
        ES3.Save<int>(SaveType.SpawnPrice.ToString(), data.spawnPrice, DATA_PATH);
        ES3.Save<string>(SaveType.LastConnect.ToString(), data.lastConnect, DATA_PATH);
        ES3.Save<Upgrade[]>(SaveType.Upgrades.ToString(), data.upgrades, DATA_PATH);
        ES3.Save<SlimeData[]>(SaveType.SlimeData.ToString(), data.slimeDatas, SLIME_PATH);
        ES3.Save<SlimeData[]>(SaveType.SlimeData_S.ToString(), data.slimeDatas_S, SLIME_PATH);
    }
    public void Save(SaveType type)
    {
        DataManager data = DataManager.Instance;

        string type_s = type.ToString();

        switch (type)
        {
            case SaveType.Coin:
                ES3.Save<int>(type_s, data.coin.amount, DATA_PATH);
                break;
            case SaveType.SpawnPrice:
                ES3.Save<int>(type_s, data.spawnPrice, DATA_PATH);
                break;
            case SaveType.LastConnect:
                ES3.Save<string>(type_s, data.lastConnect, DATA_PATH);
                break;
            case SaveType.Upgrades:
                ES3.Save<Upgrade[]>(type_s, data.upgrades, DATA_PATH);
                break;
            case SaveType.SlimeData:
                ES3.Save<SlimeData[]>(SaveType.SlimeData.ToString(), data.slimeDatas, SLIME_PATH);
                ES3.Save<SlimeData[]>(SaveType.SlimeData_S.ToString(), data.slimeDatas_S, SLIME_PATH);
                break;
        }
    }

    public void LoadAll()
    {
        DataManager dataManager = DataManager.Instance;

        dataManager.coin.amount = ES3.Load<int>(SaveType.Coin.ToString(), DATA_PATH, dataManager.DEFAULT_COIN);
        dataManager.spawnPrice = ES3.Load<int>(SaveType.SpawnPrice.ToString(), DATA_PATH, dataManager.DEFAULT_SPAWNPRICE);
        dataManager.lastConnect = ES3.Load<string>(SaveType.LastConnect.ToString(), DATA_PATH, "");

        Upgrade[] upgrades = ES3.Load<Upgrade[]>(SaveType.Upgrades.ToString(), DATA_PATH, dataManager.upgrades);
        for (int i = 0; i < upgrades.Length; i++)
        {
            dataManager.upgrades[i].level = upgrades[i].level;
            if(upgrades[i].level > 0)
            {
                dataManager.upgrades[i].SetCost();
                dataManager.upgrades[i].SetAmount();
            }
        }

        SlimeData[] loadDatas = ES3.Load<SlimeData[]>(SaveType.SlimeData.ToString(), SLIME_PATH, dataManager.slimeDatas);
        SlimeData[] loadDatas_S = ES3.Load<SlimeData[]>(SaveType.SlimeData_S.ToString(), SLIME_PATH, dataManager.slimeDatas_S);

        foreach (SlimeData data in loadDatas)
        {
            dataManager.Find_SlimeData_Ref(data.ID).isCollect = data.isCollect;
            dataManager.Find_SlimeData_Ref(data.ID).isSpecial = false;
            if (data.spawnCount > 0 && data.isCollect)
                dataManager.Find_SlimeData_Ref(data.ID).spawnCount = data.spawnCount;
        }
        foreach (SlimeData data in loadDatas_S)
        {
            dataManager.Find_SlimeData_Ref(data.ID).isCollect = data.isCollect;
            dataManager.Find_SlimeData_Ref(data.ID).isSpecial = true;
            if (data.spawnCount > 0 && data.isCollect)
                dataManager.Find_SlimeData_Ref(data.ID).spawnCount = data.spawnCount;
        }
        // 레벨 기준 오름차순 정렬
        Array.Sort(dataManager.slimeDatas, (x, y) => x.level.CompareTo(y.level));
        Array.Sort(dataManager.slimeDatas_S, (x, y) => x.level.CompareTo(y.level));
    }
}
