using UnityEngine;

public class PlayerConditionUIConnector : MonoBehaviour
{
    private PlayerCondition playerCondition;
    private ConditionUI conditionUI;

    private void Start()
    {
        playerCondition = GetComponent<PlayerCondition>();
        UIManager.Instance.Enable<ConditionUI>();
        conditionUI = UIManager.Instance.Get<ConditionUI>();
    }

    private void Update()
    {
        if (conditionUI == null) return;
        conditionUI.SetHP(playerCondition.Health);
        conditionUI.SetRed(playerCondition.RedTemperature);
        conditionUI.SetBlue(playerCondition.BlueTemperature);
        conditionUI.SetGreen(playerCondition.Stamina);
        conditionUI.SetWater(playerCondition.Water);
    }
}