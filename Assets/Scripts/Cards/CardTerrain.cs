using System.Collections.Generic;
using UnityEngine;

namespace Cards{
    [CreateAssetMenu(menuName = "Card/Terrain", fileName = "NewCardTerrain", order = 0)]
    public class CardTerrain : Card{
        public override CardType Type => CardType.Terrain;
        public List<CardEffect> OnTurnChange = new();
        
        public override void Played(GameObject card){
            int owner = card.GetComponent<CardData>().Get("Owner");
            Manager.Instance.PlayerHands[owner].Remove(card);
            OnCardPlayed?.Invoke(card);

            Manager.Instance.ChangeCardField(card, new Field(owner, FieldType.Terrain), () => {
                if (Manager.Instance.PlayerTerrains[owner] != null)
                    Destroy(Manager.Instance.PlayerTerrains[owner]);
                Manager.Instance.PlayerTerrains[owner] = card;
                
                LinkEffects();
            });
        }

        public override void Destroy(GameObject card){
            UnLinkEffects();
            base.Destroy(card);
        }

        private void LinkEffects(){
            foreach (var effect in OnPlay){
                OnCardPlayed += effect.Do;
            }

            foreach (var effect in OnDestroy){
                OnCardDestroyed += effect.Do;
            }
        }
        
        private void UnLinkEffects(){
            foreach (var effect in OnPlay){
                OnCardPlayed -= effect.Do;
            }

            foreach (var effect in OnDestroy){
                OnCardDestroyed -= effect.Do;
            }
        }
    }
}