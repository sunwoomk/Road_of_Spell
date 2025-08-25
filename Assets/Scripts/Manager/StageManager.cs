using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public class Stage
    {
        public int roundCount;
        public Dictionary<Vector2Int, int> monsterSpawns;
    }

    public static StageManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


}
