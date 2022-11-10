using System;
using UnityEngine;

public class LayoutUI : MonoBehaviour{
    public GameObject[] PlayerFields;
    public GameObject[] PlayerHands;
    public GameObject[] PlayerDraws;
    public GameObject[] PlayerDiscards;
    public GameObject[] PlayerTerrains;
    public GameObject[] PlayerHealths;

    public GameObject GetField(Field field){
        switch (field.Type){
            case FieldType.Field:
                return PlayerFields[field.Player];
            case FieldType.Hand:
                return PlayerHands[field.Player];
            case FieldType.Draw:
                return PlayerDraws[field.Player];
            case FieldType.Discard:
                return PlayerDiscards[field.Player];
            case FieldType.Terrain:
                return PlayerTerrains[field.Player];
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}