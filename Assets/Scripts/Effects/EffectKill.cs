using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(menuName = "Effects/Kill", fileName = "KillOne", order = 0)]
public class EffectKill : CardEffect{
    public int NumberKilled = 1;
    public Target Target = Target.Enemy;

    public override void Do(GameObject card){
        GameObject[] cardsDestroyed;

        int owner = card.GetComponent<CardData>().Get("Owner");
        switch (Target){
            case Target.Self:
                cardsDestroyed = KillCards(Manager.Instance.PlayerBoards[owner], NumberKilled);
                break;

            case Target.Enemy:
                cardsDestroyed = KillCards(Manager.Instance.PlayerBoards[owner == 1 ? 0 : 1], NumberKilled);
                break;

            case Target.Both:
                var cards = new List<GameObject>();
                cards = Manager.Instance.PlayerBoards[0].Union(Manager.Instance.PlayerBoards[1]).ToList();
                cardsDestroyed = KillCards(cards, NumberKilled);
                break;

            default:
                cardsDestroyed = Array.Empty<GameObject>();
                break;
        }

        foreach (var destroyedCard in cardsDestroyed) destroyedCard.GetComponent<CardUI>().BaseCard.Destroy(destroyedCard);
    }

    private static GameObject[] KillCards(List<GameObject> cards, int number){
        if (number >= cards.Count) return cards.ToArray();

        var array = new GameObject[number];

        for (var x = 0; x < number; x++){
            var rng = Random.Range(0, cards.Count);
            array[x] = cards[rng];
        }

        return array;
    }
}