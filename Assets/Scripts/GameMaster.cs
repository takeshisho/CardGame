using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    // クラスのインスタンス作成。入れ物の変数のみを作っている。このままでは空
    // なので、UnityのGUIで情報を代入する。
    [SerializeField] Battler player;
    [SerializeField] Battler enemy;
    [SerializeField] CardGenerator cardGenerator;
    [SerializeField] GameObject submitButton;

    private void Start() 
    {
        player.OnSubmitAction = SubmittedAction;
        SendCardsto(player);
        SendCardsto(enemy);
    }

    void SubmittedAction()
    {
        Debug.Log("SubmittedAction");
        if(player.IsSubmitted){
            submitButton.SetActive(false);
            // enemyがカードを出す
        }
        
    }

    void SendCardsto(Battler battler)
    {
        for (int i = 0; i < 8; i++)
        {
            Card card = cardGenerator.Spawn(i);
            // #7まではGameMasterが、battlerのHandにcardを渡していたが、これをbattlerに任せるようにする。
            // battler.Hand.Add(card);
            battler.SetCardToHand(card);
        }

        // 上記で作成したBattlerクラスのインスタンスplayerは、
        // Handというプロパティを持っている。このプロパティは、
        // Battlerhandクラスのインスタンスhandを入れるだけ。
        // よって、battlerhandクラスのインスタンスである、player.HandからResetPosition()を呼び出す。
        battler.Hand.ResetPosition();
    }
}
