
using UnityEngine;

[CreateAssetMenu(menuName = "Create Creaturax", fileName = "Creaturax", order = 0)]
public class Creaturax : CardCreature{
    public override void Played(Manager manager, int owner){
        throw new System.NotImplementedException();
    }

    public override void Drawn(Manager manager, int owner){
        throw new System.NotImplementedException();
    }

    public override void Destroyed(Manager manager, int owner){
        throw new System.NotImplementedException();
    }
}