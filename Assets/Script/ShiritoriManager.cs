using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System.Threading.Tasks;

public class ShiritoriManager : MonoBehaviour
{
    public TMP_Text backText;
    public TMP_InputField inputField;
    public TMP_Text inputNextText;
    public GameObject loadObj;
    public GameObject mojiObj;
    public char lastChar = 'り';
    private List<GameObject> _mojiList = new List<GameObject>();
    private float x, y, size;
    private int cnt, mojisu = 0;
    void Start()
    {
        inputField.onValidateInput += ValidateHiragana;
        try
        {
            Vector2 c = Camera.main.ViewportToWorldPoint(Vector2.zero);
            size = -(c.x * 2) / 10;
            x = c.x+size/2;
            y = c.y+size/2;
            mojiObj.transform.localScale = new Vector3(size,size,1);
        }
        catch (Exception e) {}
        
    }

    void Update()
    {
        
    }
    private char ValidateHiragana(string text, int charIndex, char addedChar)
    {
        if ((addedChar >= '\u3040' && addedChar <= '\u309F') || addedChar == 'ー')
        {
            return addedChar;
        }
        else
        {
            return '\0'; // 無効な文字は入力されない
        }
    }

    public void OnTextEvent()
    {
        string txt = inputField.text;
        if (txt.Length == 0)
        {
            return;
        }
        DoShiritori(txt); // 非同期で実行（待たない）
        inputField.ActivateInputField();
    }

    private async void DoShiritori(string txt)
    {
        loadObj.SetActive(true);

        bool isValid = await IsShiritori(txt);

        if (isValid)
        {
            Debug.Log("✅ OK");

            for (int i = txt.Length - 1; i >= 0; i--)
            {
                if (txt[i] != 'ー')
                {
                    lastChar = txt[i];
                    break;
                }
            }

            backText.text = txt;
            cnt++;
            inputField.text = "";
            inputNextText.SetText(lastChar.ToString());
            SpownCube(txt);
            
            Debug.Log(txt);
            Debug.Log(lastChar);
        }
        else
        {
            Debug.Log("❌ NG");
        }

        loadObj.SetActive(false);
    }

    private async Task<bool> IsShiritori(string txt)
    {
        if (txt[0] == lastChar)
        {
            bool isRealWord = await CheckWord(txt);
            return isRealWord;
        }
        else
        {
            return false;
        }
    }

    public async Task<bool> CheckWord(string word)
    {
        string url = "https://jisho.org/api/v1/search/words?keyword=" + UnityWebRequest.EscapeURL(word);

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            var operation = request.SendWebRequest();

            while (!operation.isDone)
            {
                await Task.Yield(); // ノンブロッキングで待機
            }

#if UNITY_2020_1_OR_NEWER
            if (request.result != UnityWebRequest.Result.Success)
#else
            if (request.isNetworkError || request.isHttpError)
#endif
            {
                Debug.LogError("通信エラー: " + request.error);
                return false;
            }

            string json = request.downloadHandler.text;
            bool isRealWord = json.Contains('"' + word + '"') || json.Contains('"' + H2K(word) + '"');

            if (isRealWord)
            {
                Debug.Log("✅ 「" + word + "」は実在する単語です。");
            }
            else
            {
                Debug.Log("❌ 「" + word + "」は見つかりませんでした。");
            }

            return isRealWord;
        }
    }

    string H2K(string input)
    {
        char[] result = new char[input.Length];

        for (int i = 0; i < input.Length; i++)
        {
            char c = input[i];
            if (c >= '\u3041' && c <= '\u3096') // ひらがな
            {
                result[i] = (char)(c + 0x60); // カタカナへ
            }
            else
            {
                result[i] = c;
            }
        }

        return new string(result);
    }

    void SpownCube(string text)
    {
        for (var i = cnt == 1 ? 0 : 1; i < text.Length; i++)
        {
            GameObject moji = Instantiate(mojiObj, new Vector3(x, y, 0), Quaternion.identity, transform);
            moji.GetComponent<MojiCube>().SetMoji(text[i]);
            _mojiList.Add(moji);
            x+=size;
            mojisu++;
            if (mojisu % 10 == 0)
            {
                x -= size * 10;
                y += size;
            }
        }
    }
}
