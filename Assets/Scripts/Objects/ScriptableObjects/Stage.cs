using UnityEngine;

[CreateAssetMenu(fileName = "Stage", menuName = "Scriptable Objects/Stage")]
public class Stage : ScriptableObject
{
    [System.Serializable]
    public class Round
    {
        public int minSpawn;
        public int maxSpawn;
        public float[] spawnProbabilities;
        public float[] monsterProbabilities = new float[5];
    }

    public Round[] rounds;
}
