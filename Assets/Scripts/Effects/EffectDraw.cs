using UnityEngine;

public class EffectDraw : CardEffect{
    public int CardsDrawn = 1;
    public Target Target = Target.both;

    public override void Do(Manager manager, int owner){
        if (Target == Target.both){
            for (int x = 0; x < 2; x++){
                manager.PlayerHands[x].Add(CreateInstance<Creaturax>());
            }
            return;
        }

        if (Target == Target.self){
            manager.PlayerHands[owner].Add(CreateInstance<Creaturax>());
        }

        if (Target == Target.enemy){
            if (owner == 0) manager.PlayerHands[1].Add(CreateInstance<Creaturax>());
            else manager.PlayerHands[0].Add(CreateInstance<Creaturax>());
        }
    }
}