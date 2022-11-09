using UnityEngine;

namespace Cards{
    [CreateAssetMenu(menuName = "Card/Draw", fileName = "NewCardDraw", order = 4)]
    public class CardDraw : Card{
        public override CardType Type => CardType.Draw;

        public override void Draw(Manager manager, int owner){
            var newCard = InstantiateCard(manager, owner);

            foreach (var effect in OnDraw){
                effect.Do(manager, newCard);
            }
        
            Played(manager, newCard);
        }
    }
}