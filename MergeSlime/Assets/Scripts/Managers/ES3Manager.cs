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

        ES3.Save<ulong>(SaveType.Coin.ToString(), data.coin.amount, DATA_PATH);
        ES3.Save<string>(SaveType.LastConnect.ToString(), data.lastConnect, DATA_PATH);
        ES3.Save<Level[]>(SaveType.Level.ToString(), data.levels, DATA_PATH);
        ES3.Save<SlimeData[]>(SaveType.SlimeData.ToString(), data.slimeDatas, SLIME_PATH);
        ES3.Save<SlimeData[]>(SaveType.SlimeData_S.ToString(), data.slimeDatas_S, SLIME_PATH);
        ES3.Save<MapData[]>(SaveType.Map.ToString(), data.mapDatas, DATA_PATH);
        ES3.Save<int>(SaveType.MapID.ToString(), data.curMapID, DATA_PATH);
    }
    public void Save(SaveType type)
    {
        DataManager data = DataManager.Instance;

        string type_s = type.ToString();

        switch (type)
        {
            case SaveType.Coin:
                ES3.Save<ulong>(type_s, data.coin.amount, DATA_PATH);
                break;
            case SaveType.LastConnect:
                ES3.Save<string>(type_s, data.lastConnect, DATA_PATH);
                break;
            case SaveType.SlimeData:
                ES3.Save<SlimeData[]>(SaveType.SlimeData.ToString(), data.slimeDatas, SLIME_PATH);
                ES3.Save<SlimeData[]>(SaveType.SlimeData_S.ToString(), data.slimeDatas_S, SLIME_PATH);
                break;
            case SaveType.Level:
                ES3.Save<Level[]>(type_s, data.levels, DATA_PATH);
                break;
            case SaveType.Map:
                ES3.Save<MapData[]>(SaveType.Map.ToString(), data.mapDatas, DATA_PATH);
                ES3.Save<int>(SaveType.MapID.ToString(), data.curMapID, DATA_PATH);
                break;
        }
    }

    public void LoadAll()
    {
        DataManager dataManager = DataManager.Instance;

        dataManager.coin.amount = ES3.Load<ulong>(SaveType.Coin.ToString(), DATA_PATH, dataManager.DEFAULT_COIN);
        dataManager.lastConnect = ES3.Load<string>(SaveType.LastConnect.ToString(), DATA_PATH, "");
        dataManager.curMapID = ES3.Load<int>(SaveType.MapID.ToString(), DATA_PATH, 0);

        Level[] levels = ES3.Load<Level[]>(SaveType.Level.ToString(), DATA_PATH, dataManager.levels);
        for(int i = 0; i < dataManager.levels.Length; i++)
        {
            if (levels[i] == null)
                continue;
            if (levels[i].level > 0)
                dataManager.Find_Level_Ref(levels[i].type).level = levels[i].level;
        }

        MapData[] maps = ES3.Load<MapData[]>(SaveType.Map.ToString(), DATA_PATH, dataManager.mapDatas);
        for (int i = 0; i < dataManager.mapDatas.Length; i++)
        {
            if (maps[i] == null)
                continue;
            if (maps[i].isCollect)
                dataManager.SetCollect_MapData(maps[i].ID);
        }

        for (int i = 0; i < dataManager.upgrades.Length; i++)
        {
            if(dataManager.Find_Level(dataManager.upgrades[i].type) > 0)
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
