using System.Collections.Generic;
using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    private string _playerName;
    private List<string> _skillNames = new List<string>();

    private Dictionary<string, List<string>> _playerElementPairs = new Dictionary<string, List<string>>
    {
            { "BloodMage", new List<string>{"Fire", "Water"} },
            { "Druid", new List<string>{"Slash", "Technology" } },
            { "MagicRogue", new List<string>{"Void", "Electric"} },
            { "Viking", new List<string>{"Holy", "Ice"} }
    };

    private List<string> _allSkillElements = new List<string>
    {
        "Fire", "Water", "Slash", "Technology", "Void", "Electric", "Holy", "Ice"
    };

    public string PlayerName 
    { 
        get { return _playerName; } 
        set { _playerName = value; }
    }
    public List<string> SkillNames { get { return _skillNames; } }

    public Dictionary<string, List<string>> PlayerElementPairs {  get { return _playerElementPairs; } }

    public List<string> AllSkillElements {  get { return _allSkillElements; } }

    public static GameDataManager Instance { get; private set; }

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
}
