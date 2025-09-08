using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExpBar : MonoBehaviour
{
    private const float _lerpSpeed = 2f;
    private Slider _slider;
    private Player _player;
    private TextMeshProUGUI _levelText;
    private Coroutine _moveCoroutine;

    private void Start()
    {
        _slider = GetComponent<Slider>();
        _levelText = transform.Find("LevelText").GetComponent<TextMeshProUGUI>();
    }

    //����ġ�� ���� ������ Player���� �ش� �Լ� ȣ��
    public void StartMoveExpBar()
    {
        if (_moveCoroutine != null)
            StopCoroutine(_moveCoroutine);
        _moveCoroutine = StartCoroutine(MoveExpBarCoroutine());
    }

    private IEnumerator MoveExpBarCoroutine()
    {
        float startValue = _slider.value;

        //�������� �����ϴٸ� ���� �ݺ�
        while (_player.CurExp >= _player.MaxExp)
        {
            _player.LevelUp();
            yield return AnimateLerp(startValue, 1f);
            startValue = 0f;
            _slider.value = 0f;
        }

        //�������� ��ģ �Ŀ� ��ǥ value�� ��� ������ �������� �̵�
        float targetValue = _player.CurExp / _player.MaxExp;
        yield return AnimateLerp(startValue, targetValue);
        _levelText.text = _player.Level.ToString();
    }

    private IEnumerator AnimateLerp(float from, float to)
    {
        float elapsed = 0f;
        float duration = 1f / _lerpSpeed;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            _slider.value = Mathf.Lerp(from, to, t);
            yield return null;
        }
        _slider.value = to;
    }

    public void SetPlayer(Player player)
    {
        _player = player;
        _player.SetExpBar(this);
    }
}

