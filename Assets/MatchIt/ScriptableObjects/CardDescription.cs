using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardDescription", menuName = "ScriptableObjects/CardDescription")]
public class CardDescription : ScriptableObject
{
    public List<Cards> cards;
}


[Serializable]
public class Cards
{
    public int CardID;
    public Sprite CardImage;
}