//Author: Lance
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum Choices { None, Scissors, Paper, Stone }
public enum PlayerType { Player, Bot }
public class GameManager : MonoBehaviour
{
    private enum BotMode { Random, SureWin, SureLose, CopyCat }
    [SerializeField]
    private Canvas canvas;
    [SerializeField]
    private BotMode botMode = BotMode.Random;
    public GameAssets gameAssets = new GameAssets();
    public GameState gameState = new GameState();
    [SerializeField]
    private int timeLimitPerRound = 30;
    private Vector3 resultTextOriginalLocalPosition;

    private void Awake()
    {
        resultTextOriginalLocalPosition = gameAssets.resultText.transform.localPosition;
        gameAssets.botChoiceAsset.Init(gameAssets);
        gameAssets.playerChoiceAsset.Init(gameAssets);
        InitChoiceOptions();
    }

    private void Start()
    {
        StartCoroutine(StartRound());
    }

    private void FixedUpdate()
    {
        //This will toggle between cheat modes, unfortunately did not have time to finish this feature.
        if (Input.GetKeyDown(KeyCode.Space))
            gameState.gameMode = ((int)gameState.gameMode < Enum.GetNames(typeof(GameMode)).Length) ? (GameMode)((int)gameState.gameMode+1) : 0;
    }

    private void InitChoiceOptions()
    {
        int totalChoices = Enum.GetNames(typeof(Choices)).Length - 1;
        ChoiceOptionAsset freshChoiceOptionAsset;
        for (int i = 1; i < totalChoices+1; i++)
        {
            freshChoiceOptionAsset = Instantiate(gameAssets.choiceOptionAssetPrefab, gameAssets.choiceOptionPanel);
            gameAssets.choiceOptionAssets.Add(freshChoiceOptionAsset);
            freshChoiceOptionAsset.Init((Choices)i, gameAssets, gameState);
        }
    }

    private void MakeAChoice(Choices choice, PlayerType playerType)
    {
        switch (playerType) 
        {
            case PlayerType.Player:
                gameState.playerChoice = choice;
                break;
            case PlayerType.Bot:
                gameState.botChoice = choice;
                break;
        }
    }

    private IEnumerator ConcludeRound()
    {
        gameAssets.playerChoiceAsset.SetSprite(gameState.playerChoice);
        gameAssets.botChoiceAsset.SetSprite(gameState.botChoice);
        gameAssets.playerChoiceAsset.image.DOFade(1, 0.2F);
        gameAssets.botChoiceAsset.image.DOFade(1, 0.2F);
        GameLogic(gameState.playerChoice, gameState.botChoice);
        switch (gameState.matchResult)
        {
            case MatchResult.PlayerWins:
                gameAssets.resultText.text = "Player Wins";
                break;
            case MatchResult.BotWins:
                gameAssets.resultText.text = "Bot Wins";
                break;
            case MatchResult.Draw:
                gameAssets.resultText.text = "It's a Draw!";
                break;
        }
        gameAssets.versusText.DOFade(0, 0.5F);
        gameAssets.resultText.DOFade(1, 1);
        gameAssets.resultText.transform.DOLocalMoveY(5, 2);

        yield return new WaitForSecondsRealtime(3);
        gameAssets.resultText.DOFade(0, 0.5f);
        yield return new WaitForSecondsRealtime(1);
        gameAssets.playerChoiceAsset.image.DOFade(0, 0.2F);
        gameAssets.botChoiceAsset.image.DOFade(0, 0.2F);
        gameAssets.resultText.transform.localPosition = resultTextOriginalLocalPosition;
        gameAssets.versusText.DOFade(1, 0.5F);
        StartCoroutine(StartRound());
    }

    private void GameLogic(Choices playerChoice, Choices botChoice)
    {
        if (playerChoice == Choices.Scissors && botChoice == Choices.Paper)
            gameState.matchResult = MatchResult.PlayerWins;
        else if (playerChoice == Choices.Paper && botChoice == Choices.Stone)
            gameState.matchResult = MatchResult.PlayerWins;
        else if (playerChoice == Choices.Stone && botChoice == Choices.Scissors)
            gameState.matchResult = MatchResult.PlayerWins;
        else if (playerChoice == gameState.botChoice)
            gameState.matchResult = MatchResult.Draw;
        else
            gameState.matchResult = MatchResult.BotWins;

        switch (gameState.matchResult)
        {
            case MatchResult.PlayerWins:
                gameState.playerPoints++;
                gameAssets.playerPointsText.text = gameState.playerPoints.ToString();
                break;
            case MatchResult.BotWins:
                gameState.botPoints++;
                gameAssets.botPointsText.text = gameState.botPoints.ToString();
                break;
        }
    }

    Coroutine startCountDownCoroutine;
    private IEnumerator StartCountdown()
    {
        WaitForSecondsRealtime delay = new WaitForSecondsRealtime(1);
        gameState.timeLeft = timeLimitPerRound; 
        while(gameState.timeLeft > 0)
        {
            yield return delay;
            gameState.timeLeft -= 1;
            gameAssets.timerText.text = gameState.timeLeft.ToString();
            gameAssets.timerText.transform.DOShakePosition(0.5f, 10, 50);
            gameAssets.timerText.transform.DOShakeScale(0.3f, 1, 30);
        }
    }

    private IEnumerator StartRound()
    {
        gameState.playerChoice = Choices.None;
        gameState.botChoice = Choices.None;
        gameAssets.timerText.text = "";
        startCountDownCoroutine = StartCoroutine(StartCountdown());
        while (gameState.playerChoice == Choices.None && gameState.timeLeft > 0)
        {
            yield return null;
        }
        if (gameState.timeLeft <= 0) // If player does not make a move after times up, choose a move for the player.
            gameState.playerChoice = (Choices)UnityEngine.Random.Range(1, 4);

        StopCoroutine(startCountDownCoroutine);
        MakeAChoice(gameState.playerChoice, PlayerType.Player);
        botMode = (BotMode)UnityEngine.Random.Range(0, 4);
        switch (botMode)
        {
            case BotMode.Random:
                MakeAChoice(Bot.MakeRandomChoice(), PlayerType.Bot);
                break;
            case BotMode.SureLose:
                MakeAChoice(Bot.MakeSureLoseChoice(gameState.playerChoice), PlayerType.Bot);
                break;
            case BotMode.SureWin:
                MakeAChoice(Bot.MakeSureWinChoice(gameState.playerChoice), PlayerType.Bot);
                break;
            case BotMode.CopyCat:
                gameState.botChoice = gameState.playerChoice;
                break;
        }
        StartCoroutine(ConcludeRound());
    }
}