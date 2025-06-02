using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestCellFactory : MonoBehaviour
{
    public GameObject questGroup;
    public GameObject questCellPrefab;

    public GameObject CreateQuestCell(Quest questData)
    {
        GameObject newQuestCell = Instantiate(questCellPrefab, questGroup.transform);

        // 하위 오브젝트에서 QuestText와 Count 찾기
        TextMeshProUGUI questText = newQuestCell.transform.Find("QuestText")?.GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI countText = newQuestCell.transform.Find("Count")?.GetComponent<TextMeshProUGUI>();

        if (questText != null) questText.text = questData.uiText;
        if (countText != null) countText.text = $"{questData.Count} / {questData.goal}";

        return newQuestCell;
    }
}