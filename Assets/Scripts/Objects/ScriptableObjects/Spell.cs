using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Spell", menuName = "Scriptable Objects/Spell")]
public class Spell : ScriptableObject
{
    public string element;
    public int tier;
    public float baseDamage;
    public float damagePerLevel;
    public float damageRatio;
    public int cost;
    public Vector2Int[] range;
    public string effectType;
    public List<Spell> nextSpells = new List<Spell>();
}
