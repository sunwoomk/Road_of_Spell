using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public List<int> _maxExpList = new List<int>
    {
        100, 200, 350, 550, 800, 1100, 1450, 1850, 2300, 2800,
        3350, 3950, 4600, 5300, 6050, 6850, 7700, 8600, 9550, 10500
    };

    [SerializeField] private float _power;
    [SerializeField] private int _level = 1;
    [SerializeField] private int _curExp;
    [SerializeField] private int _maxExp;
    private int _curMana = 10;
    private int _maxMana = 10;
    [SerializeField] private int _skillLevelUpPoint = 0;

    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _curMana = _maxMana;
        _maxExp = _maxExpList[0];
    }

    public float Power
    {
        get { return _power; }
    }

    public int Mana { get { return _curMana; } }

    public void UseMana(int amount)
    {
        if (amount >= _curMana)
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
        _curExp += exp;
        if (_curExp >= _maxExp)
        {
            LevelUp();
        }
    }

    public void UseSkillLevelUpPoint()
    {
        _skillLevelUpPoint--;
        if(_skillLevelUpPoint <= 0)
        {
            InGameManager.Instance.SetActiveSkillLevelUpButtons(false);
        }
    }

    public void CastSpellAnimation()
    {
        _animator.SetTrigger("CastSpell");
    }

    private void LevelUp()
    {
        _curExp -= _maxExp;
        _level += 1;
        _maxMana += 1;
        _maxExp = _maxExpList[_level - 1];
        _skillLevelUpPoint += 1;
        InGameManager.Instance.SetActiveSkillLevelUpButtons(true);
    }
}
