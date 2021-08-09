//Author: Lance
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceOptionAsset : MonoBehaviour
{
    [SerializeField]
    private Button button;
    [SerializeField]
    private Image image;
    [SerializeField]
    private Choices choice;
    private GameAssets gameAssets;
    private GameState gameState;

    private void OnValidate()
    {
        button = GetComponent<Button>();
        image = transform.GetChild(0).GetComponent<Image>();
    }

    public void Init(Choices choice, GameAssets gameAssets, GameState gameState)
    {
        this.choice = choice;
        this.gameAssets = gameAssets;
        this.gameState = gameState;
        switch (choice)
        {
            case Choices.Paper:
                image.sprite = gameAssets.paperSprite;
                break;
            case Choices.Scissors:
                image.sprite = gameAssets.scissorsSprite;
                break;
            case Choices.Stone:
                image.sprite = gameAssets.stoneSprite;
                break;
        }
        button.onClick.AddListener(SelectChoice);
    }

    private void SelectChoice()
    {
        gameState.playerChoice = choice;
    }
}