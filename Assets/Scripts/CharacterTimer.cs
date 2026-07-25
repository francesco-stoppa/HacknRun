using UnityEngine;

public class CharacterTimer : MonoBehaviour
{
    [Header("Timer")]
    [SerializeField] private float duration = 10f;
    [SerializeField] private string text = "Timer";

    float timer;
    bool reduceTImer = false;

    public void StartTimer()
    {
        reduceTImer = true;
        timer = duration;
    }

    private void Update()
    {
        if (!reduceTImer) return;

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            Destroy(gameObject);
        }
    }

    private void OnGUI()
    {
        if (!reduceTImer) return;

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

