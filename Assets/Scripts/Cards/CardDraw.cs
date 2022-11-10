using UnityEngine;

namespace Cards{
    [CreateAssetMenu(menuName = "Card/Draw", fileName = "NewCardDraw", order = 4)]
    public class CardDraw : Card{
        public override CardType Type => CardType.Draw;

        public override void Draw(int owner){
            var newCard = InstantiateCard(owner);

            foreach (var effect in OnDraw){
                effect.Do(newCard);
            }
        
            Played(newCard);
        }
    }
}