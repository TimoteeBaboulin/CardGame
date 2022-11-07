using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CardEffect : ScriptableObject{
    public enum Target{
        self,
        enemy,
        both
    }
    
    public abstract void Do(Manager manager, int owner);
}