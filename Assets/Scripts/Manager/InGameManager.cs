using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class InGameManager : Singleton<InGameManager>
{
    private Player _player;

    private GameObject _canvas;

    private GameObject _skillRangePanel;

    private string _playerName;

    private List<string> _playerSkills = new List<string>(); 
    private List<GameObject> _skillLevelUpButtons = new List<GameObject>();

    public GameObject Canvas { get { return _canvas; } }
    public Player Player { get { return _player; } }

    public GameObject SkillRangePanel { get { return _skillRangePanel; } }

    //임시로 Start함수에서 호출
    private void Start()
    {
        _playerName = GameDataManager.Instance.PlayerName;
        _playerSkills = GameDataManager.Instance.SkillNames;

        _canvas = GameObject.Find("Canvas");
        _skillRangePanel = GameObject.Find("SkillRangePanel");
        SetPlayer(_playerName);
        SetSkillPanel(_playerSkills);
        StageManager.Instance.SpawnMonsters();
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
