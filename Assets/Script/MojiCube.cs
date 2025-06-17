using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MojiCube : MonoBehaviour
{
    public TMP_Text moji;

    public void SetMoji(char c)
    {
        moji.SetText(c.ToString());
    }
}
