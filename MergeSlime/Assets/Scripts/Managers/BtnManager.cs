using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BtnManager : Singleton<BtnManager>
{
    private bool isTouching;

    public void Tab(RectTransform rect)
    {
        if (isTouching)
            return;

        UIManager um = UIManager.Instance;

        if (!rect.gameObject.activeSelf)
        {
            rect.gameObject.SetActive(true);
            um.raycastPannel.SetActive(true);
            rect.localScale = new Vector3(0.05f, 0.05f, 0.05f);
            rect.DOScale(new Vector3(1f, 1f, 1f), 0.5f).SetEase(Ease.InExpo).SetEase(Ease.OutBounce).SetUpdate(true);
        }
        else
        {
            um.raycastPannel.SetActive(false);
            rect.DOScale(new Vector3(0.05f, 0.05f, 0.05f), 0.25f).SetEase(Ease.InOutExpo).SetUpdate(true).OnComplete(() => rect.gameObject.SetActive(false));
        }
    }

    public void Tab_NoRayCast(RectTransform rect)
    {
        if (isTouching)
            return;

        if (!rect.gameObject.activeSelf)
        {
            rect.gameObject.SetActive(true);
            rect.localScale = new Vector3(0.05f, 0.05f, 0.05f);
            rect.DOScale(new Vector3(1f, 1f, 1f), 0.5f).SetEase(Ease.InExpo).SetEase(Ease.OutBounce).SetUpdate(true);
        }
        else
        {
            rect.DOScale(new Vector3(0.05f, 0.05f, 0.05f), 0.25f).SetEase(Ease.InOutExpo).SetUpdate(true).OnComplete(() => rect.gameObject.SetActive(false));
        }
    }

    public void SpawnSlimeBtn()
    {
        DataManager dataManager = DataManager.Instance;

        if (SpawnManager.Instance.spawnCount >= dataManager.upgrades[0].amount)
            return;
        if (!dataManager.coin.LoseCoin(dataManager.spawnPrice.GetPrice()))
            return;

        dataManager.spawnPrice.UpLevel();

        SpawnManager.Instance.SpawnSlime(1);
    }

    public void UpgradeBtn(int id)
    {
        DataManager dataManager = DataManager.Instance;

        if (!dataManager.coin.LoseCoin((ulong)dataManager.upgrades[id].cost))
            return;

        DataManager.Instance.upgrades[id].UpLevel();
        UIManager.Instance.SetUpgradeUI(id);
    }
}
