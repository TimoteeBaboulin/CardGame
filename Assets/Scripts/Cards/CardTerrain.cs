using System.Collections.Generic;
using UnityEngine;

namespace Cards{
    [CreateAssetMenu(menuName = "Card/Terrain", fileName = "NewCardTerrain", order = 0)]
    public class CardTerrain : Card{
        public override CardType Type => CardType.Terrain;
        public List<CardEffect> OnTurnChange = new();
        
        public override void Played(Manager manager, GameObject card){
            int owner = card.GetComponent<CardData>().Get("Owner");
            manager.PlayerHands[owner].Remove(card);
            OnCardPlayed?.Invoke(manager, card);

            manager.ChangeCardField(card, manager.LayoutUI.PlayerTerrains[owner], () => {
                if (manager.PlayerTerrains[owner] != null)
                    Destroy(manager, manager.PlayerTerrains[owner]);
                manager.PlayerTerrains[owner] = card;
                
                LinkEffects();
            });
        }

        public override void Destroy(Manager manager, GameObject card){
            UnLinkEffects();
            base.Destroy(manager, card);
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