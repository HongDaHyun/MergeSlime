using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Redcode.Pools;

public class SpawnManager : Singleton<SpawnManager>
{
    public CameraBound camBound;

    protected override void Awake()
    {
        base.Awake();

        camBound = new CameraBound();
        camBound.SetCameraBound();
    }

    private void Start()
    {
        StartCoroutine(CloudSpawnRoutine());
        StartCoroutine(StarSpawnRoutine());
    }

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

        float x = Random.Range(camBound.Left + 1, camBound.Right - 1);
        float y = Random.Range(camBound.Bottom + 1, camBound.Top - 1);
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
}

public class CameraBound
{
    public RectTransform BGCanvasRect;

    private Camera camera;
    private float size_x, size_y;

    public void SetCameraBound()
    {
        camera = Camera.main;

        size_y = camera.orthographicSize;
        size_x = camera.orthographicSize * Screen.width / Screen.height;
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