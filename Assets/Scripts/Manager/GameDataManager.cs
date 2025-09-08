using System.Collections.Generic;
using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager Instance { get; private set; }

    public string PlayerName;
    public List<string> SkillNames = new List<string>();

    public Dictionary<string, List<string>> PlayerElementPairs = new Dictionary<string, List<string>>
    {
            { "BloodMage", new List<string>{"Fire", "Water"} },
            { "Druid", new List<string>{"Slash", "Technology" } },
            { "MagicRogue", new List<string>{"Void", "Electric"} },
            { "Viking", new List<string>{"Holy", "Ice"} }
    };

    public List<string> AllSkillElements = new List<string>
    {
        "Fire", "Water", "Slash", "Technology", "Void", "Electric", "Holy", "Ice"
    };

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
