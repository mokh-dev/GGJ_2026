using UnityEngine;

public class GameManager : MonoBehaviour
{

    private static GameManager instance;

    public static GameManager Instance { get { return instance; } }

    public bool CompletedEliminationMission;
    public bool CompletedCureMission;
    public bool CompletedSaveMission;
    public bool CompletedTimeMission = true;

    [SerializeField] private int _requiredEnemyEliminatedAmount;
    [SerializeField] private int _requiredHostageCuredAmount;
    [SerializeField] private int _requiredHostageSavedAmount;
    [SerializeField] private int _requiredCompletionTime;


    public int EnemyEliminatedAmount {get; private set;}
    public int HostageCuredAmount {get; private set;}
    public int HostageSavedAmount {get; private set;}

    public float TimePassed;

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
            TimePassed += Time.deltaTime;
            if ((CompletedTimeMission == true) && (TimePassed > _requiredCompletionTime)) CompletedTimeMission = false;
        } 
    }

    public void PlayerExited()
    {
        gameActive = false;

        if (CompletedLevel() == true) Debug.Log("Level Complete!");
    }

    private bool CompletedLevel()
    {
        if (EnemyEliminatedAmount >= _requiredEnemyEliminatedAmount) CompletedEliminationMission = true;
        if (HostageCuredAmount >= _requiredHostageCuredAmount) CompletedCureMission = true;
        if (HostageSavedAmount >= _requiredHostageSavedAmount) CompletedEliminationMission = true;


        return true;
    }

    public void HostageCured()
    {
        HostageCuredAmount++;
    }

    public void EnemyEliminated()
    {
        EnemyEliminatedAmount++;
    }
}
