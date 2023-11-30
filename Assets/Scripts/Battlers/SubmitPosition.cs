using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmitPosition : MonoBehaviour
{
    // 選択されたカードを管理する
    Card submitCard;

    public Card SubmitCard { get => submitCard; }
    // 自分の子要素にする　位置を自分の場所にする。
    public void Set(Card card)
    {
        submitCard = card;
        // これで、cardの親をこのクラス（submitpositionにしてる）
        // このscriptがついているobjectを親にしたいときは、this.transformになる。thisは省略可能
        card.transform.SetParent(transform);
        // cardの位置をsubmitpositionの位置と同じにしている。
        card.transform.position = transform.position;
    }

    public void DeleteCard()
    {
        // 消したいのはcomponentではなくて、gameobjectなのでsubmitCardだけではダメ
        Destroy(submitCard.gameObject);
        // コンポーネントも空にしておく
        submitCard = null;
    }
}
