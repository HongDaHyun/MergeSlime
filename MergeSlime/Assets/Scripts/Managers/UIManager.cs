using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using TMPro;
using System;

public class UIManager : Singleton<UIManager>
{
    [Title("공용", "모든 씬에서 사용하는 UI")]
    public MoneyUI moneyUI;

    [Title("인게임")]
    public RectTransform bgCanvas;

    private void Start()
    {
        CameraBound camBound = SpawnManager.Instance.camBound;
        bgCanvas.sizeDelta = new Vector2(camBound.Width, camBound.Height);
    }
}

[Serializable]
public class MoneyUI
{
    public TextMeshProUGUI text;
    public void SetMoney(int amount)
    {
        text.text = $"<sprite=0>{amount}";
    }
}
