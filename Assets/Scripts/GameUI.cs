using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI turnResultText;
    [SerializeField] TextMeshProUGUI playerLifeText;
    [SerializeField] TextMeshProUGUI enemyLifeText;
    [SerializeField] GameObject resultPanel;
    [SerializeField] TextMeshProUGUI resultText;
    [SerializeField] GameObject playerAddNumberObj;
    [SerializeField] GameObject enemyAddNumberObj;
    // これは自作関数であり、Startみたいに元々用意されているものではない。
    public void Init()
    {
        turnResultText.gameObject.SetActive(false);
        resultPanel.gameObject.SetActive(false);
    }

    public void ShowAddNumber(int playerAddNumber, int enemyAddNumber)
    {
        if (playerAddNumber == 2) playerAddNumberObj.SetActive(true);
        else playerAddNumberObj.SetActive(false);

        if (enemyAddNumber == 2) enemyAddNumberObj.SetActive(true);
        else enemyAddNumberObj.SetActive(false);
    }   

    public void ShowLifes(int player_life, int enemy_life)
    {
        playerLifeText.text = "×" + player_life.ToString();
        enemyLifeText.text = "×" + enemy_life.ToString();
    }

    // 勝敗表示
    public void ShowTurnResult(string result)
    {
        turnResultText.gameObject.SetActive(true);
        turnResultText.text = result;
    }

    public void ShowGameResult(string result)
    {
        resultPanel.gameObject.SetActive(true);
        resultText.text = result;
    }

    public void SetupNextTurn()
    {
        turnResultText.gameObject.SetActive(false);
    }
}
