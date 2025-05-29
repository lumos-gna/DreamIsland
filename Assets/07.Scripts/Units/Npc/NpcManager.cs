
using DG.Tweening;
using UnityEngine;

public class NpcManager : MonoBehaviour
{

    public NPCUIController npcController;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //npcController.dialogueText.text = npcController.npcData.text;
            npcController.dialogueText.text = "";
            string fullText = npcController.npcData.text;
            npcController.dialogueText.DOText(fullText, 1f).SetEase(Ease.Linear);
            
            foreach (var VARIABLE in npcController.npcData.npcDialog)
            {
                VARIABLE.Reset();
            }

            npcController.OnOff();
        }
    }
}
