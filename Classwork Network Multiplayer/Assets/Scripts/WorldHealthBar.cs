using UnityEngine;
using UnityEngine.UI;

public class WorldHealthBar : MonoBehaviour
{
    [SerializeField] private Image fill;
    [SerializeField] private NetworkHealth health;

    private void OnEnable()
    {
        if (fill != null)
        {
            health.Health.OnValueChanged += OnHealthChanged;
        }
    }

    private void OnDisable()
    {
        if (fill != null)
        {
            health.Health.OnValueChanged -= OnHealthChanged;
        }
    }

    private void OnHealthChanged(int oldValue, int newValue)
    {
        UpdateFill();
    }

    private void UpdateFill()
    {
        if (!fill || health == null) return;
        fill.fillAmount = health.Health01;
    }
}
