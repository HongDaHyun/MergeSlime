using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using TMPro;
using System;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [Title("스크린"," ScreenCanvas에서 사용하는 UI")]
    public MoneyUI moneyUI;

    [Title("패널", "PannelCanvas에서 사용하는 UI")]
    public GameObject raycastPannel;

    [Title("인게임")]
    public RectTransform bgCanvas;
    public UpgradePannel[] upgradePannels;

    private void Start()
    {
        CameraBound camBound = SpawnManager.Instance.camBound;
        bgCanvas.sizeDelta = new Vector2(camBound.Width, camBound.Height);

        SetUI();
    }

    private void SetUI()
    {
        moneyUI.SetUI();
    }

    public void SetUpgradeUI(int id)
    {
        DataManager dataManager = DataManager.Instance;

        string explain = "";

        if (id == 0)
        {
            if (dataManager.upgradeLv[0].level > dataManager.SPECIAL_INCREASE_LIMIT)
            {
                upgradePannels[0].btnTxt.text = "MAX";
                upgradePannels[0].moneyTxt.text = $"<sprite=0> -";
                return;
            }

            explain = $"Proc: <color=blue>{Mathf.RoundToInt(dataManager.LUCK_PERCENT * 100)}%</color>";
            upgradePannels[0].UpdateUI(explain, dataManager.upgradeLv[0].level, dataManager.upgradeLv[0].cost);
        }

        else
            upgradePannels[id].UpdateUI(explain, dataManager.upgradeLv[id].level, dataManager.upgradeLv[id].cost);
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

        SetMoney(dataManager.coin.amount);
        SetPrice(dataManager.spawnPrice);
    }

    public void SetMoney(int amount)
    {
        coinTxt.text = $"<sprite=0>{amount}";
    }

    public void SetPrice(int price)
    {
        priceTxt.text = $"<sprite=0>{price}";
    }
}

[Serializable]
public struct UpgradePannel
{
    public Image icon;
    public TextMeshProUGUI nameTxt, explainTxt, btnTxt, moneyTxt;

    public void UpdateUI(string explain, int level, int cost)
    {
        if(explain != "")
            explainTxt.text = explain;
        btnTxt.text = $"Level: {level}";

        moneyTxt.text = $"<sprite=0>{cost}";
    }
}