
using UnityEngine;

public class NpcManager : MonoBehaviour
{

    public NPCUIController npcController;

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log(other.gameObject.name);
        if (other.gameObject.CompareTag("Player"))
        {
            npcController.dialogueText.text = npcController.npcData.text;
            foreach (var VARIABLE in npcController.npcData.npcDialog)
            {
                VARIABLE.Reset();
            }

            npcController.OnOff();
        }
    }
    
    
    //테스트용
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0)|| Input.GetKeyDown(KeyCode.Keypad0))
        {
            npcController.dialogueText.text = npcController.npcData.text;
            npcController.OnOff();
        }
    }
}
