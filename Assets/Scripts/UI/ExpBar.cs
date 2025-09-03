using UnityEngine;
using UnityEngine.UI;

public class ExpBar : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private Material _material;
    [SerializeField] private float _cycleDuration = 2.0f; // 1회 왕복 시간(초)

    void Start()
    {
        _slider.onValueChanged.AddListener(OnSliderValueChanged);
        OnSliderValueChanged(_slider.value);
    }

    void Update()
    {
        // 시간이 흐를수록 0~1로 올라갔다가 다시 0으로 내려가도록 슬라이더 값 갱신
        float t = Mathf.PingPong(Time.time / _cycleDuration, 1f);
        _slider.value = t;
    }

    void OnSliderValueChanged(float value)
    {
        _material.SetFloat("_FillAmount", value);
    }
}
