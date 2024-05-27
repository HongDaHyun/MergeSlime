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

        SpawnManager sm = SpawnManager.Instance;
        CameraBound camBound = sm.camBound;

        Vector2 ranVec = new Vector2(Random.Range(camBound.Left + 1, camBound.Right - 1), Random.Range(camBound.Bottom + 1, camBound.Top - 1));

        sm.SpawnSlime(1, ranVec);
    }
}
