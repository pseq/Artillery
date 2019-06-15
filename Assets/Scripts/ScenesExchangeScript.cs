using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ScenesExchangeScript
{
    private static bool battleIsOver = false;
    private static int gamerScore;

    public static bool GetBattleIsOver() => battleIsOver;
    public static void SetBattleIsOver(bool over) => battleIsOver = over;
    public static int GetScore() => gamerScore;
    public static void SetScore(int score) => gamerScore = score;
}
