using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MojiCube : MonoBehaviour
{
    public Text moji;
    public Sprite[] backImages;
    private Renderer objRenderer;
    
    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = backImages[Random.Range(0, backImages.Length)];
        objRenderer = GetComponent<Renderer>();
    }

    void Update()
    {
        // if (!objRenderer.isVisible)
        // {
        //     Destroy(gameObject);
        // }
    }
    
    public void SetMoji(char c)
    {
        moji.text = c.ToString();
    }
}
