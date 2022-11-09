using UnityEngine;

public abstract class CardEffect : ScriptableObject{
    public enum Target{
        self,
        enemy,
        both
    }

    public abstract void Do(Manager manager, GameObject card);
}