using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Slider HealthbarSlider;
    [SerializeField] Image HealthBarFill;
    [SerializeField] Gradient HealthBarGradient;
    [SerializeField] TextMeshProUGUI HealthBarValue;

    private void Start()
    {
        HealthBarValue.text = GameManager.Instance.BaseHealth.ToString();
        HealthBarFill.color = HealthBarGradient.Evaluate(1f);
        HealthbarSlider.maxValue = GameManager.Instance.BaseMaxHealth;
        HealthbarSlider.value = GameManager.Instance.BaseHealth;
    }

    private void Update()
    {
        HealthbarSlider.value = GameManager.Instance.BaseHealth;
        HealthBarFill.color = HealthBarGradient.Evaluate(HealthbarSlider.normalizedValue);
        HealthBarValue.text = GameManager.Instance.BaseHealth.ToString();
    }
}
