using UnityEngine;

namespace Cards{
    [CreateAssetMenu(menuName = "Card/Spell", fileName = "NewCardSpell", order = 0)]
    public class CardSpell : Card{
        public override CardType Type => CardType.Spell;

        public override void Played(Manager manager, GameObject card){
            base.Played(manager, card);
            Destroy(manager, card);
        }
    }
}