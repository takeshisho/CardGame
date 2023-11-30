using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Battler : MonoBehaviour
{
    [SerializeField] BattlerHand hand;
    [SerializeField] SubmitPosition submitPosition;
    public UnityAction OnSubmitAction;

    // プロパティ boolは何も代入しないと初期値false
    public bool IsSubmitted { get; private set; }
    public bool IsFirstSubmit{ get;  set; }
    public bool IsAddingNumberEffect{ get; set; }
    public int AddNumber{ get; private set; }
    public BattlerHand Hand { get => hand; }
    // submitPositionのプロパティであるSubmitCard
    public Card SubmitCard { get => submitPosition.SubmitCard; }
    public int Life{ get; set; }

    public void SetCardToHand(Card card)
    {
        hand.Add(card);
        // これでそれぞれのcardのOnClickCardに関数SelectedCardを登録することをしている。
        // 関数を登録しているだけで実行しているわけではない。
        card.OnClickCard = SelectedCard;
    }

    void SelectedCard(Card card)
    {
        if (IsSubmitted) return;

        // すでにセットしていれば手札に戻す。
        if(submitPosition.SubmitCard){
            hand.Add(submitPosition.SubmitCard);
        }
        hand.Remove(card);
        submitPosition.Set(card);
        hand.ResetPosition();
    }

    public void OnSubmitButton()
    {
        if(submitPosition.SubmitCard){
            // カードの決定 => 変更不可（決定ボタンを押せない/カードの交換はできない)
            IsSubmitted = true;
            OnSubmitAction?.Invoke();
        }

    }

    public void RandomSubmit()
    {
        // 手札からランダムでカードを抜き取る
        Card card = hand.RandomRemove();
        // 提出用にセット
        submitPosition.Set(card);
        hand.ResetPosition();
        // 提出をGameMasterに通知
        IsSubmitted = true;
        OnSubmitAction?.Invoke();
    }

    public void SetupNextTurn()
    {
        IsSubmitted = false;
        submitPosition.DeleteCard();
        if (IsAddingNumberEffect) AddNumber = 2;
        else AddNumber = 0;
        IsAddingNumberEffect = false;
    }
}
