using UnityEngine;



[CreateAssetMenu(fileName = "ConsumeItemData", menuName = "ScriptableObjects/Temp/Consume Item Data")]
public class ConsumeItemDataSO : ItemDataSO
{
    [System.Serializable]
    public struct ConsumeInfo
    {
        public ConsumType consumetype;
        public float value;
    }
    
    public ConsumeInfo[] Infos => infos;

    [Space(10f)] 
    [Header("ConsumeInfo")] 
    [SerializeField] private ConsumeInfo[] infos;
}