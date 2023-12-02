using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlerHand : MonoBehaviour
{
    // リストの生成<>の中はリストの要素の型名
    public List<Card> list = new List<Card>();

    public bool Isempty { get => list.Count == 0; }

    public void Add(Card card)
    {
        list.Add(card);

        // これが子要素にしてるものらしい　わからない。
        card.transform.SetParent(transform);
    }

    public void Remove(Card card)
    {
        list.Remove(card);
    }

    public void ResetPosition()
    {
        // listを小さい順に並べる。詳しくはnotion
        list.Sort((card0, card1) => card0.Base.Number - card1.Base.Number);
        for (int i = 0; i < list.Count; i++)
        {
            float posX = (i - list.Count / 2f) * 1.5f;
            // クラスからインスタンスを生成するときは、new 型名（引数）
            list[i].transform.localPosition = new Vector3(posX, 0);
        }
    }

    public Card RandomRemove()
    {
        int r = Random.Range(0, list.Count);
        Card card = list[r];
        Remove(card);
        return card;

    }
}
