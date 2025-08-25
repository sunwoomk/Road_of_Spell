using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Unity.VisualScripting;

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
    [SerializeField]
    private Canvas _uiCanvas;

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

        //TestJsonFileSave();
        LoadStageJson();
    }

    private void Start()
    {
        _currentRound++;
        SpawnMonsters();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            TurnEnd();
        }
    }

    private void TurnEnd()
    {
        _currentRound++;
        MonsterManager.Instance.AllMonstersMove();
        SpawnMonsters();
    }

    private void SpawnMonsters()
    {
        List<MonsterSpawnData> spawnsThisRound = _testStage.monsterSpawns.FindAll(m => m.x == _currentRound - 1);
        Vector2 startTilePos = new Vector2(TileManager.StartTilePos.x + TileManager.TileSize * (TileManager.TileWidth - 1), TileManager.StartTilePos.y);
        foreach (var spawn in spawnsThisRound)
        {
            Vector2 worldPos = new Vector2(startTilePos.x, startTilePos.y - (spawn.y * TileManager.TileSize));
            Vector2Int tilePos = new Vector2Int(TileManager.TileWidth - 1, spawn.y);

            MonsterManager.Instance.CreateMonster(spawn.key, worldPos, tilePos);
            Debug.Log("Spawn!");
        }
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
            new MonsterSpawnData { x = 1, y = 4 , key = 3}
        };

        string json = JsonUtility.ToJson(_testStage, true);

        File.WriteAllText(Application.dataPath + "/Resources/Stages/TestStage2.json", json);
    }

    private void LoadStageJson()
    {
        string filePath = Application.dataPath + "/Resources/Stages/TestStage2.json";

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            _testStage = JsonUtility.FromJson<Stage>(json);
            Debug.Log("Stage data loaded successfully.");
        }
        else
        {
            Debug.LogWarning("Stage JSON file not found at path: " + filePath);
        }
    }
}
