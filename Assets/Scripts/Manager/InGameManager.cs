using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class InGameManager : MonoBehaviour
{
    private Player _player;

    private GameObject _canvas;

    [SerializeField]
    private string _playerName;

    [SerializeField]
    private List<string> _playerSkills = new List<string>(); 

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

    //�ӽ÷� Start�Լ����� ȣ��
    private void Start()
    {
        _canvas = GameObject.Find("Canvas");
        SetPlayer(_playerName);
        SetSkillPanel(_playerSkills);
        StageManager.Instance.SpawnMonsters();
    }

    //���߿� ReadyScene���� ȣ���� ����
    public void SetPlayer(string playerName)
    {
        GameObject playerPrefab = Resources.Load<GameObject>("Prefabs/Characters/" + playerName);
        GameObject player = Instantiate(playerPrefab, _canvas.transform);
        _player = player.GetComponent<Player>();
        StageManager.Instance.SetPlayer(_player);
        MonsterManager.Instance.SetPlayer(_player);
    }

    //���߿� ReadyScene���� ȣ���� ����
    public void SetSkillPanel(List<string> playerSkills)
    {
        GameObject skillPanelPrefab = Resources.Load<GameObject>("Prefabs/UI/SkillPanel");
        GameObject skillPanel = Instantiate(skillPanelPrefab, _canvas.transform);
        SkillPanel skillPanelScript = skillPanel.GetComponent<SkillPanel>();
        skillPanelScript.SetSkills(playerSkills, _player);
    }
}
