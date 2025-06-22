using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine.UI;
using Random = System.Random;

public class ShiritoriManager : MonoBehaviour
{
    public Text backText;
    public InputField inputField;
    public Text inputNextText;
    public GameObject loadObj;
    public GameObject mojiObj;
    public char lastChar = 'り';
    private List<GameObject> _mojiList = new List<GameObject>();
    private float x, y, size;
    private int l = 15;
    private int cnt, mojisu = 0;
    private HashSet<string> usedWords = new HashSet<string>();
    private static readonly Dictionary<char, char> SmallToLargeMap = new Dictionary<char, char>
    {
        { 'ぁ', 'あ' }, { 'ぃ', 'い' }, { 'ぅ', 'う' }, { 'ぇ', 'え' }, { 'ぉ', 'お' },
        { 'っ', 'つ' }, { 'ゃ', 'や' }, { 'ゅ', 'ゆ' }, { 'ょ', 'よ' }, { 'ゎ', 'わ' }
    };
    void Start()
    {
        inputField.onValidateInput += ValidateHiragana;
        try
        {
            Vector2 c = Camera.main.ViewportToWorldPoint(Vector2.zero);
            size = -(c.x * 2) / l;
            x = c.x+size/2;
            y = c.y+size/2;
            mojiObj.transform.localScale = new Vector3(size,size,1);
        }
        catch (Exception e) {}  
        
    }

    void Update()
    {
        
    }

    void GameOver()
    {
        // 各オブジェクトに対して弾けるアニメーションを実行
        foreach (GameObject obj in _mojiList)
        {
            Transform trans = obj.transform;
            // Rigidbodyがなければ追加し、IsKinematicをtrueに設定
            Rigidbody2D rb = trans.GetComponent<Rigidbody2D>();
            if (rb == null)
            {
                rb = trans.gameObject.AddComponent<Rigidbody2D>();
            }
            rb.isKinematic = false;
        }
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

        char templast = ' ';
        
        for (int i = txt.Length - 1; i >= 0; i--)
        {
            if (txt[i] == 'ー')
            {
                continue;
            }
            templast = SmallToLargeMap.GetValueOrDefault(txt[i], txt[i]);
            break;
        }

        if (templast != 'ん' && isValid)
        {
            Debug.Log("✅ OK");

            lastChar = templast;
            backText.text = txt;
            cnt++;
            inputField.text = "";
            inputNextText.text = lastChar.ToString();
            usedWords.Add(txt);
            SpownCube(txt);
            
            Debug.Log(txt);
            Debug.Log(lastChar);
        }
        else
        {
            if (txt[0] == lastChar)
            {
                GameOver();
            }
            Debug.Log("❌ NG");
        }

        loadObj.SetActive(false);
    }

    private async Task<bool> IsShiritori(string txt)
    {
        if (txt[0] == lastChar && !usedWords.Contains(txt))
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
            if (mojisu > 0 && mojisu % l == 0)
            {
                x -= size * l;
                y -= size;
                Camera.main.transform.DOMoveY(-size,1f).SetRelative(true).SetEase(Ease.Linear);
            }
            GameObject moji = Instantiate(mojiObj, new Vector3(x, y, 0), Quaternion.identity, transform);
            moji.GetComponent<MojiCube>().SetMoji(text[i]);
            _mojiList.Add(moji);
            x+=size;
            mojisu++;
        }
    }
}
