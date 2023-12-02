using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{
    // クラスのインスタンス作成。入れ物の変数のみを作っている。このままでは空
    // なので、UnityのGUIで情報を代入する。
    [SerializeField] Battler player;
    [SerializeField] Battler enemy;
    [SerializeField] CardGenerator cardGenerator;
    [SerializeField] GameObject submitButton;
    [SerializeField] GameUI gameUI;

    // RuleBookはGameMasterと同じコンポーネントにある。
    // （どちらのスクリプトもGameMasterという同じGameObjectについている。）
    // このときはこんな感じで取得することができる。同じものについていなければ、SerializeFieldを使う。
    RuleBook ruleBook;
    private void Awake() {
        ruleBook = GetComponent<RuleBook>();
    }

    private void Start()
    {
        Setup();
    }
    
    void Setup() 
    {
        gameUI.Init();
        player.Life = 4;
        enemy.Life = 4;
        gameUI.ShowLifes(player.Life, enemy.Life);
        gameUI.ShowAddNumber(player.AddNumber, enemy.AddNumber);
        player.OnSubmitAction = SubmittedAction;
        enemy.OnSubmitAction = SubmittedAction;
        SendCardsto(player);
        SendCardsto(enemy);
    }

    // 現段階では、周りくどいような気もするけどこれから足してくから細かく分けてる。
    void SubmittedAction()
    {
        if(player.IsSubmitted && enemy.IsSubmitted){
            submitButton.SetActive(false);
            // cardの勝利判定
            StartCoroutine(CardsBattle());
            
        }
        else if (player.IsSubmitted){
            submitButton.SetActive(false);
            // enemyがカードを出す
            enemy.RandomSubmit();
        }
    }

    void SendCardsto(Battler battler)
    {
        for (int i = 0; i < 8; i++)
        {
            Card card = cardGenerator.Spawn(i, battler==enemy);
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
    // cardの勝利判定
    // 一秒遅らしてから結果を表示: コルーチン
    IEnumerator CardsBattle()
    {
        yield return new WaitForSeconds(0.5f);
        enemy.SubmitCard.Open();
        yield return new WaitForSeconds(0.8f); // 0.8秒まつ
        RuleBook.Result result = ruleBook.GetResult(player, enemy);

        switch (result){
            // case文は複数の条件の指定を以下のような形で書ける。

            case RuleBook.Result.TurnWin:
            case RuleBook.Result.GameWin:
                gameUI.ShowTurnResult("WIN");
                enemy.Life--;
                break;
            
            case RuleBook.Result.TurnWin2:
                gameUI.ShowTurnResult("WIN2");
                enemy.Life -= 2;
                break;

            case RuleBook.Result.TurnDraw:
            case RuleBook.Result.GameDraw:
                gameUI.ShowTurnResult("DRAW");
                break;

            case RuleBook.Result.TurnLose:
            case RuleBook.Result.GameLose:
                gameUI.ShowTurnResult("LOSE");
                player.Life--;
                break;

            case RuleBook.Result.TurnLose2:
                gameUI.ShowTurnResult("LOSE2");
                player.Life -= 2;
                break;
        }
        gameUI.ShowLifes(player.Life, enemy.Life);
        yield return new WaitForSeconds(1f); // 1秒まつ
        
        if(player.Life <= 0 || enemy.Life <= 0){
            ShowResult(result);
        }
        else if(result == RuleBook.Result.GameWin || result == RuleBook.Result.GameLose || 
            result == RuleBook.Result.GameDraw){
            ShowResult(result);
        }
        else{

            SetupNextTurn();
        }

    }
    // 表示が終わったら、次のターンへ（場のカードを捨てる）
    void SetupNextTurn()
    {
        player.SetupNextTurn();
        enemy.SetupNextTurn();
        submitButton.SetActive(true);
        gameUI.SetupNextTurn();
        gameUI.ShowAddNumber(player.AddNumber, enemy.AddNumber);
        if (enemy.IsFirstSubmit) IsFirstSubmitEffect(enemy);
        if (player.IsFirstSubmit) IsFirstSubmitEffect(player);
    }

    void IsFirstSubmitEffect(Battler battler)
    {
        battler.RandomSubmit();
        battler.SubmitCard.Open();
        battler.IsFirstSubmit = false;
    }

    void ShowResult(RuleBook.Result result)
    {
        // 勝敗panelを表示する
        switch (result)
        {
            case RuleBook.Result.GameWin:
            case RuleBook.Result.TurnWin:
            case RuleBook.Result.TurnWin2:
                gameUI.ShowGameResult("WIN");
                break;
            case RuleBook.Result.GameLose:
            case RuleBook.Result.TurnLose:
            case RuleBook.Result.TurnLose2:
                gameUI.ShowGameResult("LOSE");
                break;
            
            // TODO カードが全部無くなってもGameDrawにならない。
            case RuleBook.Result.GameDraw:
                gameUI.ShowGameResult("DRAW");
                break;
        }
    }

    public void OnRetryButton()
    {
        // 同じシーンを呼び出す時の書き方
        string currentScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentScene);
    }

    public void OnTitleyButton()
    {
        SceneManager.LoadScene("Title");
    }
}
