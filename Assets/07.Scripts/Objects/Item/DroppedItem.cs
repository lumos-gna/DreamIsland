using System.Collections;
using UnityEngine;
using DG.Tweening;


public class DroppedItem : MonoBehaviour, IInteractable
{
    public Outline Outline => _outline;

    private Outline _outline;

    private void Awake()
    {
        _outline = GetComponent<Outline>();
    }


    //테스트용
    [SerializeField] private ItemData testItemData;

    private void Start()
    {
        Init(testItemData);
    }
    public ItemData ItemData { get; private set; }

    public void Init(ItemData data)
    {
        if (!data.IsDroppable)
            return;

        ItemData = data;
        PlayDropEffect();
    }

    public void OnInteract()
    {
        GameManager.Instance.Inventory.AddItem(ItemData);

        Destroy(gameObject);
    }

    private void PlayDropEffect()
    {
        Vector3 originPos = transform.position;

        Vector3 randomOffset = new Vector3(Random.Range(-0.3f, 0.3f), 0.6f, Random.Range(-0.3f, 0.3f));

        Sequence sequence = DOTween.Sequence();

        sequence.Append(transform.DOMove(originPos + randomOffset, 0.3f).SetEase(Ease.OutQuad));
        sequence.Append(transform.DOMove(originPos, 0.2f).SetEase(Ease.InQuad));
    }
}
