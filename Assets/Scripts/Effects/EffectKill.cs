using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Effects/Kill", fileName = "KillOne", order = 0)]
public class EffectKill : CardEffect{
    public int NumberKilled = 1;
    public Target Target = Target.enemy;

    public override void Do(Manager manager, int owner){
        GameObject[] cardsDestroyed;

        switch (Target){
            case Target.self:
                cardsDestroyed = KillCards(manager.PlayerBoards[owner], NumberKilled);
                break;

            case Target.enemy:
                cardsDestroyed = KillCards(manager.PlayerBoards[owner == 1 ? 0 : 1], NumberKilled);
                break;

            case Target.both:
                var cards = new List<GameObject>();
                cards = manager.PlayerBoards[0].Union(manager.PlayerBoards[1]).ToList();
                cardsDestroyed = KillCards(cards, NumberKilled);
                break;

            default:
                cardsDestroyed = new GameObject[0];
                break;
        }

        foreach (var card in cardsDestroyed) card.GetComponent<CardUI>().BaseCard.Destroy(manager, card);
    }

    private GameObject[] KillCards(List<GameObject> cards, int number){
        if (number >= cards.Count) return cards.ToArray();

        var array = new GameObject[number];

        for (var x = 0; x < number; x++){
            var rng = Random.Range(0, cards.Count);
            array[x] = cards[rng];
        }

        return array;
    }
}