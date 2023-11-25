using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardGenerator : MonoBehaviour
{
    // notionに説明あり。public CardクラスのcardPrefabsという変数生成 初期値代入はinspectorでやる。
    [SerializeField] Card cardPrefabs;
    [SerializeField] CardBase[] cardBases;


    public Card Spawn(int num) {

        // generate card
        Card card = Instantiate(cardPrefabs);

        // CardクラスのSet関数により、CardBasesのnum番目の情報をセットする。
        card.Set(cardBases[num]);
        return card;
    }
}
