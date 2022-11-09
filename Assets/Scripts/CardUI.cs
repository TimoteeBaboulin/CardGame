using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardUI : MonoBehaviour{
    public Image Background;
    public TextMeshProUGUI Description;
    public GameObject Stats;
    public TextMeshProUGUI Health;
    public TextMeshProUGUI Defense;
    public TextMeshProUGUI Attack;

    public Card BaseCard{ get; private set; }

    public void SetCard(Card card){
        if (card == null) throw new NullReferenceException();

        BaseCard = card;
        Description.text = BaseCard.Description;
        Background.sprite = BaseCard.Sprite;

        if (card.Type != Card.CardType.Creature){
            Stats.SetActive(false);
        }
        else{
            Stats.SetActive(true);
            ChangeStats();
            var data = GetComponent<CardData>();
            data.OnChange += ChangeStats;
        }
    }

    private void ChangeStats(){
        var data = GetComponent<CardData>();
        Health.text = data.Get("Health").ToString();
        Defense.text = data.Get("Defense").ToString();
        Attack.text = data.Get("Attack").ToString();
    }
}