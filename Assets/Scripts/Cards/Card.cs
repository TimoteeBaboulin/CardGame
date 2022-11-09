using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Card : ScriptableObject{
    public static Action<Manager, GameObject> OnCardPlayed;
    public static Action<Manager, GameObject> OnCardDestroyed;

    public enum CardType{
        Creature,
        Spell,
        Terrain,
        Draw
    }

    public bool IsQuickPlay;
    public string Name;
    public string Description;
    public Sprite Sprite;

    public List<CardEffect> OnDraw = new();
    public List<CardEffect> OnPlay = new();
    public List<CardEffect> OnDestroy = new();

    public abstract CardType Type{ get; }

    /// <summary>
    /// Method called when the card is played from the player's hand
    /// </summary>
    /// <param name="manager"></param>
    /// <param name="card"></param>
    public virtual void Played(Manager manager, GameObject card){
        manager.PlayerHands[card.GetComponent<CardData>().Get("Owner")].Remove(card);
        OnCardPlayed?.Invoke(manager, card);

        foreach (var effect in OnPlay){
            effect.Do(manager, card);
        }
    }

    /// <summary>
    /// Method called when the card is drawn from the deck
    /// </summary>
    /// <param name="manager"></param>
    /// <param name="owner"></param>
    public virtual void Draw(Manager manager, int owner){
        var card = InstantiateCard(manager, owner);
        manager.ChangeCardField(card, manager.LayoutUI.PlayerHands[card.GetComponent<CardData>().Get("Owner")],
            () => {
                foreach (var effect in OnDraw){
                    effect.Do(manager, card);
                }
            });
    }

    /// <summary>
    /// Method called when the card is killed or destroyed (whether in battle or through card effect)
    /// </summary>
    /// <param name="manager"></param>
    /// <param name="card"></param>
    public virtual void Destroy(Manager manager, GameObject card){
        manager.PlayerBoards[card.GetComponent<CardData>().Get("Owner")].Remove(card);
        int owner = card.GetComponent<CardData>().Get("Owner");
        manager.ChangeCardField(card, manager.LayoutUI.PlayerDiscards[owner], () => {
            foreach (var effect in OnDestroy){
                effect.Do(manager, card);
            }

            Destroy(card);
        });
        OnCardDestroyed?.Invoke(manager, card);
    }

    protected GameObject InstantiateCard(Manager manager, int owner){
        var newCard = Instantiate(manager.CardPrefab, manager.LayoutUI.PlayerDraws[owner].transform);
        manager.PlayerHands[owner].Add(newCard);

        var data = newCard.GetComponent<CardData>();
        data.Add("Owner", owner);
        
        var UI = newCard.GetComponent<CardUI>();
        UI.SetCard(this);

        return newCard;
    }
}