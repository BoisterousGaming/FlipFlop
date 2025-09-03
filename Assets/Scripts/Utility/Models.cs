using System;
using System.Collections.Generic;

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
}
