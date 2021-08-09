//Author: Lance
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MatchResult { None, PlayerWins, BotWins, Draw }
public enum GameMode { Normal, SureWin, SureLose }
[System.Serializable]
public class GameState
{
    public Choices playerChoice;
    public Choices botChoice;
    public int playerPoints;
    public int botPoints;
    public MatchResult matchResult;
    public int timeLeft;
    public GameMode gameMode;
}