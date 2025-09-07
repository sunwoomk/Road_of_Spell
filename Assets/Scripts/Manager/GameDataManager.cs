using System.Collections.Generic;
using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager Instance { get; private set; }

    public string PlayerName;
    public string ClassSkillName;
    public string CommonSkillName;

    public Dictionary<string, string> PlayerElementPairs = new Dictionary<string, string>
    {
            { "BloodMage", "Fire" },
            { "Druid", "Void" },
            { "MagicRogue", "Electric" },
            { "Viking", "Holy" }
    };
    public List<string> CommonSkillElements = new List<string> { "Technology" };
    public List<string> AllSkillElements = new List<string> { "Fire", "Void", "Electric", "Holy", "Technology" };

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
