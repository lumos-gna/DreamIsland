
using UnityEngine;


[CreateAssetMenu(fileName = "NPC", menuName = "NPC/NPCData")]
public class NpcData : ScriptableObject
{
    public Sprite Sprite;   //NPC 사진
    public string text;     //대화 시작시 텍스트
    public NPCData[] npcDatas;  //대화의 가짓수

    
    public string NextText(int selectedDialogue)   //대화 텍스트 전달
    {
        return npcDatas[selectedDialogue].GetText();
    }
    
    
    public bool TryGetNextDialog(int selectedDialogue, out NPCDialog dialog)
    {
        return npcDatas[selectedDialogue].TryGetNextDialog(out dialog);
    }
}
