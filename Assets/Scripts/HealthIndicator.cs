using UnityEngine;

internal sealed class HealthIndicator : MonoBehaviour
{
    TextMesh textMesh;
    Health health;
    float displayedHealth;

    void Start()
    {
        textMesh = GetComponent<TextMesh>();
        health = GetComponentInParent<Health>();
        displayedHealth = health.current - 1.0f;
    }

    void Update()
    {
        float value = health.current;
        if (!Mathf.Approximately(displayedHealth, value)) { // !=
            displayedHealth = value;
            textMesh.text = $"{value}";
        }
    }
}
