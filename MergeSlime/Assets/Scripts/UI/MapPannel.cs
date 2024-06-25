using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Redcode.Pools;
using UnityEngine.UI;
using TMPro;

public class MapPannel : MonoBehaviour, IPoolObject
{
    public int ID;

    public TextMeshProUGUI titleTxt, explainTxt, buttonTxt;
    public Button buyBtn;
    public Image mapImg;

    private DataManager dataManager;
    private UIManager uiManager;
    private ES3Manager es3Manager;
    private SpawnManager spawnManager;

    public void OnCreatedInPool()
    {
        dataManager = DataManager.Instance;
        uiManager = UIManager.Instance;
        es3Manager = ES3Manager.Instance;
        spawnManager = SpawnManager.Instance;
    }

    public void OnGettingFromPool()
    {
    }

    public void SetPannel(int id)
    {
        ID = id;

        MapData data = dataManager.Find_MapData(ID);

        titleTxt.text = data.name;
        explainTxt.text = data.mapBonuses.Length == 0 ? "None\n" : "";
        foreach(Map_Bonus bonus in data.mapBonuses)
        {
            explainTxt.text += bonus.amount >= 0 ? $"{bonus.type} <color=red>+ {bonus.amount}</color>\n" :
                $"{bonus.type} <color=blue>- {-bonus.amount}</color>\n";
        }
        explainTxt.text = explainTxt.text.TrimEnd('\n');

        mapImg.sprite = data.mapSprite;

        UpdatePannel();
    }

    public void UpdatePannel()
    {
        MapData data = dataManager.Find_MapData(ID);

        if(!data.isCollect)
            buttonTxt.text = $"<sprite=0>{data.cost}";
        else if(dataManager.curMapID == ID)
        {
            buttonTxt.color = Color.black;
            buttonTxt.text = "Selected";
            buyBtn.interactable = false;
        }
        else
        {
            buttonTxt.color = Color.black;
            buttonTxt.text = "Collected";
            buyBtn.interactable = true;
        }
    }

    public void ClickButton()
    {
        MapData data = dataManager.Find_MapData(ID);

        // 구매
        if(!data.isCollect)
        {
            if (!dataManager.coin.LoseCoin((ulong)data.cost))
                return;
            dataManager.SetCollect_MapData(ID);
            spawnManager.SpawnNoticePannel("New Map", $"{data.name} is UnLock!!", 0, dataManager.Find_Sprite("Rocket"));
        }
        // 맵 바꾸기
        else
        {
            dataManager.curMapID = ID;

            uiManager.mapCollectionUI.SetMapUI(ID);
        }

        uiManager.mapCollectionUI.UpdateAllPannel();
        es3Manager.Save(SaveType.Map);
    }
}
