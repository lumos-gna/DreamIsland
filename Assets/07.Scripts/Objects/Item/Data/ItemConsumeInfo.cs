using UnityEngine;



[System.Serializable]
public class ItemConsumeInfo
{
    [System.Serializable]
    public struct ConsumeState
    {
        public ConditionType type;
        public float value;
    }

    public ConsumeState[] states;
}