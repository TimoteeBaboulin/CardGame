using System;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour{
    public enum GamePhase{
        Draw,
        QuickPlay,
        Monster1,
        Main,
        Monster2
    }

    public List<Card> Deck;

    public GamePhase CurrentPhase = GamePhase.Draw;
    public int CurrentPlayerIndex = 0;

    public List<Card>[] PlayerBoards = new List<Card>[2];
    public List<Card>[] PlayerHands = new List<Card>[2];
    public List<Card>[] PlayerDeck = new List<Card>[2];
    
    public GameObject CardPrefab;
    public GameObject Canvas;

    private void Start(){
        Instantiate(CardPrefab, Canvas.transform);
        CardPrefab.GetComponent<CardUI>().SetCard(Deck[0]);
    }
}