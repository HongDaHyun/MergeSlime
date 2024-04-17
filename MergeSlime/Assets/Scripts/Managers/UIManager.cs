using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    public RectTransform bgCanvas;

    private void Start()
    {
        CameraBound camBound = SpawnManager.Instance.camBound;
        bgCanvas.sizeDelta = new Vector2(camBound.Width, camBound.Height);
    }
}
