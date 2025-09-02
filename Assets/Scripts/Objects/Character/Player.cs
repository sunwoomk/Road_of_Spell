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

    [SerializeField] private float _power;
    [SerializeField] private int _level = 1;
    [SerializeField] private int _curExp;
    [SerializeField] private int _maxExp;
    private int _curMana = 1;
    private int _maxMana = 1;
    [SerializeField] private int _skillLevelUpPoint = 0;

    private Animator _animator;

    public float Power
    {
        get { return _power; }
    }

    public int CurMana { get { return _curMana; } }
    public int MaxMana { get { return _maxMana; } }

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _curMana = _maxMana;
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
        _curExp += exp * 10;
        while (_curExp >= _maxExp)
        {
            LevelUp();
        }
    }

    public void UseSkillLevelUpPoint()
    {
        _skillLevelUpPoint--;
        //if(_skillLevelUpPoint <= 0)
        //{
        //    InGameManager.Instance.SetActiveSkillLevelUpButtons(false);
        //}
    }

    public void CastSpellAnimation()
    {
        _animator.SetTrigger("CastSpell");
    }

    private void LevelUp()
    {
        _curExp -= _maxExp;
        if (_level >= _maxLevel)
        {
            _curExp = 0;
            return;
        }
        _level += 1;
        _maxMana += 1;
        _maxExp = _maxExpList[_level - 1];
        _skillLevelUpPoint += 1;
        Debug.Log("LevelUp! 현재 레벨 : " +  _level);
        //InGameManager.Instance.SetActiveSkillLevelUpButtons(true);
    }
}
