
using DG.Tweening;
using UnityEngine;

public class NpcManager : MonoBehaviour
{

    public NPCUIController npcController;
    private GameObject _player;
    public Vector3 offset;
    public float followSpeed; // 따라오는 속도
    public float maxDistance; // 최대 거리 제한

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            npcController.dialogueText.text = "";
            string fullText = npcController.npcData.text;
            npcController.dialogueText.DOText(fullText, 1f).SetEase(Ease.Linear);
            
            foreach (NPCDialog npcDialog in npcController.npcData.npcDialog)
            {
                npcDialog.Reset();
            }

            npcController.OnOff();
        }
    }
    
    

    void Start()
    {
        _player = GameObject.Find("Player");
    }
    
    

    void Update()
    {
        Vector3 targetPosition = _player.transform.position + offset;
        float distance = Vector3.Distance(transform.position, targetPosition);

        if (distance > maxDistance)
        {
            // 거리가 너무 멀면, 요정을 부드럽게 이동시켜 따라오게 함
            transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
        }
        
        transform.LookAt(_player.transform);

    }
}
