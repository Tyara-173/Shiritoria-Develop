using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MojiCube : MonoBehaviour
{
    public TMP_Text moji;
    public Sprite[] backImages;

    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = backImages[Random.Range(0, backImages.Length)];
    }
    
    public void SetMoji(char c)
    {
        moji.SetText(c.ToString());
    }
}
