using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RuleBook : MonoBehaviour
{
    //*
    // skeltonがいれば数字対決
    // 密偵（次のかーど先に出す）がいるなら追加効果
    // drackyがいるなら引き分け
    // 暗殺者がいて、王子がいないなら数字効果反転
    // 王子と姫がいるならGameの勝利判定
    // ここまで来れば数字対決（大臣なら２倍）
    //*/

    // 動画では、Game Masterのshow resultでやってる部分をここに持ってきた。

    public Result GetResult(Battler player, Battler enemy)
    {
        CardType player_card_type = player.SubmitCard.Base.Type;
        CardType enemy_card_type = enemy.SubmitCard.Base.Type;

        // カードがなくなったらDRAWにするようにした。残りのlife数で勝敗決めるのもあり。
        if (player.Hand.list.Count <= 0 || enemy.Hand.list.Count <= 0){
            return Result.GameDraw;
        }
            

        if (player_card_type == CardType.Killing_Machine ||
            enemy_card_type == CardType.Killing_Machine)
            return NumberBattle(player, enemy);

        if (player_card_type == CardType.Slime) enemy.IsFirstSubmit = true;
        if (enemy_card_type == CardType.Slime) player.IsFirstSubmit = true;

        // Prestidigitor（次のターン+2の効果の実装）
        if (player_card_type == CardType.Prestidigitator)
            player.IsAddingNumberEffect = true;
        if (enemy_card_type == CardType.Prestidigitator)
            enemy.IsAddingNumberEffect = true;

        if (player_card_type == CardType.Dracky ||
            enemy_card_type == CardType.Dracky)
            return Result.TurnDraw;

        if ((player_card_type == CardType.Skeleton &&
            enemy_card_type != CardType.Dragonlord) ||
            (player_card_type != CardType.Dragonlord &&
            enemy_card_type == CardType.Skeleton))
            return SkeltonEffect(NumberBattle(player, enemy));

        if (player_card_type == CardType.Healslime &&
            enemy_card_type == CardType.Dragonlord)
            return Result.GameWin;

        if (player_card_type == CardType.Dragonlord &&
            enemy_card_type == CardType.Healslime)
            return Result.GameLose;


            return NumberBattle(player, enemy, Green_DragonEffect: true);
    }

    Result NumberBattle(Battler player, Battler enemy, bool Green_DragonEffect=false)
    {
        int player_num = player.SubmitCard.Base.Number + player.AddNumber;
        int enemy_num = enemy.SubmitCard.Base.Number + enemy.AddNumber;
        if(!Green_DragonEffect){
            if (player_num > enemy_num) return Result.TurnWin;
            else if (player_num < enemy_num) return Result.TurnLose;
            else return Result.TurnDraw;
        }
        else {
            if (player_num > enemy_num) {
                if (player.SubmitCard.Base.Type == CardType.Green_Dragon) return Result.TurnWin2;
                else return Result.TurnWin;
            }
        
            else if (player_num < enemy_num) {
                if (player.SubmitCard.Base.Type == CardType.Green_Dragon) return Result.TurnLose2;
                else return Result.TurnLose;
            }

            else return Result.TurnDraw;
        }

    }

    Result SkeltonEffect(Result result)
    {
        if (result == Result.TurnWin) return Result.TurnLose;
        else if (result == Result.TurnWin2) return Result.TurnLose2;
        else if (result == Result.TurnLose) return Result.TurnWin;
        else if (result == Result.TurnLose2) return Result.TurnWin2;
        else return Result.TurnDraw;
    }
    
    public enum Result
    {
        TurnWin,
        TurnLose,
        TurnDraw,
        TurnWin2,
        TurnLose2,
        GameWin,
        GameLose,
        GameDraw,
    }
}
