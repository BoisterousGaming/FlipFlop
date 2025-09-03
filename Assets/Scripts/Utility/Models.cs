using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerializableKeyValuePair<TKey, TValue>
{
    public TKey key;
    public TValue value;
}

[Serializable]
public class SaveData
{
    public int score;
    public GridData gridData;
}

[Serializable]
public class GridData
{
    public int rows;
    public int columns;
    public List<int> cardIDs = new();
    public List<int> destroyedCardIDs = new();
    public List<Vector2Serializable> cardPositions = new();
    public Vector2Serializable cardSize; 
}

[Serializable]
public struct Vector2Serializable
{
    public float x;
    public float y;

    public Vector2Serializable(float x, float y)
    {
        this.x = x;
        this.y = y;
    }

    public static implicit operator Vector2(Vector2Serializable v) => new Vector2(v.x, v.y);
    public static implicit operator Vector2Serializable(Vector2 v) => new Vector2Serializable(v.x, v.y);
}
