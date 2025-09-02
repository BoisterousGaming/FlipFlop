using System;

public abstract class Models
{
    [Serializable]
    public class SerializableKeyValuePair<TKey, TValue>
    {
        public TKey key;
        public TValue value;
    }
}
