//Author: Lance
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bot
{
    public static Choices MakeRandomChoice()
    {
        return (Choices)Random.Range(1, 4);
    }

    public static Choices MakeSureWinChoice(Choices playerChoice)
    {
        switch (playerChoice)
        {
            case Choices.Paper:
                return Choices.Scissors;
            case Choices.Scissors:
                return Choices.Stone;
            case Choices.Stone:
                return Choices.Paper;
            default:
                return Choices.Paper;
        }
    }

    public static Choices MakeSureLoseChoice(Choices playerChoice)
    {
        switch (playerChoice)
        {
            case Choices.Paper:
                return Choices.Stone;
            case Choices.Scissors:
                return Choices.Paper;
            case Choices.Stone:
                return Choices.Scissors;
            default:
                return Choices.Paper;
        }
    }
}