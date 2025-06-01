
using DG.Tweening;
using UnityEngine;

public class NpcManager : MonoBehaviour
{

    public NPCUIController npcController;
    private GameObject _player;
    public Vector3 offset;
    public GameObject model;
    public float followSpeed; // 따라오는 속도
    public float maxDistance; // 최대 거리 제한

    public NpcData forestData;
    public NpcData snowData;
    public NpcData desertData;
    
    private Animator _animator;

    public void TalkWithFairy()
    {
        npcController.dialogueText.text = "";
        string fullText = npcController.npcData.text;
        npcController.dialogueText.DOText(fullText, 1f).SetUpdate(true).SetEase(Ease.Linear);
            
        foreach (NpcDialog npcDialog in npcController.npcData.npcDialog)
        {
            npcDialog.Reset();
        }

        npcController.OnOff();
    }
    
    //npc 데이터 바꾸기
    public void ChangeData(Region region)
    {
        switch (region)
        {
            case Region.Forest:
                npcController.npcData = forestData;
                break;
            case Region.Arctic:
                npcController.npcData = snowData;
                QuestManager.Instance.QuestPlusCount("메인 퀘스트1");
                break;
            case Region.Desert:
                npcController.npcData = desertData;
                QuestManager.Instance.QuestPlusCount("메인 퀘스트2");
                break;
        }
    }

    
    void Start()
    {
        _player = GameObject.Find("Player");
        _animator = model.GetComponent<Animator>();
    }
    
    

    void Update()
    {
        Vector3 targetPosition = _player.transform.position + offset;
        float distance = Vector3.Distance(transform.position, targetPosition);

        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
        
        transform.LookAt(_player.transform);

        
        
    }
}
