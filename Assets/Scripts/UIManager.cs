using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{

    [SerializeField] private Color _missionIncompleteColor;
    [SerializeField] private Color _missionCompleteColor;


    [SerializeField] private TextMeshProUGUI _completionTimeText;
    [SerializeField] private TextMeshProUGUI _enemyEliminatedText;
    [SerializeField] private TextMeshProUGUI _hostageCuredText;
    [SerializeField] private TextMeshProUGUI _hostageSavedText;
    private GameManager gameManager;




    void Start()
    {
        gameManager = GameManager.Instance;
    }


    void Update()
    {
        // UpdateMissionStatusUI(gameManager.CompletedTimeMission, _completionTimeText);
        // UpdateMissionStatusUI(gameManager.CompletedEliminationMission, _enemyEliminatedText);
        // UpdateMissionStatusUI(gameManager.CompletedCureMission, _hostageCuredText);
        // UpdateMissionStatusUI(gameManager.CompletedSaveMission, _hostageSavedText);
    }

    private void UpdateMissionStatusUI(bool missionCompletionStatus, float currentMissionStatusData, TextMeshProUGUI missionText)
    {
        if (missionText == null) return;

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
