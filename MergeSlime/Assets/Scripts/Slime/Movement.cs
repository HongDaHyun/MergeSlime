using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float moveSpeed;
    private Vector2 moveDir;
    [HideInInspector] public bool isDrop;

    private Slime slime;

    private void Awake()
    {
        slime = GetComponent<Slime>();
    }

    public void ReSet()
    {
        moveDir = Random.insideUnitCircle.normalized;
        isDrop = false;
    }

    public void Move()
    {
        // 회전
        slime.body.transform.Rotate(Vector3.forward * 50f * Time.fixedDeltaTime);

        slime.rigid.MovePosition(slime.rigid.position + moveDir * moveSpeed * Time.fixedDeltaTime);
    }

    public void MoveReflect(Collision2D collision)
    {
        Vector2 reflectNormal = collision.collider.ClosestPoint(slime.rigid.position) - slime.rigid.position;
        moveDir = Vector2.Reflect(moveDir, reflectNormal.normalized);
    }

    public IEnumerator DropRoutine()
    {
        isDrop = true;
        yield return new WaitForSeconds(0.1f);
        isDrop = false;
    }

    public void Merge(Slime otherSlime)
    {
        int maxLv = slime.isSpecial ? DataManager.Instance.SLIME_S_LENGTH : DataManager.Instance.SLIME_LENGTH;
        if (otherSlime.level != slime.level || slime.level >= maxLv || otherSlime.level >= maxLv || slime.isSpecial != otherSlime.isSpecial)
            return;

        slime.SetState(State.Merge);
        otherSlime.SetState(State.Merge);

        slime.spawnManager.DeSpawnSlime(otherSlime);
        slime.spawnManager.SpawnPop(slime.body.transform);
        slime.spawnManager.slimeList[slime.spawnManager.slimeList.FindIndex(lv => lv == slime.level)] = ++slime.level;
        ES3Manager.Instance.Save(SaveType.SLIMES);
        slime.SetSlime(slime.level);
        slime.expression.SetFace(Face.Surprise, 1.5f);

        slime.dataManager.coin.GainCoin(slime.mining.miningAmount);
        slime.spawnManager.SpawnMoneyTxt(slime.body.transform, slime.mining.miningAmount);
    }

    public void Drag()
    {
        if(slime.state == State.Pick)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.x = Mathf.Clamp(mousePos.x, slime.spawnManager.camBound.Left, slime.spawnManager.camBound.Right);
            mousePos.y = Mathf.Clamp(mousePos.y, slime.spawnManager.camBound.Bottom, slime.spawnManager.camBound.Top);
            mousePos.z = 0;

            transform.position = mousePos;
        }
    }
}
