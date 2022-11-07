using UnityEngine;

public abstract class Card : ScriptableObject{
    public enum CardType{
        Creature,
        Spell,
        Terrain,
        Draw
    }
    public bool IsQuickPlay = false;
    public string Name;
    public string Description;
    
    public abstract CardType Type{ get; }

    public abstract void Played(Manager manager, int owner);
    public abstract void Drawn(Manager manager, int owner);
    public abstract void Destroyed(Manager manager, int owner);
}