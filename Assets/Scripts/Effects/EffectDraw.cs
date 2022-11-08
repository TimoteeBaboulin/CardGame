using UnityEngine;

[CreateAssetMenu(menuName = "Effects/Draw", fileName = "DrawOne", order = 1)]
public class EffectDraw : CardEffect{
    public int CardsDrawn = 1;
    public Target Target = Target.self;

    public override void Do(Manager manager, int owner){
        switch (Target){
            case Target.self:
                manager.DrawCards(owner, CardsDrawn);
                break;

            case Target.enemy:
                manager.DrawCards(owner == 0 ? 1 : 0, CardsDrawn);
                break;

            case Target.both:
                manager.DrawCards(0, CardsDrawn);
                manager.DrawCards(1, CardsDrawn);
                break;
        }
    }
}