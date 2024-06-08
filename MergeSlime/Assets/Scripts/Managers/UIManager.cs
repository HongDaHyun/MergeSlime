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

    [Title("패널", "PannelCanvas에서 사용하는 UI")]
    public GameObject raycastPannel;

    [Title("인게임")]
    public RectTransform bgCanvas;
    public UpgradePannel[] upgradePannels;
    public CollectionPannel collecionPannel;

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
        Upgrade upgrade = DataManager.Instance.upgrades[id];

        switch(id)
        {
            case 0:
                upgradePannels[0].UpdateExplain($"Proc: <color=blue>{upgrade.amount}%</color>");
                break;
            case 1:
                upgradePannels[id].UpdateExplain($"x{upgrade.amount}%");
                break;
            case 2:
                upgradePannels[id].UpdateExplain($"+{upgrade.amount}");
                break;
        }

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
        BtnManager.Instance.Tab(pannel);

        titleTxt.text = "New!!";

        SlimeData data = dataManager.Find_SlimeData_level(level, isSpecial);

        bodyImg.sprite = data.sprite.bodySprite;
        faceImg.sprite = data.sprite.FindFace(Face.Idle);

        nameTxt.text = data.name;
        nameTxt.color = data.color;
        explainTxt.text = data.explain;
        moneyTxt.text = $"{data.GetMiningAmount(isSpecial)}/{data.GetMiningCool(isSpecial)}sec";

        SetSpecial(isSpecial);
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