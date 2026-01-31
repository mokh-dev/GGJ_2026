using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{

    [SerializeField] private Color _missionIncompleteColor;
    [SerializeField] private Color _missionCompleteColor;


    [SerializeField] private TextMeshProUGUI _completionTimeText;
    [SerializeField] private TextMeshProUGUI _enemyEliminatedText;
    [SerializeField] private TextMeshProUGUI _enemyDetectionText;
    [SerializeField] private TextMeshProUGUI _hostageCuredText;
    [SerializeField] private TextMeshProUGUI _hostageSavedText;
    private GameManager gameManager;




    void Start()
    {
        gameManager = GameManager.Instance;
    }


    void Update()
    {
        if (gameManager.LevelHasTimeMission) UpdateMissionStatusUI(gameManager.CompletedTimeMission, gameManager.TimePassed, gameManager.RequiredCompletionTime, _completionTimeText);
        if (gameManager.LevelHasEliminationMission) UpdateMissionStatusUI(gameManager.CompletedEliminationMission, gameManager.EnemyEliminatedAmount, gameManager.RequiredMaxEliminationsAmount,  _enemyEliminatedText);
        if (gameManager.LevelHasDetectionMission) UpdateMissionStatusUI(gameManager.CompletedDetectionMission, gameManager.EnemyDetectionAmount, gameManager.RequiredMaximumDetectionAmount,  _enemyDetectionText);
        if (gameManager.LevelHasCureMission) UpdateMissionStatusUI(gameManager.CompletedCureMission, gameManager.HostageCuredAmount, gameManager.RequiredHostageCuredAmount, _hostageCuredText);
        if (gameManager.LevelHasSaveMission) UpdateMissionStatusUI(gameManager.CompletedSaveMission, gameManager.HostageSavedAmount, gameManager.RequiredHostageSavedAmount, _hostageSavedText);
    }

    private void UpdateMissionStatusUI(bool missionCompletionStatus, float currentMissionStatusData, float requiredMissionData, TextMeshProUGUI missionText)
    {
        if (missionText == null) return;

        if (missionText != _completionTimeText)
        {
            missionText.text = ((int)currentMissionStatusData).ToString() + " / " + ((int)requiredMissionData).ToString(); 
        }
        else
        {
            missionText.text = Mathf.RoundToInt(currentMissionStatusData).ToString() + " / " + requiredMissionData.ToString(); 
        }
        
        

        if (missionCompletionStatus == true)
        {
            missionText.color = _missionCompleteColor;
        }
        else
        {
            missionText.color = _missionIncompleteColor;
        }
    }
}
