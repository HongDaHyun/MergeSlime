using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Expression : MonoBehaviour
{
    private Slime slime;
    private Coroutine curFaceRoutine;

    private void Awake()
    {
        slime = GetComponent<Slime>();
    }

    public void SetFace(Face _face)
    {
        slime.face.sprite = slime.dataManager.Find_SlimeData_level(slime.level, slime.isSpecial).sprite.FindFace(_face);
    }

    public void SetFace(Face _face, float time)
    {
        if (curFaceRoutine != null)
            StopCoroutine(curFaceRoutine);

        curFaceRoutine = StartCoroutine(FaceRoutine(_face, time));
    }

    private IEnumerator FaceRoutine(Face _face, float time)
    {
        SetFace(_face);

        yield return new WaitForSeconds(time);

        SetFace(Face.Idle);
        curFaceRoutine = null;
    }
}
