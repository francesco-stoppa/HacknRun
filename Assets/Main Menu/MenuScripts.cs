using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem; // Importante per il nuovo Input System!
using TMPro;

public class MenuManager : MonoBehaviour
{
    [Header("Impostazioni Livelli")]
    [Tooltip("Nome della scena da caricare quando premi Play nel Menu Principale")]
    [SerializeField] private string gameSceneName;

    [Tooltip("Nome della scena del Menu Principale (utilizzato dalla Pausa per tornare indietro)")]
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    [Header("Pannelli UI (Menu Principale)")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject creditsPanel;

    [Header("Pannelli UI (In-Game / Pausa)")]
    [SerializeField] private GameObject pauseMenuPanel;

    [Header("UI Audio - Icone/Testo")]
    [SerializeField] private Image audioButtonIcon;
    [SerializeField] private Sprite iconAudioOn;
    [SerializeField] private Sprite iconAudioOff;
    [SerializeField] private TextMeshProUGUI audioButtonText;
    [SerializeField] private string textAudioOn = "AUDIO: ON";
    [SerializeField] private string textAudioOff = "AUDIO: OFF";

    [Header("Effetti Sonori (SFX)")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip buttonClickSFX;

    private bool isMuted = false;
    private bool isPaused = false;

    private void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        UpdateAudioUI();

        // Se ci troviamo nel Menu Principale, imposta la vista corretta
        if (mainMenuPanel != null || creditsPanel != null)
        {
            OpenMainMenu();
        }

        // Se ci troviamo in gioco (c'è il pannello pausa), assicurati che sia disattivato all'avvio
        if (pauseMenuPanel != null)
        {
            Resume();
        }
    }

    private void Update()
    {
        // Gestione Pausa tramite tasto ESC (compatibile con il Nuovo Input System)
        if (pauseMenuPanel != null && Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    // --- EFFETTI SONORI ---

    public void PlayClickSound()
    {
        if (audioSource != null && buttonClickSFX != null)
        {
            audioSource.PlayOneShot(buttonClickSFX);
        }
    }

    // --- GESTIONE CREDITS E SUB-MENU ---

    public void OpenCredits()
    {
        PlayClickSound();
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (creditsPanel != null) creditsPanel.SetActive(true);
    }

    public void OpenMainMenu()
    {
        PlayClickSound();
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
        if (creditsPanel != null) creditsPanel.SetActive(false);
    }

    // --- GESTIONE PAUSA (IN-GAME) ---

    public void Pause()
    {
        PlayClickSound();

        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(true);

        Time.timeScale = 0f; // Blocca il tempo di gioco
        isPaused = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Resume()
    {
        PlayClickSound();

        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);

        Time.timeScale = 1f; // Ripristina il tempo
        isPaused = false;
    }

    public void ReturnToMainMenu()
    {
        PlayClickSound();
        Time.timeScale = 1f; // Ripristina sempre il tempo prima di caricare una nuova scena!

        if (!string.IsNullOrEmpty(mainMenuSceneName))
        {
            SceneManager.LoadScene(mainMenuSceneName);
        }
    }

    // --- NAVIGAZIONE SCENE ---

    public void LoadNextLevel()
    {
        PlayClickSound();
        Time.timeScale = 1f;

        if (!string.IsNullOrEmpty(gameSceneName))
        {
            SceneManager.LoadScene(gameSceneName);
        }
    }

    // --- GESTIONE AUDIO ---

    public void ToggleMute()
    {
        PlayClickSound();
        isMuted = !isMuted;
        AudioListener.volume = isMuted ? 0f : 1f;
        UpdateAudioUI();
    }

    private void UpdateAudioUI()
    {
        if (audioButtonIcon != null)
            audioButtonIcon.sprite = isMuted ? iconAudioOff : iconAudioOn;

        if (audioButtonText != null)
            audioButtonText.text = isMuted ? textAudioOff : textAudioOn;
    }

    // --- QUIT ---

    public void QuitGame()
    {
        PlayClickSound();
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}