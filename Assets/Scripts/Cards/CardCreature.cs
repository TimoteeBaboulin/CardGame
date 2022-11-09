using UnityEngine;

namespace Cards{
    [CreateAssetMenu(menuName = "Card/Creature", fileName = "CardCreature", order = 0)]
    public class CardCreature : Card{
        public int BaseHealth = 4;
        public int BaseDefense;
        public int BaseAttack = 2;

        public override CardType Type => CardType.Creature;

        public override void Draw(Manager manager, int owner){
            var newCard = Instantiate(manager.CardPrefab, manager.LayoutUI.PlayerDraws[owner].transform);
            manager.PlayerHands[owner].Add(newCard);

            var data = newCard.GetComponent<CardData>();
            data.Add("Health", BaseHealth);
            data.Add("Defense", BaseDefense);
            data.Add("Attack", BaseAttack);
            data.Add("Owner", owner);
        
            var UI = newCard.GetComponent<CardUI>();
            UI.SetCard(this);
            
            manager.ChangeCardField(newCard, manager.LayoutUI.PlayerHands[newCard.GetComponent<CardData>().Get("Owner")],
                () => {
                    foreach (var effect in OnDraw){
                        effect.Do(manager, newCard);
                    }
                });
        }

        public override void Played(Manager manager, GameObject card){
            int owner = card.GetComponent<CardData>().Get("Owner");
            manager.PlayerBoards[owner].Add(card);
            manager.PlayerHands[owner].Remove(card);
            OnCardPlayed?.Invoke(manager, card);

            manager.ChangeCardField(card, manager.LayoutUI.PlayerFields[owner],()=> {
                foreach (var effect in OnPlay) effect.Do(manager, card);
            });
        }
    }
}