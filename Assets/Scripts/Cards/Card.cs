using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events; // UnityActionを使うためにimport
using TMPro;

public class Card : MonoBehaviour
{
    // card UI and process of game
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI numberText;
    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI descriptionText;
    [SerializeField] GameObject hidePanel;

    // プロパティ: 外から取得はできるけど代入はprivate(このファイルの中のみ) 
    public CardBase Base { get; private set; }

    // battlerはbattlerHandを管理している時などはbattlerHandの関数を使う時などは、インスタンス
    // とか作るだけでいいが、cardは、battlerを持っていない。そんな時に使うのがaction
    // UnityActionは、関数を登録することができる
    public UnityAction<Card> OnClickCard;

    // CardBaseの持っている情報を取得 .Nameとかはプロパティ
    public void Set(CardBase cardBase, bool isEnemy){
        Base = cardBase;
        nameText.text = cardBase.Name;
        numberText.text = cardBase.Number.ToString();
        icon.sprite = cardBase.Icon;
        descriptionText.text = cardBase.Description;
        hidePanel.SetActive(isEnemy);
    }
    // 自分で作った関数。Inspctor でButtonというコンポーネントにクリックしたら実行されるように設定してある
    public void OnClick()
    {
        // これにより登録した関数を実行することができる。
        // Battler.csにて、SelectedCardを登録している。よってSelectedCardをここでは実行している
        // ここでのthisはクラスのcardではなくて、objectのcardを指す。
        OnClickCard?.Invoke(this);
    }

    public void Open()
    {
        hidePanel.SetActive(false);
    }
}
