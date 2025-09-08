using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEditor;
using System.Linq;
using UnityEngine.SceneManagement;

public class ReadySceneManager : Singleton<ReadySceneManager>
{
    private string _playerName;

    private List<string> _playerNames = new List<string>();

    private Dictionary<string, Sprite> _playerPortraits = new Dictionary<string, Sprite>();

    private Button _nextButton;
    private Button _selectPlayerButton;
    private Button _startButton;
    private Image _playerPortrait;

    private SkillSetUpPanel _skillSetUpPanel;

    public string PlayerName 
    {
        get { return _playerName; } 
        set { _playerName = value; }
    }

    private void Start()
    {
        _playerPortrait = GameObject.Find("PlayerPortrait").GetComponent<Image>();
        _skillSetUpPanel = GameObject.Find("SkillSetUpPanel").GetComponent<SkillSetUpPanel>();
        SetButtons();
        LoadPlayerNames();
        SetPortrait(_playerName);
    }

    private void SetButtons()
    {
        _nextButton = GameObject.Find("NextButton").GetComponent<Button>();
        _nextButton.onClick.AddListener(NextPlayerPortrait);

        _selectPlayerButton = GameObject.Find("SelectPlayerButton").GetComponent<Button>();
        _selectPlayerButton.onClick.AddListener(SelectPlayer);

        _startButton = GameObject.Find("StartButton").GetComponent<Button>();
        _startButton.onClick.AddListener(StartInGame);    
    }

    private void LoadPlayerNames()
    {
        _playerNames.Clear();

        //해당 경로에 있는 프리팹 전부 가져오기
        GameObject[] prefabs = Resources.LoadAll<GameObject>("Prefabs/Players");

        // 이름순으로 정렬
        var sortedPrefabs = prefabs.OrderBy(p => p.name);

        foreach (GameObject prefab in sortedPrefabs)
        {
            // 첫 캐릭터 이름 넣기
            if (_playerName == null) _playerName = prefab.name;
            _playerNames.Add(prefab.name);
            LoadPortrait(prefab.name);
        }
    }

    private void LoadPortrait(string playerName)
    {
        Sprite sprite = Resources.Load<Sprite>("Textures/PlayerPortraits/" + playerName);
        _playerPortraits.Add(playerName, sprite);
    }

    private void SetPortrait(string playerName)
    {
        // Dictionary에서 이름이 같은 매개변수를 찾아서 sprite 반환
        if (_playerPortraits.TryGetValue(playerName, out Sprite sprite))
        {
            _playerPortrait.sprite = sprite;
        }

        _skillSetUpPanel.SetClassSkillImage(playerName);
    }

    private void NextPlayerPortrait()
    {
        int currentIndex = _playerNames.IndexOf(_playerName);

        int nextIndex = (currentIndex + 1) % _playerNames.Count;  // 리스트 끝이면 다시 처음으로

        _playerName = _playerNames[nextIndex];  // 다음 플레이어 이름으로 변경
        SetPortrait(_playerName);
    }

    private void SelectPlayer()
    {
        _nextButton.gameObject.SetActive(false);
    }

    private void StartInGame()
    {
        GameDataManager.Instance.PlayerName = _playerName;
        SceneManager.LoadScene("InGameScene");
    }

    //private string GetSkillNameFromPlayerName(string playerName)
    //{
    //    switch (playerName)
    //    {
    //        case "BloodMage":
    //            return "FirePit";
    //        case "Druid":
    //            return "VoidBlackHole";
    //        case "MagicRogue":
    //            return "ElectricExplosion";
    //        case "Viking":
    //            return "HolyCross";
    //        default:
    //            return "Error";
    //    }
    //}
}
