using UnityEngine;

namespace Cards{
    [CreateAssetMenu(menuName = "Card/Spell", fileName = "NewCardSpell", order = 0)]
    public class CardSpell : Card{
        public override CardType Type => CardType.Spell;

        public override void Played(GameObject card){
            base.Played(card);
            Destroy(card);
        }
    }
}