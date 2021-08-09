//Author: Lance
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class GameAssets
{
    public ChoiceAsset playerChoiceAsset, botChoiceAsset;
    public Sprite scissorsSprite, paperSprite, stoneSprite;
    public ChoiceOptionAsset choiceOptionAssetPrefab;
    public Transform choiceOptionPanel;
    public List<ChoiceOptionAsset> choiceOptionAssets = new List<ChoiceOptionAsset>();
    public TextMeshProUGUI timerText, playerPointsText, botPointsText, resultText, versusText;
}