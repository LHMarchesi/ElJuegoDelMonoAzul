using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    [SerializeField] private GameObject pauseObject;  // Reference to the RawImage UI element representing the pause menu
    [SerializeField] private GameObject gameOverObject;  // Reference to the RawImage UI element representing the pause menu
    [SerializeField] private GameObject winObject;  // Reference to the RawImage UI element representing the pause menu

    private void Awake()
    {
        // Ensure only one instance of PauseMenu exists
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        SetPause(false);     // Make sure the menu's are hidden on start
        SetGameOver(false);  
        SetWin(false);  
    }

    public void SetPause(bool value)   // Enables or disables the pause menu UI.
    {
        pauseObject.SetActive(value);
    }

    public void SetGameOver(bool value)   // Enables or disables the pause menu UI.
    {
        gameOverObject.SetActive(value);
    }
    
    public void SetWin(bool value)   // Enables or disables the pause menu UI.
    {
        winObject.SetActive(value);
    }
}
