using UnityEngine;

public enum GameState
{
    MainMenu,
    Playing,
    Paused,
    GameOver
}

// Track general game progression
public enum ProgressionState
{
    Cutscene0,
    Boss1,
    Custscene1,
    Boss2,
    Cutscene2,
    Boss3,
    Cutscene3,
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private ConfigFile config;

    private void Awake()
    {
        //singleton initializtion
        if (Instance != null)
        {
            Debug.LogWarning($"A later instance of {nameof(GameManager)} on {gameObject.name} was destroyed to preserve an earlier instance on {Instance.gameObject.name}.");
            DestroyImmediate(this);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        //Initialize static classes using current config file
        AudioManager.Init(config);
    }

    private void Update()
    {
        AudioManager.Update();
    }
}
