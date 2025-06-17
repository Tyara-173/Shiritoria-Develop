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
    public int cnt = 0;
    public char lastChar = 'り';
    private List<GameObject> _mojiList = new List<GameObject>();
    private int x, y = -6;
    void Start()
    {
        inputField.onValidateInput += ValidateHiragana;
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
        inputField.ActivateInputField();
        string txt = inputField.text;
        DoShiritori(txt); // 非同期で実行（待たない）
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
        foreach (var c in text)
        {
            GameObject moji = Instantiate(mojiObj, new Vector3(x, y, 0), Quaternion.identity, transform);
            moji.GetComponent<MojiCube>().SetMoji(c);
            _mojiList.Add(moji);
            x++;
        }
    }
}
