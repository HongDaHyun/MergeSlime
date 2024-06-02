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
        for (int i = 0; i < upgradePannels.Length; i++)
            SetUpgradeUI(i);
    }

    public void SetUpgradeUI(int id)
    {
        Upgrade upgrade = DataManager.Instance.upgradeLv[id];

        if (id == 0)
            upgradePannels[0].UpdateExplain($"Proc: <color=blue>{upgrade.amount}%</color>");
        else
            upgradePannels[id].UpdateExplain($"+{upgrade.amount}");

        if (upgrade.level >= upgrade.levelLimit && upgrade.levelLimit != -1)
        {
            upgradePannels[id].btnTxt.text = "MAX";
            upgradePannels[id].moneyTxt.text = $"<sprite=0> -";
            return;
        }

        upgradePannels[id].UpdateUI(upgrade.level, upgrade.cost);
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

    public void UpdateUI(int level, int cost)
    {
        btnTxt.text = $"Level: {level}";
        moneyTxt.text = $"<sprite=0>{cost}";
    }

    public void UpdateExplain(string explain)
    {
        explainTxt.text = explain;
    }
}