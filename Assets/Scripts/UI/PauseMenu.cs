using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject pauseMenuUI;
    public Button resumeButton;
    public Button restartButton;
    public Button quitButton;
    public Image crosshairImage;

    [Header("Player Components")]
    public GameObject player;
    private CharacterController characterController;
    private PlayerLook playerLook;
    private PlayerMovement playerMovement;

    private bool isPaused = false;

    void Start()
    {
        SetupButtons();
        CachePlayerComponents();
        SetPauseUI(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    private void SetupButtons()
    {
        resumeButton?.onClick.AddListener(Resume);
        restartButton?.onClick.AddListener(RestartLevel);
        quitButton?.onClick.AddListener(QuitGame);
    }

    private void CachePlayerComponents()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            characterController ??= player.GetComponent<CharacterController>();
            playerLook ??= player.GetComponent<PlayerLook>();
            playerMovement ??= player.GetComponent<PlayerMovement>();
        }
    }

    public void Resume()
    {
        isPaused = false;
        SetPauseState(false);
    }

    public void Pause()
    {
        isPaused = true;
        SetPauseState(true);
    }

    private void SetPauseState(bool pause)
    {
        Time.timeScale = pause ? 0f : 1f;
        SetPauseUI(pause);
        TogglePlayerControl(!pause);
        SetCursorState(!pause);
        ShowCrosshair(!pause);
    }

    private void SetPauseUI(bool show)
    {
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(show);
    }

    private void TogglePlayerControl(bool enable)
    {
        if (playerMovement != null) playerMovement.enabled = enable;
        if (playerLook != null) playerLook.enabled = enable;
    }

    private void SetCursorState(bool locked)
    {
        Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !locked;
    }

    private void ShowCrosshair(bool show)
    {
        if (crosshairImage != null)
            crosshairImage.enabled = show;
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("StartMenu");
    }
}
