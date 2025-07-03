using UnityEngine;

public class PauseState : IGameState
{
    public void Enter()
    {
        Time.timeScale = 0f;    // Freeze the game
        UIManager.Instance.SetPause(true);  // Show the pause menu UI
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Switch back to gameplay state
            GameManager.Instance.ChangeState(new GameplayState());
        }
    }

    public void Exit()
    {
        Time.timeScale = 1f;    // Resume the game
        UIManager.Instance.SetPause(false);
    }
}
