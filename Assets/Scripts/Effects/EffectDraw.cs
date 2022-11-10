using UnityEngine;

[CreateAssetMenu(menuName = "Effects/Draw", fileName = "DrawOne", order = 1)]
public class EffectDraw : CardEffect{
    public int CardsDrawn = 1;
    public Target Target = Target.Self;

    public override void Do(GameObject card){
        int owner = card.GetComponent<CardData>().Get("Owner");
        
        switch (Target){
            case Target.Self:
                Manager.Instance.DrawCards(owner, CardsDrawn);
                break;

            case Target.Enemy:
                Manager.Instance.DrawCards(owner == 0 ? 1 : 0, CardsDrawn);
                break;

            case Target.Both:
                Manager.Instance.DrawCards(0, CardsDrawn);
                Manager.Instance.DrawCards(1, CardsDrawn);
                break;
        }
    }
}