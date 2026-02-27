using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameState State;

    private GameState previousState;

    [SerializeField] private FieldOfView FirstEnemyFOV;
    [SerializeField] private FieldOfView SecondEnemyFOV;
    [SerializeField] private FieldOfView ThirdEnemyFOV;
    [SerializeField] private FieldOfView FourthEnemyFOV;
    [SerializeField] private FieldOfView FithEnemyFOV;
    [SerializeField] private FieldOfView SixthEnemyFOV;
    [SerializeField] private FieldOfView SeventhEnemyFOV;
    [SerializeField] private CollectibleCount CollectiblesCount;
    [SerializeField] private Collectible Collectibles;


    public static event Action<GameState> OnStateChange;

    void Awake()
    {
        Instance = this;
    }
    void Start ()
    {
        UpdateGameState(GameState.CollectStealth);
    }

    public void ToggleMenu()
    {
        if (State != GameState.Menu)
        {
            previousState = State;
            UpdateGameState(GameState.Menu);
        }
        else
        {
            UpdateGameState(previousState);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (State != GameState.Menu)
            {
                previousState = State; 
                UpdateGameState(GameState.Menu);
            }
            else
            {
                UpdateGameState(previousState);
            }
        }
        if (State == GameState.CollectStealth || State == GameState.CollectSeen)
        {
            bool anyoneSeesMe = AnyEnemySeesPlayer();
            if (anyoneSeesMe && State == GameState.CollectStealth)
                UpdateGameState(GameState.CollectSeen);
            else if (!anyoneSeesMe && State == GameState.CollectSeen)
                UpdateGameState(GameState.CollectStealth);

            if (IsInCachRange())
                UpdateGameState(GameState.Lose);

            if (CollectiblesCount != null && CollectiblesCount.count >= CollectiblesCount.collectibleTotal)
                UpdateGameState(GameState.Victory);
        }
    }
    private bool AnyEnemySeesPlayer()
    {
        return (FirstEnemyFOV != null && FirstEnemyFOV.canSeePlayer) ||
               (SecondEnemyFOV != null && SecondEnemyFOV.canSeePlayer) ||
               (ThirdEnemyFOV != null && ThirdEnemyFOV.canSeePlayer) ||
               (FourthEnemyFOV != null && ThirdEnemyFOV.canSeePlayer) ||
               (FithEnemyFOV != null && ThirdEnemyFOV.canSeePlayer) ||
               (SixthEnemyFOV != null && ThirdEnemyFOV.canSeePlayer) ||
               (SeventhEnemyFOV != null && FourthEnemyFOV.canSeePlayer);
    }

    private bool IsInCachRange()
    {
        return (FirstEnemyFOV != null && FirstEnemyFOV.cachDistance) ||
               (SecondEnemyFOV != null && SecondEnemyFOV.cachDistance) ||
               (ThirdEnemyFOV != null && ThirdEnemyFOV.cachDistance) ||
               (FourthEnemyFOV != null && ThirdEnemyFOV.cachDistance) ||
               (FithEnemyFOV != null && ThirdEnemyFOV.cachDistance) ||
               (SixthEnemyFOV != null && ThirdEnemyFOV.cachDistance) ||
               (SeventhEnemyFOV != null && FourthEnemyFOV.cachDistance);
    }

    public void UpdateGameState(GameState newState)
    {
        State = newState;

        switch (newState)
        {
            case GameState.CollectStealth:
                HandleCollectStealth();
                break;
            case GameState.CollectSeen:
                HandleCollectSeen();
                break;
            case GameState.Menu:
                HandleMenu();
                break;
            case GameState.Victory:
                HandleVictory();
                break;
            case GameState.Lose:
                HandleLose();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);

        }
        OnStateChange?.Invoke(newState);
    }

    private void HandleCollectStealth()
    {

    }
    private void HandleCollectSeen()
    {

    }

    private void HandleMenu()
    {

    }

    private void HandleVictory()
    {
        
    }

    private void HandleLose()
    {

    }
}

public enum GameState
{
    CollectStealth,
    CollectSeen,
    Menu,
    Victory,
    Lose,
}
