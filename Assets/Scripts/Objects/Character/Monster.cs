using UnityEngine;

public class Monster : MonoBehaviour
{
    private int _level;
    private int _maxHp;
    private int _currentHp;
    private int _defense;
    private Vector2Int _position;

    private Animator _animator;

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
    }
}
