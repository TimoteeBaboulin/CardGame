using System.Collections.Generic;
using UnityEngine;

public abstract class Card : ScriptableObject{
    public enum CardType{
        Creature,
        Spell,
        Terrain,
        Draw
    }

    public bool IsQuickPlay;
    public string Name;
    public string Description;

    public List<CardEffect> OnDraw = new();
    public List<CardEffect> OnPlay = new();
    public List<CardEffect> OnDestroy = new();

    public abstract CardType Type{ get; }

    public virtual void Played(Manager manager, GameObject card){
        manager.PlayerHands[card.GetComponent<CardData>().Get("Owner")].Remove(card);
        
        foreach (var effect in OnPlay) effect.Do(manager, card.GetComponent<CardData>().Get("Owner"));
    }

    public virtual void Draw(Manager manager, int owner){
        var card = InstantiateCard(manager, owner);
        manager.ChangeCardField(card, manager.LayoutUI.PlayerHands[card.GetComponent<CardData>().Get("Owner")],
            () => {
                foreach (var effect in OnDraw){
                    effect.Do(manager, owner);
                }
            });
    }

    public virtual void Destroy(Manager manager, GameObject card){
        manager.PlayerBoards[card.GetComponent<CardData>().Get("Owner")].Remove(card);
        int owner = card.GetComponent<CardData>().Get("Owner");
        manager.ChangeCardField(card, manager.LayoutUI.PlayerDiscard[owner], () => {
            foreach (var effect in OnDestroy){
                effect.Do(manager, card.GetComponent<CardData>().Get("Owner"));
            }

            Destroy(card);
        });
    }

    protected GameObject InstantiateCard(Manager manager, int owner){
        var newCard = Instantiate(manager.CardPrefab, manager.LayoutUI.PlayerDraw[owner].transform);
        manager.PlayerHands[owner].Add(newCard);

        var data = newCard.GetComponent<CardData>();
        data.Add("Owner", owner);
        
        var UI = newCard.GetComponent<CardUI>();
        UI.SetCard(this);

        return newCard;
    }
}