using UnityEngine;
using UnityEngine.SceneManagement;
public interface IGameState
{
    void Enter();
    void Update();
    void Exit();
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // The game state machine managing different IGameState instances
    private GameStateMachine stateMachine;
    private void Awake()
    {
        // Ensure there's only one instance of GameManager (Singleton pattern)
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Initialize the state machine and set the initial state
        stateMachine = new GameStateMachine();
        int currentBuildIndex = SceneManager.GetActiveScene().buildIndex; // Store the current build index

        switch (currentBuildIndex)  //
        {
            case 0: // Main Menu
                stateMachine.ChangeState(new MainMenuState());
                break;
            case 1: // Game
                stateMachine.ChangeState(new GameplayState());
                break;
        }
    }

    private void Update()
    {
        stateMachine?.Update();
    }

    public void ChangeState(IGameState newState)    // Public method to change the current game state.
    {
        stateMachine.ChangeState(newState);
        Debug.Log(stateMachine.CurrentState);
    }
}

