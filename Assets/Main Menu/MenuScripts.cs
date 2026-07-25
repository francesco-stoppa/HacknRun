using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem; // Nuovo Input System
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

    [Header("UI Audio - Switch Sprite Icona")]
    [Tooltip("L'Image del pulsante audio che deve cambiare sprite")]
    [SerializeField] private Image audioButtonImage;

    [Tooltip("Sprite per l'icona dell'audio ATTIVO")]
    [SerializeField] private Sprite iconAudioOn;

    [Tooltip("Sprite per l'icona dell'audio DISATTIVATO (Mute)")]
    [SerializeField] private Sprite iconAudioOff;

    [Header("UI Audio - Toggle Testi/Grafiche ON/OFF")]
    [Tooltip("GameObject visibile quando l'audio è ATTIVO (es. testo/grafica ON)")]
    [SerializeField] private GameObject audioOnObject;

    [Tooltip("GameObject visibile quando l'audio è DISATTIVATO (es. testo/grafica OFF)")]
    [SerializeField] private GameObject audioOffObject;

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

        // Inizializza sia la sprite sia la grafica ON/OFF all'avvio
        UpdateAudioUI();

        // Se ci troviamo nel Menu Principale, imposta la vista corretta
        if (mainMenuPanel != null || creditsPanel != null)
        {
            OpenMainMenu();
        }

        // Se ci troviamo in gioco, assicuratevi che il menu di pausa sia nascosto all'avvio
        if (pauseMenuPanel != null)
        {
            Resume();
        }
    }

    private void Update()
    {
        // Gestione Pausa tramite tasto ESC (Nuovo Input System)
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
        if (!isMuted && audioSource != null && buttonClickSFX != null)
        {
            audioSource.PlayOneShot(buttonClickSFX);
        }
    }

    // --- GESTIONE MENU PRINCIPALE E CREDITI ---

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
        Time.timeScale = 1f; // Ripristina il tempo prima di cambiare scena

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

    // --- GESTIONE AUDIO (TOGGLE COMPLETO) ---

    public void ToggleMute()
    {
        // Riproduce il suono del click prima di mutare
        PlayClickSound();

        // Inverte lo stato mutato
        isMuted = !isMuted;

        // Gestisce il volume globale di Unity
        AudioListener.volume = isMuted ? 0f : 1f;

        // Aggiorna sia le sprite che i GameObject ON/OFF
        UpdateAudioUI();
    }

    private void UpdateAudioUI()
    {
        // 1. SWITCH SPRITE DELL'ICONA
        if (audioButtonImage != null)
        {
            audioButtonImage.sprite = isMuted ? iconAudioOff : iconAudioOn;
        }

        // 2. SWITCH DEI GAMEOBJECT (TESTI/GRAFICHE ON e OFF)
        if (audioOnObject != null) audioOnObject.SetActive(!isMuted);
        if (audioOffObject != null) audioOffObject.SetActive(isMuted);
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