using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Redcode.Pools;

public class SpawnManager : Singleton<SpawnManager>
{
    public CameraBound camBound;
    public Transform border_L, border_R, border_T, border_B;

    protected override void Awake()
    {
        base.Awake();

        camBound = new CameraBound();
        camBound.SetCameraBound();
        SetBorder();
    }

    private void Start()
    {
        StartCoroutine(CloudSpawnRoutine());
        StartCoroutine(StarSpawnRoutine());
    }

    #region ¸Ê
    public void SetBorder()
    {
        // ¿Ü°û¼± Å©±â ¼³Á¤
        border_L.localScale = new Vector3(5, camBound.Height + 5, 1);
        border_R.localScale = new Vector3(5, camBound.Height + 5, 1);
        border_T.localScale = new Vector3(camBound.Width + 5, 5, 1);
        border_B.localScale = new Vector3(camBound.Width + 5, 5, 1);

        // ¿Ü°û¼± À§Ä¡ ¼³Á¤
        border_L.position = new Vector3(camBound.Left - border_L.localScale.x / 2f, 0, 0);
        border_R.position = new Vector3(camBound.Right + border_R.localScale.x / 2f, 0, 0);
        border_T.position = new Vector3(0, camBound.Top + border_T.localScale.y / 2f, 0);
        border_B.position = new Vector3(0, camBound.Bottom - border_B.localScale.y / 2f, 0);
    }
    #endregion

    #region Cloud
    private void SpawnCloud()
    {
        string cloudName = $"Cloud_{Random.Range(0, 3)}";
        Transform cloudTrans = PoolManager.Instance.GetFromPool<Transform>(cloudName);
        cloudTrans.name = cloudName;

        float x = camBound.Left - cloudTrans.localScale.x;
        float y = Random.Range(camBound.Bottom + cloudTrans.localScale.y, camBound.Top - cloudTrans.localScale.y);

        cloudTrans.position = new Vector2(x, y);

        cloudTrans.DOLocalMoveX(camBound.Right + cloudTrans.localScale.x, 5f).SetUpdate(true).SetEase(Ease.Linear).OnComplete(() => DeSpawnCloud(cloudName, cloudTrans));
    }

    private void DeSpawnCloud(string name,Transform trans)
    {
        PoolManager.Instance.TakeToPool<Transform>(name, trans);
    }

    private IEnumerator CloudSpawnRoutine()
    {
        SpawnCloud();

        yield return new WaitForSeconds(Random.Range(0.5f, 1.2f));

        StartCoroutine(CloudSpawnRoutine());
    }
    #endregion

    #region Star
    private void SpawnStar()
    {
        Transform starTrans = PoolManager.Instance.GetFromPool<Transform>("Star");

        float x = camBound.RanWidthBoundary();
        float y = camBound.RanHeightBoudnary();
        float scale = Random.Range(0.1f, 0.5f);

        starTrans.position = new Vector2(x, y);
        starTrans.rotation = Quaternion.Euler(0, 0, Random.Range(-180f, 180f));
        starTrans.localScale = Vector3.zero;

        Sequence starSeq = DOTween.Sequence().SetUpdate(true);
        starSeq.Append(starTrans.DOScale(scale, 1f).SetEase(Ease.Linear))
            .AppendInterval(1f)
            .Append(starTrans.DOScale(0f, 1f).SetEase(Ease.Linear))
            .AppendCallback(() => DeSpawnStar(starTrans));
    }

    private void DeSpawnStar(Transform trans)
    {
        PoolManager.Instance.TakeToPool<Transform>("Star", trans);
    }

    private IEnumerator StarSpawnRoutine()
    {
        SpawnStar();

        yield return new WaitForSeconds(Random.Range(0.5f, 1.2f));

        StartCoroutine(StarSpawnRoutine());
    }
    #endregion

    #region Slime
    public Slime SpawnSlime(int level, Vector2 vec2)
    {
        Slime slime = PoolManager.Instance.GetFromPool<Slime>("Slime");

        slime.transform.position = vec2;
        slime.SetSlime(level);
        SpawnPop(slime.transform);

        return slime;
    }

    public void DeSpawnSlime(Slime slime)
    {
        PoolManager.Instance.TakeToPool<Slime>(slime);
    }
    #endregion

    #region VFX
    public VFX_Pop SpawnPop(Transform parent)
    {
        VFX_Pop pop = PoolManager.Instance.GetFromPool<VFX_Pop>("VFX_Pop");

        pop.transform.position = parent.transform.position;
        Vector3 scale = parent.transform.localScale;
        pop.transform.localScale = new Vector3(scale.x + 0.3f, scale.y + 0.3f, scale.z + 0.3f);

        return pop;
    }

    public void DeSpawnPop(VFX_Pop pop)
    {
        PoolManager.Instance.TakeToPool<VFX_Pop>("VFX_Pop", pop);
    }
    #endregion
}

public class CameraBound
{
    private Camera camera;
    private float size_x, size_y;

    public void SetCameraBound()
    {
        camera = Camera.main;

        size_y = camera.orthographicSize;
        size_x = camera.orthographicSize * Screen.width / Screen.height;
    }

    public float RanWidthBoundary()
    {
        return Random.Range(Left + 1, Right - 1);
    }

    public float RanHeightBoudnary()
    {
        return Random.Range(Bottom + 1, Top - 1);
    }

    public float Bottom
    {
        get
        {
            return size_y * -1 + camera.gameObject.transform.position.y;
        }
    }

    public float Top
    {
        get
        {
            return size_y + camera.gameObject.transform.position.y;
        }
    }

    public float Left
    {
        get
        {
            return size_x * -1 + camera.gameObject.transform.position.x;
        }
    }

    public float Right
    {
        get
        {
            return size_x + camera.gameObject.transform.position.x;
        }
    }

    public float Height
    {
        get
        {
            return size_y * 2;
        }
    }

    public float Width
    {
        get
        {
            return size_x * 2;
        }
    }
}