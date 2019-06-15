using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuLogicScript : MonoBehaviour
{
    public ZUIManager zuiManager;
    public Popup gameOverPopup;
    public Text scoreText;

    void Start()
    {
        // после загрузки сцены проверяем - нужно ли открывать геймовер
        if (ScenesExchangeScript.GetBattleIsOver()) zuiManager.OpenPopup(gameOverPopup);
        ScenesExchangeScript.SetBattleIsOver(false);

        // пишем счёт в геймовер
        scoreText.text = ScenesExchangeScript.GetScore().ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
