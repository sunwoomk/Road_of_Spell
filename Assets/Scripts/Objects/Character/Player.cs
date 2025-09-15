using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private List<int> _maxExpList = new List<int>
    {
        100, 200, 350, 550, 800, 1100, 1450, 1850, 2300, 2800,
        3350, 3950, 4600, 5300, 6050, 6850, 7700, 8600, 9550, 10500
    };

    private int _maxLevel = 20;

    private float _power;
    private int _level = 1;
    private int _curExp;
    private int _maxExp;
    private int _curHp;
    private int _maxHp = 5;

    private int _curMana = 1;
    private int _maxMana = 1;
    private int _skillLevelUpPoint = 0;

    private Animator _animator;

    private ExpBar _expBar;

    public float Power
    {
        get { return _power; }
    }

    public int CurMana { get { return _curMana; } }
    public int MaxMana { get { return _maxMana; } }
    public float CurExp { get { return _curExp; } }
    public float MaxExp { get { return _maxExp; } }
    public float CurHp { get { return _curHp; } }
    public float MaxHp { get { return _maxHp; } }
    public int Level { get { return _level; } }

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _curMana = _maxMana;
        _curHp = _maxHp;
        _maxExp = _maxExpList[0];
    }

    private void Update()
    {
        if (_skillLevelUpPoint >= 1)
        {
            InGameManager.Instance.SetActiveSkillLevelUpButtons(true);
        }
        else
        {
            InGameManager.Instance.SetActiveSkillLevelUpButtons(false);
        }
    }

    public void SetExpBar(ExpBar expBar)
    {
        _expBar = expBar;
    }

    public void UseMana(int amount)
    {
        if (amount <= _curMana)
        {
            _curMana -= amount;
        }
    }

    public void RefillMana()
    {
        _curMana = _maxMana;
    }

    public void AddExp(int exp)
    {
        if (_level >= _maxLevel)
        {
            _curExp = 0;
            return;
        }
        _curExp += exp * 10;
        _expBar.StartMoveExpBar();

        while (_curExp >= _maxExp && _level < _maxLevel)
        {
            LevelUp();
        }
    }

    public void TakeDamage(int power)
    {
        _curHp -= power;
    }

    public void UseSkillLevelUpPoint()
    {
        _skillLevelUpPoint--;
    }

    public void CastSpellAnimation()
    {
        _animator.SetTrigger("CastSpell");
    }

    public void LevelUp()
    {
        if (_level >= _maxLevel) return;
        _curExp -= _maxExp;
        _level += 1;
        _maxMana += 1;
        _maxExp = _maxExpList[_level - 1];
        _skillLevelUpPoint += 1;
        Debug.Log("LevelUp! 현재 레벨 : " + _level);
    }
}
