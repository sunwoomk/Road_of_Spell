using UnityEngine;
using UnityEngine.UI;

public class ExpBar : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private Material _material;
    [SerializeField] private float _cycleDuration = 2.0f; // 1ȸ �պ� �ð�(��)

    void Start()
    {
        _slider.onValueChanged.AddListener(OnSliderValueChanged);
        OnSliderValueChanged(_slider.value);
    }

    void Update()
    {
        // �ð��� �带���� 0~1�� �ö󰬴ٰ� �ٽ� 0���� ���������� �����̴� �� ����
        float t = Mathf.PingPong(Time.time / _cycleDuration, 1f);
        _slider.value = t;
    }

    void OnSliderValueChanged(float value)
    {
        _material.SetFloat("_FillAmount", value);
    }
}
