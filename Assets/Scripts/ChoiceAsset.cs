//Author: Lance
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceAsset : MonoBehaviour
{
    public Image image;
    private GameAssets gameAssets;

    private void OnValidate()
    {
        image = GetComponent<Image>();
    }

    public void Init(GameAssets gameAssets)
    {
        this.gameAssets = gameAssets;
    }

    public void SetSprite(Choices choice)
    {
        switch (choice)
        {
            case Choices.Scissors:
                image.sprite = gameAssets.scissorsSprite;
                break;
            case Choices.Paper:
                image.sprite = gameAssets.paperSprite;
                break;
            case Choices.Stone:
                image.sprite = gameAssets.stoneSprite;
                break;
        }
    }
}
