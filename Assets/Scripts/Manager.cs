using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

public class Manager : MonoBehaviour{
    public enum GamePhase{
        Draw,
        QuickPlay,
        Monster1,
        Main,
        Monster2
    }

    public List<Card> Deck;

    public TextMeshProUGUI PhaseText;
    public GamePhase CurrentPhase = GamePhase.Draw;
    public int CurrentPlayerIndex;

    public GameObject CardPrefab;
    public LayoutUI LayoutUI;

    public readonly List<GameObject>[] PlayerBoards = new List<GameObject>[2];
    //Currently unused, should be used later on
    //public readonly List<GameObject>[] PlayerDeck = new List<GameObject>[2];
    public readonly List<GameObject>[] PlayerHands = new List<GameObject>[2];

    private bool _canPlay = true;

    private void Start(){
        InitialiseLists();

        DrawCards(0, 5);
        DrawCards(1, 5);
    }

    private void Update(){
        if (!Input.GetButtonDown("Fire1") || !_canPlay) return;

        var raycaster = LayoutUI.GetComponent<GraphicRaycaster>();
        var eventSystem = EventSystem.current;

        var data = new PointerEventData(eventSystem){
            position = Input.mousePosition
        };

        var results = new List<RaycastResult>();
        raycaster.Raycast(data, results);
        foreach (var result in results)
            if (result.gameObject.GetComponentInParent<CardUI>()){
                var card = result.gameObject.GetComponentInParent<CardUI>();
                if (!PlayerHands[CurrentPlayerIndex].Contains(card.gameObject)) return;
                if (!CanPlay(card.BaseCard.Type)) return;
                
                card.GetComponent<CardUI>().BaseCard.Played(this, card.gameObject);
                return;
            }
    }

    public void DrawCards(int player, int cardCount){
        for (var x = 0; x < cardCount; x++) DrawRandomCard(player);
    }

    public void ChangePhase(){
        switch (CurrentPhase){
            case GamePhase.Draw:
                CurrentPhase = GamePhase.QuickPlay;
                break;
            case GamePhase.QuickPlay:
                CurrentPhase = GamePhase.Monster1;
                break;
            case GamePhase.Monster1:
                CurrentPhase = GamePhase.Main;
                break;
            case GamePhase.Main:
                CurrentPhase = GamePhase.Monster2;
                break;
            case GamePhase.Monster2:
                CurrentPhase = GamePhase.Draw;
                CurrentPlayerIndex = CurrentPlayerIndex == 0 ? 1 : 0;
                break;
        }

        PhaseText.text = "Current phase: " + CurrentPhase.ToString();
    }

    public void ChangeCardField(GameObject card, GameObject field, Action function){
        StartCoroutine(CardAnimationCoroutine(card, field, function));
    }

    private void DrawRandomCard(int player){
        var rng = Random.Range(0, Deck.Count);
        Deck[rng].Draw(this, player);
    }
    
    private void InitialiseLists(){
        InitialiseList(PlayerBoards);
        InitialiseList(PlayerHands);
        //InitialiseList(PlayerDeck);
    }

    private void InitialiseList(List<GameObject>[] list){
        for (var x = 0; x < list.Length; x++) list[x] = new List<GameObject>();
    }

    private void Play(GameObject card){
        card.GetComponent<CardUI>().BaseCard.Played(this, card);
    }

    private bool CanPlay(Card.CardType type){
        return (type != Card.CardType.Draw && CurrentPhase != GamePhase.Main) || (type == Card.CardType.Draw && CurrentPhase != GamePhase.Draw);
    }

    private IEnumerator CardAnimationCoroutine(GameObject card, GameObject field, Action function){
        _canPlay = false;
        card.transform.SetParent(LayoutUI.transform);
        float timer = 0;
        
        Vector3 goal = field.transform.position;
        Transform cardTransform = card.transform;
        Vector3 start = cardTransform.position.Copy();

        while (timer < 1){
            cardTransform.position = Vector3.Lerp(start, goal, timer);
            timer += Time.deltaTime;
            yield return null;
        }
        
        cardTransform.SetParent(field.transform);
        cardTransform.position = goal;
        _canPlay = true;
        function.Invoke();
    }
}