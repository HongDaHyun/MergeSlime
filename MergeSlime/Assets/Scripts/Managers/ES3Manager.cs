using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        ES3.Save<List<int>>(SaveType.SLIMES.ToString(), SpawnManager.Instance.slimeList, SLIME_PATH);
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
            case SaveType.SLIMES:
                ES3.Save<List<int>>(type_s, SpawnManager.Instance.slimeList, SLIME_PATH);
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
            dataManager.upgrades[i].cost = upgrades[i].cost;
            dataManager.upgrades[i].amount = upgrades[i].amount;
        }

        SpawnManager spawnManager = SpawnManager.Instance;

        spawnManager.slimeList = ES3.Load<List<int>>(SaveType.SLIMES.ToString(), SLIME_PATH, new List<int>());
    }
}
