using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Redcode.Pools;
using TMPro;
using DG.Tweening;

public class NoticePannel : MonoBehaviour, IPoolObject
{
    private RectTransform rect;
    private float limitT, limitB;

    SpawnManager spawnManager;
    UIManager uiManager;

    public Image iconImg;
    public TextMeshProUGUI titleTxt, explainTxt, rewardTxt;

    public void OnCreatedInPool()
    {
        spawnManager = SpawnManager.Instance;
        uiManager = UIManager.Instance;

        rect = GetComponent<RectTransform>();

        SetLimit();
    }

    public void OnGettingFromPool()
    {
        RanSpawnPoint();
    }

    public void SetUI(string title, string explain, ulong reward, Sprite icon)
    {
        iconImg.sprite = icon;

        titleTxt.text = $"<{title}>";
        explainTxt.text = explain;

        rewardTxt.gameObject.SetActive(reward > 0);
        rewardTxt.text = $"<sprite=0>{uiManager.GetCoinUnit(reward)}";
    }

    private void SetLimit()
    {
        RectTransform parent = rect.parent.GetComponent<RectTransform>();

        limitT = (parent.rect.height / 2) - (rect.rect.height / 2);
        limitB = -(parent.rect.height / 2) + (rect.rect.height / 2);
    }

    private void RanSpawnPoint()
    {
        int ranDir = Random.Range(0, 2); // 0 -> ¿Þ, 1 -> ¿À

        Vector2 ranPoint = new Vector2(ranDir, 0.5f);
        rect.pivot = ranPoint;
        rect.anchorMax = ranPoint;
        rect.anchorMin = ranPoint;

        float ranY = Random.Range(limitB, limitT);
        
        if(ranDir == 0)
        {
            rect.localPosition = new Vector2(-rect.rect.width, ranY);
            RightMove();
        }
        else
        {
            rect.localPosition = new Vector2(rect.rect.width, ranY);
            LeftMove();
        }
    }

    private void RightMove()
    {
        Sequence rightSeq = DOTween.Sequence().SetUpdate(true);
        rightSeq.Append(rect.DOAnchorPosX(10f, 0.5f).SetEase(Ease.OutBounce))
            .AppendInterval(5f)
            .Append(rect.DOAnchorPosX(-rect.rect.width, 0.5f)).
            OnComplete(() => spawnManager.DeSpawnNoticePannel(this));
    }

    private void LeftMove()
    {
        Sequence leftSequence = DOTween.Sequence().SetUpdate(true);
        leftSequence.Append(rect.DOAnchorPosX(-10f, 0.5f).SetEase(Ease.OutBounce))
            .AppendInterval(5f)
            .Append(rect.DOAnchorPosX(rect.rect.width, 0.5f)).
            OnComplete(() => spawnManager.DeSpawnNoticePannel(this));
    }
}
