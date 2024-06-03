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

    public void SpawnSlimeBtn()
    {
        DataManager dataManager = DataManager.Instance;
        if (!dataManager.coin.LoseCoin(dataManager.spawnPrice))
            return;
        dataManager.SetPrice();

        SpawnManager.Instance.SpawnSlime(1, true);
    }

    public void UpgradeBtn(int id)
    {
        DataManager.Instance.upgrades[id].UpLevel();
        UIManager.Instance.SetUpgradeUI(id);
    }
}
