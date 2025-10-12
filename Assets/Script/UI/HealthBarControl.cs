using UnityEngine;
using UnityEngine.UI;

public class HealthBarControl : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    public Gradient colGradient;
    private Image fillImage;

    private void Awake()
    {
        fillImage = healthSlider.fillRect.GetComponent<Image>();
    }
    public void SetSliderValue(float vcurenthealth, float maxHealth)
    {
        healthSlider.maxValue = maxHealth; 
        healthSlider.value = vcurenthealth;
        fillImage.color = colGradient.Evaluate(healthSlider.normalizedValue);
    }
}
