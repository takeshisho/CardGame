using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardGenerator : MonoBehaviour
{
    // notionに説明あり。public CardクラスのcardPrefabsという変数生成
    [SerializeField] Card cardPrefabs;

    private void Start(){
        for (int i = 0; i < 8; i++) {
            Spawn();
        }
        
    }

    // generate card
    public void Spawn() {
        Instantiate(cardPrefabs);
    }
}
