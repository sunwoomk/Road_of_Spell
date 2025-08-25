using UnityEngine;
using System.Collections.Generic;

public class MonsterManager : MonoBehaviour
{
    public struct MonsterData
    {
        private int key;
        private string name;
        private int power;
        private int hp;
        private int _baseDefense;
        private int _speed;
        private int _levelPerHp;
        private int _levelPerDefence;

        public MonsterData(int key, string name, int power, int hp, int baseDefense, int speed, int levelPerHp, int levelPerDefence)
        {
            this.key = key;
            this.name = name;
            this.power = power;
            this.hp = hp;
            this._baseDefense = baseDefense;
            this._speed = speed;
            this._levelPerHp = levelPerHp;
            this._levelPerDefence = levelPerDefence;
        }

        public int Key => key;
        public string Name => name;
        public int Power => power;
        public int Hp => hp;
        public int BaseDefense => _baseDefense;
        public int Speed => _speed;
        public int LevelPerHp => _levelPerHp;
        public int LevelPerDefence => _levelPerDefence;
    }

    private List<MonsterData> _allMonsterData = new List<MonsterData>();
    private List<GameObject> _monsters = new List<GameObject>();

    public static MonsterManager Instance { get; private set; }

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

        _allMonsterData = LoadMonsterData("Tables/MonsterTable");
    }

    private List<MonsterData> LoadMonsterData(string csvFilePath)
    {
        List<MonsterData> monsterList = new List<MonsterData>();
        TextAsset csvData = Resources.Load<TextAsset>(csvFilePath);

        string[] lines = csvData.text.Split('\n');
        for (int i = 1; i < lines.Length; i++)
        {
            string line = lines[i].Trim();
            if (string.IsNullOrEmpty(line)) continue;

            string[] tokens = line.Split(',');
            if (tokens.Length < 8) continue;

            MonsterData monster = new MonsterData(
                int.Parse(tokens[0]),
                tokens[1],
                int.Parse(tokens[2]),
                int.Parse(tokens[3]),
                int.Parse(tokens[4]),
                int.Parse(tokens[5]),
                int.Parse(tokens[6]),
                int.Parse(tokens[7])
            );
            monsterList.Add(monster);
        }
        return monsterList;
    }

    public void CreateMonster(int key, Vector2 position)
    {
        MonsterData monsterData = _allMonsterData.Find(m => m.Key == key + 100);
        string name = monsterData.Name;
        GameObject monster = Resources.Load<GameObject>("Prefabs/Monsters/" + name);
        Instantiate(monster, position, Quaternion.identity);
        Monster monsterScript = monster.GetComponent<Monster>();
        monsterScript.SetDatas(monsterData);
        _monsters.Add(monster);
    }
}
