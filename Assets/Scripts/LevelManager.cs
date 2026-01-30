using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelManager : MonoBehaviour
{

    public void LoadLevel(int levelToLoad)
    {
        SceneManager.LoadScene(levelToLoad);    
    }

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    void Update()
    {
        
    }
}
