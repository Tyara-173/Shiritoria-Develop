using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class TitleAnime : MonoBehaviour
{
    [Header("落下させるオブジェクトのPrefab")]
    public GameObject fallingObjectPrefab;

    [Header("オブジェクトの生成設定")]
    [Tooltip("一度に表示されるオブジェクトの最大数")]
    public int maxObjectCount = 200;
    [Tooltip("オブジェクトが生成される間隔（秒）")]
    public float spawnInterval = 0.2f;

    [Header("オブジェクトの動きのランダム設定")]
    [Tooltip("落下速度の範囲（秒）")]
    public Vector2 fallDurationRange = new Vector2(5f, 10f);
    [Tooltip("回転速度の範囲")]
    public Vector2 rotationSpeedRange = new Vector2(30f, 90f);
    [Tooltip("オブジェクトの大きさの範囲")]
    public Vector2 scaleRange = new Vector2(0.5f, 2f);
    
    
    Vector2 spawnXRange = new Vector2(-10f, 10f);
    float spawnYPosition = 6f;
    float endYPosition = -6f;
    
    private const string HiraganaChars = "あいうえおかきくけこさしすせそたちつてとなにぬねのはひふへほまみむめもやゆよらりるれろわをん"
                                         + "がぎぐげござじずぜぞだぢづでどばびぶべぼぱぴぷぺぽ";
    void Start()
    {
        Vector2 c = Camera.main.ViewportToWorldPoint(Vector2.zero);
        spawnXRange = new Vector2(c.x, -c.x);
        spawnYPosition = -c.y+2f;
        endYPosition = c.y-2f;
        Debug.Log(c);
        DOVirtual.DelayedCall(spawnInterval, SpawnObject).SetLoops(-1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void SpawnObject()
    {
        float scale = Random.Range(scaleRange.x, scaleRange.y);
        // --- 1. オブジェクトの生成と初期設定 ---
        Vector3 spawnPos = new Vector3(
            Random.Range(spawnXRange.x, spawnXRange.y), // X座標をランダムに
            spawnYPosition, // Y座標は固定
            -scale
        );
        GameObject newObj = Instantiate(fallingObjectPrefab, spawnPos, Quaternion.identity, transform);
        newObj.GetComponent<MojiCube>().SetMoji(HiraganaChars[Random.Range(0, HiraganaChars.Length)]);

        // 大きさをランダムに設定
        newObj.transform.localScale = Vector3.one * scale;


        // --- 2. DOTweenでアニメーションを設定 ---
        
        // 落下アニメーション
        float fallDuration = Random.Range(fallDurationRange.x, fallDurationRange.y);
        newObj.transform.DOMoveY(endYPosition, fallDuration)
            .SetEase(Ease.Linear) // 一定速度で落下
            .OnComplete(() => {
                // アニメーションが完了したら（画面外に出たら）オブジェクトを破壊する
                Destroy(newObj);
            });

        // 回転アニメーション
        float rotationSpeed = Random.Range(rotationSpeedRange.x, rotationSpeedRange.y);
        newObj.transform.DORotate(new Vector3(0, 0, (Random.Range(0, 2) * 2 - 1) * 360), 360 / rotationSpeed, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Restart); // 無限に回転し続ける
    }

    public void LoadGameScene()
    {
        SceneManager.LoadScene("Game");
    }
}
