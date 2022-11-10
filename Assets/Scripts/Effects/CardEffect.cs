using UnityEngine;

public abstract class CardEffect : ScriptableObject{
    public enum Target{
        Self,
        Enemy,
        Both
    }

    public abstract void Do(GameObject card);
}