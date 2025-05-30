using UnityEngine;



[CreateAssetMenu(fileName = "ConsumeItemData", menuName = "ScriptableObjects/Temp/Consume Item Data")]
public class ConsumeItemDataSO : ItemDataSO
{
    [System.Serializable]
    public struct ConsumeState
    {
        public ConsumType consumetype;
        public float value;
    }
    
    public ConsumeState[] Infos => infos;

    [Space(10f)] 
    [Header("ConsumeInfo")] 
    [SerializeField] private ConsumeState[] infos;
}