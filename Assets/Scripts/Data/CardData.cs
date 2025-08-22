using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCard", menuName = "Card/Create New Card")]
public class CardData : ScriptableObject
{
    public string cardName;
    public Sprite cardArt;
    public int cost;

    public CardCategory category;  // ex: Attack | Special

    public TargetType targetType;

    public List<CardEffectSO> effects = new();

}