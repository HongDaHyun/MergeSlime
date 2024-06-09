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
    public RectTransform bgCanvas;
    public UpgradePannel[] upgradePannels;
    public CollectionPannel collectionPannel;
    public CollectionUI collectionUI;

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

        collectionUI.SetUI();
    }

    public void SetUpgradeUI(int id)
    {
        Upgrade upgrade = DataManager.Instance.upgrades[id];

        switch(id)
        {
            case 0:
                upgradePannels[0].UpdateExplain($"{upgrade.amount}<color=grey>(+{upgrade.NextAmountIncrease()})</color>");
                spawnLimitUI.UpdateTxt();
                break;
            case 1:
                upgradePannels[id].UpdateExplain($"x{upgrade.amount}<color=grey>(+{upgrade.NextAmountIncrease()})</color>%");
                break;
            case 2:
                upgradePannels[id].UpdateExplain($"{upgrade.amount}<color=grey>(+{upgrade.NextAmountIncrease()})</color>");
                break;
        }

        upgradePannels[id].UpdateButtonUI(upgrade.level, upgrade.cost);
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
public struct SpawnLimitUI
{
    public TextMeshProUGUI text;

    public void UpdateTxt()
    {
        text.text = $"{SpawnManager.Instance.spawnCount}/{DataManager.Instance.upgrades[0].amount}";
    }
}

[Serializable]
public struct UpgradePannel
{
    public Image icon;
    public TextMeshProUGUI nameTxt, explainTxt, btnTxt, moneyTxt;

    public void UpdateButtonUI(int level, int cost)
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
        Luck luck = dataManager.luck;
        float luckAmount = dataManager.luck.GetAmount();
        float collectAmount = Mathf.Floor(luck.level / (float)(dataManager.SLIME_LENGTH + dataManager.SLIME_S_LENGTH) * 100f) / 100f;

        collectCountTxt.text = $"Number of Collected: {luck.level}";
        collectSlider.value = collectAmount;
        sliderPercentTxt.text = $"{collectAmount * 100}%";
        luckPercentTxt.text = $"Probability of Spawning Special Slime: <color=blue>{luckAmount * 100}%</color>";
    }
}