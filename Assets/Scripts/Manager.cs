using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cards;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

//Enums and structs used even outside of the manager class
public enum FieldType{
    Field,
    Hand,
    Draw,
    Discard,
    Terrain
}
public readonly struct Field{
    public Field(int player, FieldType type){
        Player = player;
        Type = type;
    }
        
    public readonly int Player;
    public readonly FieldType Type;
}

public class Manager : MonoBehaviour{
    public static Manager Instance;
    
    //should be self-explanatory
    public enum GamePhase{
        Draw,
        QuickPlay,
        Monster1,
        Main,
        Monster2
    }

    //List of all possible cards
    public List<Card> Deck;

    //Phase related elements
    public TextMeshProUGUI PhaseText;
    public GamePhase CurrentPhase = GamePhase.Draw;
    public int CurrentPlayerIndex;
    
    //Turn Change Event
    public static Action OnTurnChange;

    //Dependencies
    public GameObject CardPrefab;
    public LayoutUI LayoutUI;
    
    //Player collections
    public readonly List<GameObject>[] PlayerBoards = new List<GameObject>[2];
    public readonly List<GameObject>[] PlayerHands = new List<GameObject>[2];
    public readonly GameObject[] PlayerTerrains = new GameObject[2];
    public int[] PlayerLife = new int [] {20, 20};

    //Serves to limit play while an animation is playing
    private bool _canPlay = true;

    private CardUI _selectedCard;

    private void Start(){
        if (Instance != null) throw new Exception("Can't have more than 1 manager.");
        Instance = this;
        
        InitialiseLists();
        DrawCards(0, 5);
        DrawCards(1, 5);
    }

    private void Update(){
        //Vérifies si on peut jouer et si on a cliqué
        if (!Input.GetButtonDown("Fire1") || !_canPlay) return;

        //Récupères la carte sur laquelle on a cliqué et vérifies qu'on a bel et bien reçu une carte
        var card = GetCard();
        if(card == null){
            _selectedCard = null;
            return;
        }

        //Si on a cliqué sur une carte de notre main: joues-là
        GameObject cardGO = card.gameObject;
        if (PlayerHands[CurrentPlayerIndex].Contains(cardGO)){
            PlayCard(card);
            return;
        }
        
        
        if (CurrentPhase != GamePhase.Monster1 && CurrentPhase != GamePhase.Monster2) return;
        
        if (PlayerBoards[CurrentPlayerIndex].Contains(cardGO)){
            if (card.BaseCard.Type == Card.CardType.Creature){
                _selectedCard = card;
                return;
            }
        }
        
        if (_selectedCard == null) return;
        if (PlayerBoards[CurrentPlayerIndex == 0 ? 1 : 0].Contains(cardGO)){
            Attack(_selectedCard, card);
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
                if (CurrentPlayerIndex == 0 ) ChangeTurn();
                break;
        }

        PhaseText.text = "Current phase: " + CurrentPhase.ToString();
    }

    public void ChangeCardField(GameObject card, Field field, Action function){
        StartCoroutine(CardAnimationCoroutine(card, field, function));
    }
    
    private void PlayCard(CardUI card){
        if (!CanPlay(card.BaseCard)) return;
        
        card.BaseCard.Played(card.gameObject);
    }

    private void Attack(CardUI attacker, CardUI defender){
        if (attacker == null || defender == null) return;
        if (CurrentPhase != GamePhase.Monster1 && CurrentPhase != GamePhase.Monster2) return;
        attacker.BaseCard.Attack(attacker.gameObject, defender.gameObject);
    }

    private void DrawRandomCard(int player){
        var rng = Random.Range(0, Deck.Count);
        Deck[rng].Draw(player);
    }
    
    private void InitialiseLists(){
        InitialiseList(PlayerBoards);
        InitialiseList(PlayerHands);
        
    }

    private static void InitialiseList(List<GameObject>[] list){
        for (var x = 0; x < list.Length; x++) list[x] = new List<GameObject>();
    }

    private bool CanPlay(Card card){
        var type = card.Type;
        return (type != Card.CardType.Draw && CurrentPhase == GamePhase.Main) || (type == Card.CardType.Draw && CurrentPhase == GamePhase.Draw) || (card.IsQuickPlay && CurrentPhase == GamePhase.QuickPlay);
    }

    private IEnumerator CardAnimationCoroutine(GameObject card, Field field, Action function){
        _canPlay = false;
        card.transform.SetParent(LayoutUI.transform);
        float timer = 0;

        Transform fieldTransform = LayoutUI.GetField(field).transform;
        Vector3 goal = fieldTransform.position;
        Vector3 start = card.transform.position.Copy();

        while (timer < 1){
            card.transform.position = Vector3.Lerp(start, goal, timer);
            timer += Time.deltaTime;
            yield return null;
        }
        
        card.transform.SetParent(fieldTransform);
        card.transform.position = goal;
        _canPlay = true;
        function.Invoke();
    }

    private CardUI GetCard(){
        var raycaster = LayoutUI.GetComponent<GraphicRaycaster>();
        var eventSystem = EventSystem.current;

        var data = new PointerEventData(eventSystem){
            position = Input.mousePosition
        };

        var results = new List<RaycastResult>();
        raycaster.Raycast(data, results);
        return (from result in results where result.gameObject.GetComponentInParent<CardUI>() select result.gameObject.GetComponentInParent<CardUI>()).FirstOrDefault();
    }

    private void ChangeTurn(){
        foreach (var board in PlayerBoards){
            foreach (var card in board){
                card.GetComponent<CardData>().CanAttack = true;
            }
        }
        
        OnTurnChange?.Invoke();

        int firstPlayer = PlayerBoards[0].Count;
        int secondPlayer = PlayerBoards[1].Count;

        if (firstPlayer > secondPlayer){
            if (PlayerLife[1] <= firstPlayer - secondPlayer) GameOver(0);
            PlayerLife[1] -= firstPlayer - secondPlayer;
            LayoutUI.PlayerHealths[1].GetComponent<TextMeshProUGUI>().text = PlayerLife[1].ToString();
        }
        else if (secondPlayer > firstPlayer){
            if (PlayerLife[0] <= secondPlayer - firstPlayer) GameOver(1);
            PlayerLife[0] -= secondPlayer - firstPlayer;
            LayoutUI.PlayerHealths[0].GetComponent<TextMeshProUGUI>().text = PlayerLife[0].ToString();
        }
    }

    private void GameOver(int winner){
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Debug.Log("Player " + (winner + 1) + " Wins");
    }
}