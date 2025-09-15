using System.Threading;
using TMPro;
using UnityEngine;
using System.Collections;

public class Monster : MonoBehaviour
{
    private const float DeadDuration = 1f;
    private const float MoveDuration = 1f;

    private int _level;
    private float _maxHp;
    private float _currentHp;
    private float _defense;
    private int _power;
    private int _speed;
    private int _dropExp;
    private Vector2Int _position;

    private Player _player;
    private Animator _animator;

    public Vector2Int Position
    {
        get { return _position; }
        set { _position = value; }
    }

    public int Speed => _speed;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _level = 1;
    }

    public void SetDatas(MonsterManager.MonsterData monsterData)
    {
        _maxHp = monsterData.Hp;
        _currentHp = _maxHp;
        _defense = monsterData.BaseDefense;
        _speed = monsterData.Speed;
        _dropExp = monsterData.DropExp;
        _power = monsterData.Power;
    }

    public void SetPlayer(Player player)
    {
        _player = player;
    }

    public void MonsterMove(int moveDistance)
    {
        RectTransform rect = gameObject.GetComponent<RectTransform>();
        Vector2 currentPos = rect.anchoredPosition;

        // 월드 단위 이동 거리 예: 왼쪽으로 _speed * TileSize 만큼 이동
        Vector2 worldDistance = new Vector2(moveDistance * TileManager.TileSize, 0);

        // 월드 거리 → 캔버스 거리 변환
        Vector2 canvasDistance = WorldDistanceToCanvasDistance(worldDistance);

        Vector2 targetPos = currentPos + canvasDistance;
        _animator.SetBool("IsMove", true);
        StartCoroutine(MoveRectTransform(rect, targetPos, MoveDuration));

        //포지션값 수정
        _position += new Vector2Int(moveDistance, 0);

        //타일 마지막 칸에 도달하면 플레이어에게 데미지
        if(_position.x <= 0)
        {
            _player.TakeDamage(_power);
            Dead();

            //플레이어의 체력이 0 아래로 떨어지면 GameOver
            if(_player.CurHp <= 0)
            {
                InGameManager.Instance.SetGameResultPanelActiveTrue("GameOver");
            }
        }
    }

    public void TakeDamage(float damage)
    {
        _currentHp -= damage;
        if(_currentHp <= 0)
        {
            Dead();
        }
    }

    public void Dead()
    {
        _animator.SetTrigger("Dead");
        _player.AddExp(_dropExp);
        StartCoroutine(WaitAndDestroy());

        //죽을때마다 KillCount 증가
        InGameManager.Instance.KillCount += 1;

        //KillCount가 MonsterCount를 넘으면 GameClear
        if(InGameManager.Instance.KillCount >= InGameManager.Instance.MonsterCount)
        {
            InGameManager.Instance.SetGameResultPanelActiveTrue("GameClear");
        }
    }

    private IEnumerator WaitAndDestroy()
    {
        yield return new WaitForSeconds(DeadDuration);
        MonsterManager.Instance.RemoveMonster(gameObject);
        Destroy(gameObject);
    }

    //선형보간을 활용하여 부드러운 움직임 구현
    private IEnumerator MoveRectTransform(RectTransform rect, Vector2 targetPos, float duration)
    {
        Vector2 startPos = rect.anchoredPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            rect.anchoredPosition = Vector2.Lerp(startPos, targetPos, elapsed / duration);
            yield return null;
        }

        _animator.SetBool("IsMove", false);
        rect.anchoredPosition = targetPos;
    }

    private Vector2 WorldDistanceToCanvasDistance(Vector2 worldDistance)
    {
        Vector3 worldPos = Vector3.zero;
        Vector3 worldPosWithDistance = new Vector3(worldDistance.x, worldDistance.y, 0);

        Vector2 canvasPos = TileManager.Instance.WorldToCanvasPosition(worldPos);
        Vector2 canvasPosWithDistance = TileManager.Instance.WorldToCanvasPosition(worldPosWithDistance);

        return canvasPosWithDistance - canvasPos;
    }
}
