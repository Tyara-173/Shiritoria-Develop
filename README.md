# Shiritoria
[![Made with Unity](https://img.shields.io/badge/Made%20with-Unity_2022.3.22f1-57b9d3.svg?style=flat&logo=unity)](https://unity3d.com)

<p align="center"><img src="Icons/Shiritoria_Icon.png" alt="代替テキスト" width="200" height="200"></p>


# 概要
　「しりとりあ」は，Unityで作成されたしりとりゲームです．

## タイトル画面
![alt text](Icons/Image1.png)

## しりとり
前の単語の最後の文字からつながるように単語を入力していきます．[jisho.org](https://jisho.org/)APIを使用し，単語の実在判定を行い，実在する単語のみ受け付ける仕様となっています．

![alt text](Icons/Image2.png)

## ひらがなビスケット
しりとりが成立すると背景にビスケットが溜まっていきます．ビスケットにはひらがなが書かれており，行ったしりとりがつながるように溜まっていきます．

![alt text](Icons/Image3.png)

## 終了/エラー
しりとりが成立していない，または存在しない単語の入力は弾かれます．

![alt text](Icons/image4.png)
![alt text](Icons/image5.png)

「ん」で終了する，または過去に使用した単語を入力したらゲームオーバーです．
![alt text](Icons/image6.png)

## メニュー
ゲーム中はEscキーでメニューが開けます．
![alt text](Icons/image7.png)

# 実行手順

https://github.com/Tyara-173/Shiritoria_Desktopをダウンロードし，Shiritoria.exeを実行してください．（Windows以外での動作確認はできておりません．）

# 参考にしたサイト
https://w.atwiki.jp/ultimate/pages/16.html<br>
https://note.com/5mingame2/n/n341823e433a3

# AIの使用について

Unityは過去にも使用していたため，基礎的な部分で詰まることは少なかったですが，単語の実在判定やAPIの利用などの部分はかなり手伝ってもらいました．<br>
実在判定についての質問：https://chatgpt.com/share/6858728a-f3a8-8000-bf8e-02b764ab3169<br>
httpリクエストについての質問：https://chatgpt.com/share/685872c0-05f0-8000-bad9-8c35bc7e8b5a<br>
タイトル画面の背景作成：https://gemini.google.com/share/6dd0f3f4ad41<br>

基本的にコードすべてのコピペは行わず，部分的な質問・コードの生成をお願いしていました．

# 使用素材
じゆうちょうフォント by よく飛ばない鳥様 <br>
https://yokutobanaitori.web.fc2.com/ <br>
<br>
いろいろなビスケットのイラスト 　by  いらすとや様 <br>
https://www.irasutoya.com/2018/01/blog-post_258.html
