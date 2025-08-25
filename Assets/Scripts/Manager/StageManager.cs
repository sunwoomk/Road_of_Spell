using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class StageManager : MonoBehaviour
{
    [System.Serializable]
    public struct MonsterSpawnData
    {
        public int x;
        public int y;
        public int key;
    }
    [System.Serializable]
    public class Stage
    {
        public int roundCount;
        public List<MonsterSpawnData> monsterSpawns;
    }

    [SerializeField]
    private Stage _testStage;

    private int _currentRound = 0;

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

        TestJsonFileSave();
    }

    private void TestJsonFileSave()
    {
        _testStage = new Stage();
        _testStage.roundCount = 2;
        _testStage.monsterSpawns = new List<MonsterSpawnData>
        {
            new MonsterSpawnData { x = 0, y = 1 , key = 1},
            new MonsterSpawnData { x = 0, y = 3 , key = 2},
            new MonsterSpawnData { x = 1, y = 2 , key = 1},
            new MonsterSpawnData { x = 0, y = 4 , key = 3}
        };

        string json = JsonUtility.ToJson(_testStage, true);

        File.WriteAllText(Application.dataPath + "/Resources/Stages/TestStage.json", json);
    }
}
