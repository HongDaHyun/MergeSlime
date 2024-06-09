using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Redcode.Pools;
using UnityEngine.UI;
using TMPro;

public class CollectPannel : MonoBehaviour, IPoolObject
{
    public int ID;
    public bool isCollect;
    public Image bodyImg, faceImg;
    public TextMeshProUGUI nameTxt;

    private UIManager uiManager;

    public void OnCreatedInPool()
    {
        uiManager = UIManager.Instance;
    }

    public void OnGettingFromPool()
    {
    }

    public void SetUI(SlimeData data)
    {
        ID = data.ID;

        bodyImg.sprite = data.sprite.bodySprite;
        faceImg.sprite = data.sprite.FindFace(Face.Idle);

        UpdateCollectUI(data);
    }

    public void UpdateCollectUI(SlimeData data)
    {
        if (data.isCollect)
        {
            isCollect = true;

            bodyImg.color = Color.white;
            faceImg.color = Color.white;

            nameTxt.color = data.color;
            nameTxt.text = data.name;
        }
        else
        {
            isCollect = false;

            bodyImg.color = Color.black;
            faceImg.color = Color.black;

            nameTxt.color = Color.black;
            nameTxt.text = "???";
        }
    }

    public void Click()
    {
        if (!isCollect || uiManager.collectionPannel.pannel.gameObject.activeSelf)
            return;

        uiManager.collectionPannel.SetPannel_UsedCollection(ID);
    }
}
