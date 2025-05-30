using UnityEngine;

public enum ConsumType
{
    health,
    water,
    hunger,
}

[System.Serializable]
public class ItemConsumeInfo
{
    [System.Serializable]
    public struct ConsumeState
    {
        public ConsumType consumetype;
        public float value;
    }

    public ConsumeState[] states;
}