public class MainMenuState : IGameState
{
    public void Enter() { SoundManager.Instance.PlayMusic(SoundManager.Instance.mainMenuMusic, true); }

    public void Update() { }

    public void Exit() { SoundManager.Instance.StopMusic(); }
}
