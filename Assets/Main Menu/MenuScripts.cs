using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuManager : MonoBehaviour
{
    [Header("Impostazioni Livello")]
    [Tooltip("Scrivi il nome esatto della scena di gioco")]
    [SerializeField] private string sceneToLoad;

    [Header("Pannelli UI")]
    [Tooltip("Trascina qui l'oggetto HackerBackground (che contiene i bottoni del menu)")]
    [SerializeField] private GameObject mainMenuPanel;

    [Tooltip("Trascina qui l'oggetto Credits Panel")]
    [SerializeField] private GameObject creditsPanel;

    [Header("UI Audio - Icona (Opzionale)")]
    [SerializeField] private Image audioButtonIcon;
    [SerializeField] private Sprite iconAudioOn;
    [SerializeField] private Sprite iconAudioOff;

    [Header("UI Audio - Testo (Opzionale)")]
    [SerializeField] private TextMeshProUGUI audioButtonText;
    [SerializeField] private string textAudioOn = "AUDIO: ON";
    [SerializeField] private string textAudioOff = "AUDIO: OFF";

    private bool isMuted = false;

    private void Start()
    {
        // Aggiorna la grafica dell'audio all'avvio
        UpdateAudioUI();

        // Di default mostra il Menu Principale e nasconde i Credits
        OpenMainMenu();
    }

    // --- GESTIONE CREDITS ---

    // Chiamata dal bottone "Credits"
    public void OpenCredits()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (creditsPanel != null) creditsPanel.SetActive(true);
    }

    // Chiamata dal bottone "Back / Indietro" dentro i Credits
    public void OpenMainMenu()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
        if (creditsPanel != null) creditsPanel.SetActive(false);
    }

    // --- GESTIONE CAMBIO SCENA ---

    public void LoadNextLevel()
    {
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.LogWarning("Nessun nome di scena impostato su " + gameObject.name);
        }
    }

    // --- GESTIONE AUDIO ---

    public void ToggleMute()
    {
        isMuted = !isMuted;
        AudioListener.volume = isMuted ? 0f : 1f;
        UpdateAudioUI();
    }

    private void UpdateAudioUI()
    {
        if (audioButtonIcon != null)
        {
            audioButtonIcon.sprite = isMuted ? iconAudioOff : iconAudioOn;
        }

        if (audioButtonText != null)
        {
            audioButtonText.text = isMuted ? textAudioOff : textAudioOn;
        }
    }

    // --- GESTIONE QUIT ---

    public void QuitGame()
    {
        Debug.Log("Chiusura gioco...");
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}