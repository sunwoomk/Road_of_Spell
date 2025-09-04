using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class InGameManager : MonoBehaviour
{
    private Player _player;

    private GameObject _canvas;

    private GameObject _skillRangePanel;

    [SerializeField]
    private string _playerName;

    [SerializeField]
    private List<string> _playerSkills = new List<string>(); 
    private List<GameObject> _skillLevelUpButtons = new List<GameObject>();

    public GameObject Canvas { get { return _canvas; } }
    public Player Player { get { return _player; } }

    public GameObject SkillRangePanel { get { return _skillRangePanel; } }

    public static InGameManager Instance { get; private set; }

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

    //임시로 Start함수에서 호출
    private void Start()
    {
        _canvas = GameObject.Find("Canvas");
        _skillRangePanel = GameObject.Find("SkillRangePanel");
        SetPlayer(_playerName);
        SetSkillPanel(_playerSkills);
        StageManager.Instance.SpawnMonsters();
    }

    //나중에 ReadyScene에서 호출할 예정
    public void SetPlayer(string playerName)
    {
        GameObject playerPrefab = Resources.Load<GameObject>("Prefabs/Characters/" + playerName);
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
        GameObject skillPanelPrefab = Resources.Load<GameObject>("Prefabs/UI/SkillPanel");
        GameObject skillPanel = Instantiate(skillPanelPrefab, _canvas.transform);
        skillPanel.name = "SkillPanel";
        SkillPanel skillPanelScript = skillPanel.GetComponent<SkillPanel>();
        skillPanelScript.SetSkills(playerSkills, _player);
        skillPanelScript.CreateSkillSelectButton();
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
