using UnityEngine;

public class GameplayState : IGameState
{

    public void Enter()
    {
        // Called when entering the gameplay state
        // You can add logic here to initialize gameplay systems

        if (!SoundManager.Instance.IsPlaying(SoundManager.Instance.levelMusic))
        {
            SoundManager.Instance.PlayMusic(SoundManager.Instance.levelMusic, true);
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Switch to the pause state
            GameManager.Instance.ChangeState(new PauseState());
        }
    }

    public void Exit()
    {
        // Called when exiting gameplay state
        // Clean up or save data if necessary
    }
}
