using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Unity.VisualScripting;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.Networking;

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

    public delegate void StageDataLoadedHandler();
    public event StageDataLoadedHandler OnStageDataLoaded;

    public Stage CurrentStage { get { return _currentStage; } }

    protected override void Awake()
    {
        base.Awake();
        _currentRoundCount++;

    #if UNITY_EDITOR || UNITY_STANDALONE
        LoadStageJson();
    #elif UNITY_ANDROID
        StartCoroutine(LoadStageJsonFromMobile());
    #else
        Debug.LogWarning("Unsupported platform for loading stage JSON.");
    #endif
    }

    private void Start()
    {
        _canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        _endTurnButton = _canvas.transform.Find("EndTurnButton").GetComponent<Button>();
        _endTurnButton.onClick.AddListener(TurnEnd);
        _currentRoundText = _canvas.transform.Find("CurrentRound").transform.Find("CurrentRoundText").GetComponent<TextMeshProUGUI>();
        _currentRoundText.text = _currentRoundCount.ToString();
        //InGameManager.Instance.MonsterCount = _currentStage.monsterSpawns.Count;
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
            OnStageDataLoaded?.Invoke();
            Debug.Log("Stage data loaded successfully.");
        }
        else
        {
            Debug.LogWarning("Stage JSON file not found at path: " + filePath);
        }
    }

    // ����� ȯ�濡�� StreamingAssets�� �������� JSON ������ �񵿱� �ε��Ͽ� �Ľ��ϴ� �ڷ�ƾ
    private IEnumerator LoadStageJsonFromMobile()
    {
        // �������� �̸� ��� JSON ���� ��� ����
        string fileName = GameDataManager.Instance.StageName + ".json";
        string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, fileName);

        // UnityWebRequest�� ���� ��û �� �Ϸ� ���
        UnityWebRequest www = UnityWebRequest.Get(filePath);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            // JSON �Ľ� �� �������� ������ ���� �� �̺�Ʈ ȣ��
            string json = www.downloadHandler.text;
            _currentStage = JsonUtility.FromJson<Stage>(json);
            //InGameManager.Instance.MonsterCount = _currentStage.monsterSpawns.Count;
            OnStageDataLoaded?.Invoke();
        }
    }
}
