using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InGameManager : Singleton<InGameManager>
{
    private Player _player;

    private GameObject _canvas;

    private GameObject _skillRangePanel;

    private GameObject _inGameBackground;

    private GameObject _gameResultPanel;

    private string _playerName;

    [SerializeField]
    private int _monsterCount;
    [SerializeField]
    private int _killCount;

    private List<string> _playerSkills = new List<string>(); 
    private List<GameObject> _skillLevelUpButtons = new List<GameObject>();

    public GameObject Canvas { get { return _canvas; } }
    public Player Player { get { return _player; } }

    public GameObject SkillRangePanel { get { return _skillRangePanel; } }

    public int MonsterCount
    {
        get { return _monsterCount; }
        set 
        { 
            _monsterCount = value;
            Debug.Log("MonsterCount = " + value);
        }
    }

    public int KillCount
    {
        get { return _killCount; }
        set 
        { 
            _killCount = value;
            Debug.Log("KillCount = " + value);
        }
    }

    //임시로 Start함수에서 호출
    private void Start()
    {
        _inGameBackground = GameObject.Find("InGameBackground");
        _inGameBackground.GetComponent<SpriteRenderer>().sprite = 
            Resources.Load<Sprite>("Textures/AreaImages/" + GameDataManager.Instance.StageName);

        _playerName = GameDataManager.Instance.PlayerName;
        _playerSkills = GameDataManager.Instance.SkillNames;

        _canvas = GameObject.Find("Canvas");
        _skillRangePanel = GameObject.Find("SkillRangePanel");

        SetPlayer(_playerName);
        SetSkillPanel(_playerSkills);
        SetGameResultPanel();
        //StageManager.Instance.SpawnMonsters();
        #if UNITY_ANDROID
            StageManager.Instance.OnStageDataLoaded += OnStageDataLoaded;
        #else
            // PC 환경에서는 LoadStageJson() 동기 호출하므로 바로 진행 가능
            StageManager.Instance.SpawnMonsters();
        #endif

        MonsterCount = StageManager.Instance.CurrentStage.monsterSpawns.Count;
    }

    private void OnStageDataLoaded()
    {
        StageManager.Instance.SpawnMonsters();
        StageManager.Instance.OnStageDataLoaded -= OnStageDataLoaded;
    }

    //나중에 ReadyScene에서 호출할 예정
    public void SetPlayer(string playerName)
    {
        GameObject playerPrefab = Resources.Load<GameObject>("Prefabs/Players/" + playerName);
        GameObject player = Instantiate(playerPrefab, _canvas.transform);
        _player = player.GetComponent<Player>();
        StageManager.Instance.SetPlayer(_player);
        MonsterManager.Instance.SetPlayer(_player);
        GameObject.Find("ManaPanel").GetComponent<ManaPanel>().SetPlayer(_player);
        GameObject.Find("ExpBar").GetComponent<ExpBar>().SetPlayer(_player);
        GameObject.Find("PlayerHpPanel").GetComponent<PlayerHpPanel>().SetPlayer(_player);
        GameObject.Find("PlayerHpPanel").GetComponent<PlayerHpPanel>().SetPlayerHp();
    }

    //나중에 ReadyScene에서 호출할 예정
    public void SetSkillPanel(List<string> playerSkills)
    {
        GameObject inGameSkillPanelPrefab = Resources.Load<GameObject>("Prefabs/UI/InGameSkillPanel");
        GameObject inGameSkillPanel = Instantiate(inGameSkillPanelPrefab, _canvas.transform);
        inGameSkillPanel.name = "InGameSkillPanel";
        InGameSkillPanel inGameSkillPanelScript = inGameSkillPanel.GetComponent<InGameSkillPanel>();
        inGameSkillPanelScript.SetSkills(playerSkills, _player);
        inGameSkillPanelScript.CreateSkillSelectButton();
    }

    private void SetGameResultPanel()
    {
        _gameResultPanel = _canvas.transform.Find("GameResultPanel").gameObject;
        _gameResultPanel.SetActive(false);
        Button worldMapButton = _gameResultPanel.transform.Find("WorldMapButton").gameObject.GetComponent<Button>();
        worldMapButton.onClick.AddListener(LoadWorldMapScene);
    }

    private void LoadWorldMapScene()
    {
        SceneManager.LoadScene("WorldMapScene");
    }

    public void SetGameResultPanelActiveTrue(string gameResult) 
    {
        _gameResultPanel.SetActive(true);
        if(gameResult == "GameClear")
        {
            _gameResultPanel.transform.Find("GameClearTextImage").gameObject.SetActive(true);
            _gameResultPanel.transform.Find("GameOverTextImage").gameObject.SetActive(false);
        }
        else if(gameResult == "GameOver")
        {
            _gameResultPanel.transform.Find("GameClearTextImage").gameObject.SetActive(false);
            _gameResultPanel.transform.Find("GameOverTextImage").gameObject.SetActive(true);
        }
    }

    public void AddSkillLevelUpButton(GameObject button)
    {
        _skillLevelUpButtons.Add(button);
    }

    public void SetActiveSkillLevelUpButtons(bool isActive)
    {
        foreach (GameObject button in _skillLevelUpButtons)
        {
            button.SetActive(isActive);
        }
    }


}
