using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GradientBar : MonoBehaviour
{
    BossDryad bossData;

    Slider slider;
    public Gradient gradient;
    public Image fill;

    [SerializeField] TextMeshProUGUI CurHP_Text;
    [SerializeField] TextMeshProUGUI MaxHP_Text;
    
    private void Awake()
    {
        bossData = FindObjectOfType<BossDryad>();
        slider = GetComponent<Slider>();
        SetMaxHP(bossData.GetMaxHP());
    }

    void Update()
    {
        SetHP(bossData.GetHP());
    }

    public void SetMaxHP(float health)
    {
        slider.maxValue = health;
        slider.value = health;
        MaxHP_Text.text = "" + health;
        CurHP_Text.text = "" + health;

        gradient.Evaluate(1f);
    }

    public void SetHP(float health)
    {
        slider.value = health;
        fill.color = gradient.Evaluate(slider.normalizedValue);
        CurHP_Text.text = "" + health;
    }
}
