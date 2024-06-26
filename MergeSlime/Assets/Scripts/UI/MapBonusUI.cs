using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Redcode.Pools;
using TMPro;

public class MapBonusUI : MonoBehaviour, IPoolObject
{
    public LevelType type;
    public TextMeshProUGUI contents;

    DataManager dataManager;

    public void OnCreatedInPool()
    {
        dataManager = DataManager.Instance;
    }

    public void OnGettingFromPool()
    {
    }

    public void SetUI(LevelType _type)
    {
        type = _type;

        UpdateText();
    }

    public void UpdateText()
    {
        int bonusLv = dataManager.Find_BonusLevel(type);

        contents.text = bonusLv >= 0 ? $"{type}: <color=red>+{dataManager.Find_BonusLevel(type)}</color>" :
            $"{type}: <color=blue>{dataManager.Find_BonusLevel(type)}</color>";
    }
}
