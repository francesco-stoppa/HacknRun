using UnityEngine;
public class CharacterTimer : MonoBehaviour
{
    [Header("Timer")]
    [SerializeField] private float duration = 10f;
    [SerializeField] private string text = "Timer";
    [Header("Audio & Game Over")]
    [SerializeField] private AudioClip explosionSFX;
    [SerializeField] private GameObject deathScreenPanel;
    float timer;
    bool reduceTImer = false;
    bool isGameOver = false;
    public void StartTimer()
    {
        reduceTImer = true;
        timer = duration;
    }
    private void Start()
    {
        // Blocca il cursore al centro dello schermo e lo nasconde
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void OnApplicationFocus(bool hasFocus)
    {
        // Se il gioco riprende il focus (es. dopo Alt+Tab) riblocca il cursore,
        // ma solo se non siamo nella death screen (altrimenti serve poterci cliccare)
        if (hasFocus && !isGameOver)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
    [SerializeField] private MenuManager menuManager; // trascinalo nell'Inspector

    private void Update()
    {
        if (!reduceTImer) return;

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            if (menuManager != null)
            Debug.Log(Cursor.lockState);
            Debug.Log(Cursor.visible);
            menuManager.ShowDeathScreen();
            
            Destroy(gameObject);
        
        }
    }
    private void OnGUI()
    {
        if (!reduceTImer) return;
        // Se il tempo è fermo (es. Pausa o Death Screen), non disegnare la Label OnGUI per non bloccare la UI
        if (Time.timeScale == 0f) return;
        GUIStyle style = new GUIStyle(GUI.skin.label);
        style.fontSize = 30;
        style.normal.textColor = Color.white;
        GUI.Label(
            new Rect(10, 10, 300, 40),
            $"{text}: {Mathf.Max(timer, 0f):F1}",
            style
        );
    }
}