using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExpBar : MonoBehaviour
{
    private Slider _slider;
    private Player _player;
    private TextMeshProUGUI _levelText;

    private void Start()
    {
        _slider = GetComponent<Slider>();
        _levelText = transform.Find("LevelText").GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (_player != null)
        {
            _slider.value = (float)_player.CurExp / (float)_player.MaxExp;
            _levelText.text = _player.Level.ToString();
        }
    }

    public void SetPlayer(Player player)
    {
        _player = player;
    }
}
