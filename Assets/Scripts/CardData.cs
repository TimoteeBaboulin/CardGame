using System;
using System.Collections.Generic;
using UnityEngine;

public class CardData : MonoBehaviour{
    private readonly Dictionary<string, int> _data = new();
    public event Action OnChange;

    public bool Contains(string key){
        if (_data.ContainsKey(key)) return true;
        return false;
    }

    public int Get(string key){
        if (!_data.ContainsKey(key))
            throw new NullReferenceException("Please check that the value exists before accessing it.");
        return _data[key];
    }

    public bool Change(string key, int value){
        if (!_data.ContainsKey(key))
            throw new NullReferenceException("Please check that the value exists before accessing it.");

        if (_data[key] == value) return false;

        _data[key] = value;
        OnChange?.Invoke();
        return true;
    }

    public void Add(string key, int value){
        if (_data.ContainsKey(key))
            throw new NullReferenceException("Please check that the value doesn't exists before creating it.");

        _data.Add(key, value);
    }

    [ContextMenu("RemoveHealth")]
    public void RemoveHealth(){
        Change("Health", Get("Health") - 1);
    }
}