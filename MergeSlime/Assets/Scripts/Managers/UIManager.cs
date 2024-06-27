using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using TMPro;
using System;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : Singleton<UIManager>
{
    [Title("스크린"," ScreenCanvas에서 사용하는 UI")]
    public MoneyUI moneyUI;
    public SpawnLimitUI spawnLimitUI;

    [Title("패널", "PannelCanvas에서 사용하는 UI")]
    public GameObject raycastPannel;

    [Title("인게임")]
    public UpgradePannel[] upgradePannels;
    public CollectionPannel collectionPannel;
    public CollectionUI collectionUI;
    public MapCollectionUI mapCollectionUI;

    private void Start()
    {
        CameraBound camBound = SpawnManager.Instance.camBound;
        mapCollectionUI.bgCanvas.GetComponent<RectTransform>().sizeDelta = new Vector2(camBound.Width, camBound.Height);

        SetUI();
    }

    private void SetUI()
    {
        moneyUI.SetUI();
        SetUpgradeUI_All();

        collectionUI.SetUI();
        ConnectRewardUI();
        mapCollectionUI.SetUI();
    }

    public void SetUpgradeUI_All()
    {
        for (int i = 0; i < upgradePannels.Length; i++)
            SetUpgradeUI(i);
    }
    public void SetUpgradeUI(int id)
    {
        DataManager dataManager = DataManager.Instance;

        Upgrade upgrade = dataManager.upgrades[id];

        switch(id)
        {
            case 0:
                upgradePannels[0].UpdateExplain($"{upgrade.GetAmount()}<color=grey>(+{upgrade.INCREASE_AMOUNT})</color>");
                spawnLimitUI.UpdateTxt();
                break;
            case 1:
                upgradePannels[id].UpdateExplain($"x{upgrade.GetAmount()}<color=grey>(+{upgrade.INCREASE_AMOUNT})</color>%");
                break;
            case 2:
                upgradePannels[id].UpdateExplain($"{upgrade.GetAmount()}<color=grey>(+{upgrade.INCREASE_AMOUNT})</color>");
                break;
        }

        upgradePannels[id].UpdateButtonUI(dataManager.Find_Level(upgrade.type), dataManager.Find_BonusLevel(upgrade.type), upgrade.GetCost());
    }

    public void ConnectRewardUI()
    {
        DataManager dataManager = DataManager.Instance;

        ulong reward = dataManager.ConnectReward();

        if (reward > 0)
            SpawnManager.Instance.SpawnNoticePannel("Connect Reward", "The slimes have been making money while you were away!", reward, dataManager.Find_Sprite("Coin"));
    }

    public string GetCoinUnit(ulong amount)
    {
        ulong amountBig = amount;
        ulong amountSmall = 0;
        char unitChar = 'a';

        while (amountBig >= 1000)
        {
            amountSmall = amountBig % 1000;
            amountBig /= 1000;
            unitChar++;
        }

        return unitChar > 'a' ? $"{amountBig}.{amountSmall}{unitChar}" : $"{amountBig}{unitChar}";
    }
}

[Serializable]
public struct MoneyUI
{
    public TextMeshProUGUI coinTxt;
    public TextMeshProUGUI priceTxt;

    public void SetUI()
    {
        DataManager dataManager = DataManager.Instance;

        SetMoney();
        SetPrice(dataManager.spawnPrice.GetPrice());
    }

    public void SetMoney()
    {
        coinTxt.text = $"<sprite=0>{UIManager.Instance.GetCoinUnit(DataManager.Instance.coin.amount)}";
    }

    public void SetPrice(ulong price)
    {
        priceTxt.text = $"<sprite=0>{UIManager.Instance.GetCoinUnit(price)}";
    }
}

[Serializable]
public struct SpawnLimitUI
{
    public TextMeshProUGUI text;

    public void UpdateTxt()
    {
        text.text = $"{SpawnManager.Instance.spawnCount}/{DataManager.Instance.upgrades[0].GetAmount()}";
    }
}

[Serializable]
public struct UpgradePannel
{
    public Image icon;
    public TextMeshProUGUI nameTxt, explainTxt, btnTxt, moneyTxt;

    public void UpdateButtonUI(int level, int bonus, ulong cost)
    {
        btnTxt.text = bonus > 0 ? $"Level: {level}<color=blue>(+{bonus})</color>" : $"Level: {level}";
        moneyTxt.text = $"<sprite=0>{UIManager.Instance.GetCoinUnit(cost)}";
    }

    public void UpdateExplain(string explain)
    {
        explainTxt.text = explain;
    }
}

[Serializable]
public struct CollectionPannel
{
    public RectTransform pannel;
    public TextMeshProUGUI titleTxt;
    public Image bodyImg, faceImg;
    public TextMeshProUGUI nameTxt;
    public TextMeshProUGUI explainTxt;
    public TextMeshProUGUI moneyTxt;
    public RectTransform specialRect;

    public void SetPannel_NewCollection(int level, bool isSpecial)
    {
        DataManager dataManager = DataManager.Instance;
        BtnManager.Instance.Tab_NoRayCast(pannel);

        titleTxt.text = "New!!";

        SlimeData data = dataManager.Find_SlimeData_level(level, isSpecial);

        SetPannel(data);
    }

    public void SetPannel_UsedCollection(int ID)
    {
        BtnManager.Instance.Tab_NoRayCast(pannel);

        titleTxt.text = "Collect";

        SlimeData data = DataManager.Instance.Find_SlimeData(ID);

        SetPannel(data);
    }

    private void SetPannel(SlimeData data)
    {
        bodyImg.sprite = data.sprite.bodySprite;
        faceImg.sprite = data.sprite.FindFace(Face.Idle);

        nameTxt.text = data.name;
        nameTxt.color = data.color;
        explainTxt.text = data.explain;
        moneyTxt.text = $"{data.GetMiningAmount()}/{data.GetMiningCool()}sec";

        SetSpecial(data.isSpecial);
    }

    private void SetSpecial(bool isSpecial)
    {
        if (isSpecial)
        {
            specialRect.gameObject.SetActive(true);
            specialRect.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            Sequence seq = DOTween.Sequence().SetUpdate(true);
            seq.Append(specialRect.DOScale(1.5f, 1f))
                .Append(specialRect.DOScale(0.5f, 1f))
                .SetLoops(-1);
        }
        else
            specialRect.gameObject.SetActive(false);
    }
}

[Serializable]
public struct CollectionUI
{
    public TextMeshProUGUI collectCountTxt;
    public Slider collectSlider;
    public TextMeshProUGUI sliderPercentTxt;
    public TextMeshProUGUI luckPercentTxt;
    public Transform collectionGrid;

    public void SetUI()
    {
        SpawnManager.Instance.SpawnCollectPannels();

        UpdateLuckSlider();
    }

    public void UpdateUI(int ID)
    {
        CollectPannel[] collectPannels = collectionGrid.GetComponentsInChildren<CollectPannel>(true);
        Array.Find(collectPannels, item => item.ID == ID).UpdateCollectUI(DataManager.Instance.Find_SlimeData(ID));

        UpdateLuckSlider();
    }

    private void UpdateLuckSlider()
    {
        DataManager dataManager = DataManager.Instance;
        float luckAmount = dataManager.luck.GetAmount();
        float collectAmount = Mathf.Floor(dataManager.Find_Level(LevelType.Luck) / (float)(dataManager.SLIME_LENGTH + dataManager.SLIME_S_LENGTH) * 100f) / 100f;

        collectCountTxt.text = $"Number of Collected: {dataManager.Find_Level(LevelType.Luck)}";
        collectSlider.value = collectAmount;
        sliderPercentTxt.text = $"{collectAmount * 100}%";
        luckPercentTxt.text = $"Probability of Spawning Special Slime: <color=blue>{luckAmount * 100}%</color>";
    }
}

[Serializable]
public struct MapCollectionUI
{
    public Image bgCanvas;
    public RectTransform contentRect;
    public RectTransform bonusRect;

    public void SetUI()
    {
        DataManager dataManager = DataManager.Instance;
        SpawnManager spawnManager = SpawnManager.Instance;

        foreach (MapData data in dataManager.mapDatas)
            spawnManager.SpawnMapPannel(data.ID);

        foreach(Level lv in dataManager.levels)
        {
            if (lv.type == LevelType.Luck)
                continue;
            spawnManager.SpawnMapBonusUI(lv.type);
        }

        SetMapUI(dataManager.curMapID);
    }

    public void SetMapUI(int ID)
    {
        MapData data = DataManager.Instance.Find_MapData(ID);

        bgCanvas.sprite = data.mapSprite;
    }

    public void UpdateAllMapPannel()
    {
        MapPannel[] pannels = contentRect.GetComponentsInChildren<MapPannel>();

        foreach (MapPannel pannel in pannels)
            pannel.UpdatePannel();
    }

    public void UpdateAllBonusUI()
    {
        MapBonusUI[] uis = bonusRect.GetComponentsInChildren<MapBonusUI>();

        foreach (MapBonusUI ui in uis)
            ui.UpdateText();
    }
}