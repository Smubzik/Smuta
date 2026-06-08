using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthSlider;
    public Train train;
    
    private void Start()
    {
        if (train == null)
            train = FindObjectOfType<Train>();
        
        if (healthSlider == null)
            healthSlider = GetComponent<Slider>();
        
        Refresh();
    }
    
    private void Update()
    {
        Refresh();
    }
    
    public void Refresh()
    {
        if (train != null && healthSlider != null)
        {
            healthSlider.maxValue = train.MaxHealth;
            healthSlider.value = train.CurrentHealth;
        }
    }
    
    public void Hide()
    {
        gameObject.SetActive(false);
    }
    
    public void Show()
    {
        gameObject.SetActive(true);
    }
}