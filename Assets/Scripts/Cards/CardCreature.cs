using Unity.VisualScripting;
using UnityEngine;

namespace Cards{
    [CreateAssetMenu(menuName = "Card/Creature", fileName = "CardCreature", order = 0)]
    public class CardCreature : Card{
        public int BaseHealth = 4;
        public int BaseDefense;
        public int BaseAttack = 2;

        public override CardType Type => CardType.Creature;

        public override void Draw(int owner){
            var newCard = InstantiateCard(owner);
            var data = newCard.GetComponent<CardData>();
            data.Change("Health", 0);
            data.Change("Defense", 0);
            data.Change("Attack", 0);

            Manager.Instance.ChangeCardField(newCard, new Field(owner, FieldType.Hand),
                () => {
                    foreach (var effect in OnDraw){
                        effect.Do(newCard);
                    }
                });
        }

        public override void Played(GameObject card){
            var owner = card.GetComponent<CardData>().Get("Owner");
            
            Manager.Instance.PlayerBoards[owner].Add(card);
            PlayCard(card);

            card.GetComponent<CardData>().CanAttack = false;
            Manager.Instance.ChangeCardField(card, new Field(owner, FieldType.Field),()=> {
                foreach (var effect in OnPlay) effect.Do(card);
            });
        }

        public override void Attack(GameObject attacker, GameObject defender){
            if (attacker.GetComponent<CardUI>().BaseCard.Type != CardType.Creature || defender.GetComponent<CardUI>().BaseCard.Type != CardType.Creature) return;
            if (!attacker.GetComponent<CardData>().CanAttack) return;
            
            int attack = attacker.GetComponent<CardData>().Get("Attack");
            int defense = defender.GetComponent<CardData>().Get("Defense");
            int health = defender.GetComponent<CardData>().Get("Health");

            if (health <= attack - defense)
                Destroy(defender);
                
            defender.GetComponent<CardData>().Change("Health", health - (attack - defense));
            attacker.transform.SetParent(Manager.Instance.LayoutUI.PlayerFields[attacker.GetComponent<CardData>().Get("Owner")].transform);
        }

        private void UpdateAttack(GameObject card){
            
        }
    }
}