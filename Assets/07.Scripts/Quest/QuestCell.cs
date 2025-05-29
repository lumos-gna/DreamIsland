using TMPro;
using UnityEngine;

public class QuestCell : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI questText;
    [SerializeField] private TextMeshProUGUI countText;

    public void SetData(string quest, int count, int goal)
    {
        questText.text = quest;
        countText.text = count+" / "+goal;
    }
}