using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _power;

    [SerializeField]
    private int _level;

    private int _exp;
    private int _maxExp;
    private int _mana;

    public float Power
    {
        get { return _power; }
    }
    private void Update()
    {
        if (_exp >= _maxExp)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        _exp -= _maxExp;
        _level += 1;
    }
}
