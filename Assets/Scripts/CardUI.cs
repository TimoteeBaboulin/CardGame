using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardUI : MonoBehaviour{
    public Image Background;
    public TextMeshProUGUI Description;
    public Card BaseCard;

    public GameObject Stats;
    public TextMeshProUGUI Health;
    public TextMeshProUGUI Defense;
    public TextMeshProUGUI Attack;

    public void SetCard(Card card){
        if (card == null) throw new NullReferenceException();

        BaseCard = card;
        Description.text = BaseCard.Description;

        if (card.Type != Card.CardType.Creature){
            Stats.SetActive(false);
        }
        else{
            Stats.SetActive(true);
            var creature = card as CardCreature;
            Health.text = creature.BaseHealth.ToString();
            Defense.text = creature.BaseDefense.ToString();
            Attack.text = creature.BaseAttack.ToString();
        }
    }
}