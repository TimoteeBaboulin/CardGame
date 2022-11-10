using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Cards{
    public abstract class Card : ScriptableObject{
        protected static Action<GameObject> OnCardPlayed;
        protected static Action<GameObject> OnCardDestroyed;

        public enum CardType{
            Creature,
            Spell,
            Terrain,
            Draw
        }

        public bool IsQuickPlay;
        public string Name;
        public string Description;
        public Sprite Sprite;

        public List<CardEffect> OnDraw = new();
        public List<CardEffect> OnPlay = new();
        public List<CardEffect> OnDestroy = new();

        public abstract CardType Type{ get; }

        /// <summary>
        /// Method called when the card is played from the player's hand
        /// </summary>
        /// <param name="card"></param>
        public virtual void Played(GameObject card){
            PlayCard(card);

            foreach (var effect in OnPlay){
                effect.Do(card);
            }
        }

        /// <summary>
        /// Method called when the card is drawn from the deck
        /// </summary>
        /// <param name="owner"></param>
        public virtual void Draw(int owner){
            var card = InstantiateCard(owner);
            Manager.Instance.ChangeCardField(card, new Field(owner, FieldType.Hand),
                () => {
                    foreach (var effect in OnDraw){
                        effect.Do(card);
                    }
                });
        }

        /// <summary>
        /// Method called when the card is killed or destroyed (whether in battle or through card effect)
        /// </summary>
        /// <param name="card"></param>
        public virtual void Destroy(GameObject card){
            int owner = card.GetComponent<CardData>().Get("Owner");
            Manager.Instance.PlayerBoards[owner].Remove(card);
            
            Manager.Instance.ChangeCardField(card, new Field(owner, FieldType.Discard), () => {
                foreach (var effect in OnDestroy){
                    effect.Do(card);
                }

                Object.Destroy(card);
            });
            OnCardDestroyed?.Invoke(card);
        }

        public virtual void Attack(GameObject attacker, GameObject defender){}
    
        protected GameObject InstantiateCard(int owner){
            var newCard = Instantiate(Manager.Instance.CardPrefab, Manager.Instance.LayoutUI.PlayerDraws[owner].transform);
            Manager.Instance.PlayerHands[owner].Add(newCard);

            var data = newCard.GetComponent<CardData>();
            data.Add("Health", 0);
            data.Add("Defense", 0);
            data.Add("Attack", 0);
            data.Add("Owner", owner);
            
            newCard.GetComponent<CardUI>().SetCard(this);

            return newCard;
        }

        protected static void PlayCard(GameObject card){
            Manager.Instance.PlayerHands[card.GetComponent<CardData>().Get("Owner")].Remove(card);
            OnCardPlayed?.Invoke(card);
        }
    }
}