using NUnit.Framework;
using UnityEngine;

[CreateAssetMenu(fileName = "Spell", menuName = "Scriptable Objects/Spell")]
public class Spell : ScriptableObject
{
    public int tempNum;
    public string element;
    public int tier;
    public float baseDamage;
    public float damagePerLevel;
    public float damageRatio;
    public int cost;
    public Vector2Int[] range;
}
