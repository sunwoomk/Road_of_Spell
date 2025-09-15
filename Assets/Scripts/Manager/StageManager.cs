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

    public void SpawnMonsters()  // 현재 라운드에 해당하는 몬스터들을 스폰하는 함수
    {
        // x 값이 현재 라운드와 같은 요소만 추출
        List<MonsterSpawnData> spawnsThisRound = _currentStage.monsterSpawns.FindAll(m => m.x == _currentRoundCount - 1);

        Vector2 startTilePos = new Vector2(
            TileManager.StartTilePos.x + TileManager.TileSize * (TileManager.TileWidth - 1),
            TileManager.StartTilePos.y
        );

        foreach (MonsterSpawnData spawn in spawnsThisRound)
        {
            // 월드 좌표 구하기
            Vector2 worldPos = new Vector2(
                startTilePos.x,
                startTilePos.y - (spawn.y * TileManager.TileSize)
            );

            // 해당 몬스터가 배치될 타일 좌표 (타일 맵 기준 좌표)
            Vector2Int tilePos = new Vector2Int(TileManager.TileWidth - 1, spawn.y);

            // (spawn.key: 몬스터 종류 키, worldPos: 실제 위치, tilePos: 타일 좌표)
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

    // 모바일 환경에서 StreamingAssets의 스테이지 JSON 파일을 비동기 로드하여 파싱하는 코루틴
    private IEnumerator LoadStageJsonFromMobile()
    {
        // 스테이지 이름 기반 JSON 파일 경로 생성
        string fileName = GameDataManager.Instance.StageName + ".json";
        string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, fileName);

        // UnityWebRequest로 파일 요청 및 완료 대기
        UnityWebRequest www = UnityWebRequest.Get(filePath);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            // JSON 파싱 후 스테이지 데이터 저장 및 이벤트 호출
            string json = www.downloadHandler.text;
            _currentStage = JsonUtility.FromJson<Stage>(json);
            //InGameManager.Instance.MonsterCount = _currentStage.monsterSpawns.Count;
            OnStageDataLoaded?.Invoke();
        }
    }
}
