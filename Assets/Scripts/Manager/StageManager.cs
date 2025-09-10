using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Unity.VisualScripting;
using UnityEngine.UI;
using TMPro;

public class StageManager : Singleton<StageManager>
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

    private Stage _currentStage;
    private Button _endTurnButton;
    private TextMeshProUGUI _currentRoundText;
    private Player _player;
    private Canvas _canvas;

    private int _currentRoundCount = 0;
    //private int _currentStageCount = 0;

    protected override void Awake()
    {
        base.Awake();

        _currentRoundCount++;
        //_currentStageCount++;

        LoadStageJson();
    }

    private void Start()
    {
        _canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        _endTurnButton = _canvas.transform.Find("EndTurnButton").GetComponent<Button>();
        _endTurnButton.onClick.AddListener(TurnEnd);
        _currentRoundText = _canvas.transform.Find("CurrentRound").transform.Find("CurrentRoundText").GetComponent<TextMeshProUGUI>();
        _currentRoundText.text = _currentRoundCount.ToString();
    }

    public void SetPlayer(Player player)
    {
        _player = player;
    }

    private void TurnEnd()
    {
        _currentRoundCount++;
        _currentRoundText.text = _currentRoundCount.ToString();
        MonsterManager.Instance.AllMonstersMove();
        SpawnMonsters();
        _player.RefillMana();
    }

    public void SpawnMonsters()  // ���� ���忡 �ش��ϴ� ���͵��� �����ϴ� �Լ�
    {
        // x ���� ���� ����� ���� ��Ҹ� ����
        List<MonsterSpawnData> spawnsThisRound = _currentStage.monsterSpawns.FindAll(m => m.x == _currentRoundCount - 1);

        Vector2 startTilePos = new Vector2(
            TileManager.StartTilePos.x + TileManager.TileSize * (TileManager.TileWidth - 1),
            TileManager.StartTilePos.y
        );

        foreach (MonsterSpawnData spawn in spawnsThisRound)
        {
            // ���� ��ǥ ���ϱ�
            Vector2 worldPos = new Vector2(
                startTilePos.x,
                startTilePos.y - (spawn.y * TileManager.TileSize)
            );

            // �ش� ���Ͱ� ��ġ�� Ÿ�� ��ǥ (Ÿ�� �� ���� ��ǥ)
            Vector2Int tilePos = new Vector2Int(TileManager.TileWidth - 1, spawn.y);

            // (spawn.key: ���� ���� Ű, worldPos: ���� ��ġ, tilePos: Ÿ�� ��ǥ)
            MonsterManager.Instance.CreateMonster(spawn.key, worldPos, tilePos);
        }
    }

    private void LoadStageJson()
    {
        //string filePath = Application.dataPath + "/Resources/Stages/Stage" + _currentStageCount + ".json";
        string filePath = Application.dataPath + "/Resources/Stages/" + GameDataManager.Instance.StageName + ".json";

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            _currentStage = JsonUtility.FromJson<Stage>(json);
            Debug.Log("Stage data loaded successfully.");
        }
        else
        {
            Debug.LogWarning("Stage JSON file not found at path: " + filePath);
        }
    }
}
