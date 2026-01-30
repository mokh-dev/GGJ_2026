using UnityEngine;

public class GameManager : MonoBehaviour
{

    private static GameManager instance;

    public static GameManager Instance { get { return instance; } }

    public bool LevelHasEliminationMission;
    public bool LevelHasDetectionMission;
    public bool LevelHasCureMission;
    public bool LevelHasSaveMission;
    public bool LevelHasTimeMission;

    public bool CompletedEliminationMission;
    public bool CompletedDetectionMission;
    public bool CompletedCureMission;
    public bool CompletedSaveMission;
    public bool CompletedTimeMission;

    public int RequiredEnemyEliminatedAmount;
    public int RequiredHostageCuredAmount;
    public int RequiredHostageSavedAmount;
    public int RequiredCompletionTime;
    public int RequiredMaximumDetectionAmount;


    public int EnemyEliminatedAmount {get; private set;}
    public int EnemyDetectionAmount {get; private set;}
    public int HostageCuredAmount {get; private set;}
    public int HostageSavedAmount {get; private set;}

    public float TimePassed {get; private set;}

    private bool gameActive = true;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        } 
        else 
        {
            instance = this;
        }
    }

    
    void Start()
    {

    }

    void Update()
    {
        if (gameActive)
        {
            MissionChecks();

            TimePassed += Time.deltaTime;
            if ((CompletedTimeMission == true) && (TimePassed > (float)RequiredCompletionTime)) CompletedTimeMission = false;
        } 
    }

    public void PlayerExited()
    {
        gameActive = false;
    }

    private void MissionChecks()
    {
        if (EnemyEliminatedAmount >= RequiredEnemyEliminatedAmount) CompletedEliminationMission = true;
        if (HostageCuredAmount >= RequiredHostageCuredAmount) CompletedCureMission = true;
        if (HostageSavedAmount >= RequiredHostageSavedAmount) CompletedSaveMission = true;
        if (EnemyDetectionAmount >= RequiredMaximumDetectionAmount) CompletedDetectionMission = false;
    }

    public void HostageCured()
    {
        HostageCuredAmount++;
    }
    public void HostageSaved()
    {
        HostageSavedAmount++;
    }

    public void EnemyEliminated()
    {
        EnemyEliminatedAmount++;
    }

    public void EnemyDetected()
    {
        EnemyDetectionAmount++;
    }
}
