using UnityEngine;
using UnityEngine.UI;

public class ExpBar : MonoBehaviour
{
    private Slider _slider;

    private Player _player;

    private void Start()
    {
        _slider = GetComponent<Slider>();
    }

    private void Update()
    {
        if (_player != null)
        {
            _slider.value = (float)_player.CurExp / (float)_player.MaxExp;
        }
    }

    public void SetPlayer(Player player)
    {
        _player = player;
    }
}
