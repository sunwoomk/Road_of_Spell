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

    [SerializeField] private Canvas _uiCanvas;
    private List<MonsterData> _allMonsterData = new List<MonsterData>();
    //private List<GameObject> _monsters = new List<GameObject>();
    private Dictionary<Vector2Int, GameObject> _monsters = new Dictionary<Vector2Int, GameObject>();

    //public List<GameObject> Monsters => _monsters;
    public Dictionary<Vector2Int, GameObject> Monsters => _monsters;

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

    public void CreateMonster(int key, Vector2 worldPos, Vector2Int tilePos)
    {
        MonsterData monsterData = _allMonsterData.Find(m => m.Key == key + 100);
        string name = monsterData.Name;
        GameObject monster = Resources.Load<GameObject>("Prefabs/Monsters/" + name);

        Vector2 canvasPos = TileManager.Instance.WorldToCanvasPosition(worldPos);
        GameObject monsterInstance = Instantiate(monster, _uiCanvas.transform);
        monsterInstance.GetComponent<RectTransform>().anchoredPosition = canvasPos;

        Monster monsterScript = monsterInstance.GetComponent<Monster>();
        monsterScript.SetDatas(monsterData);
        monsterScript.Position = tilePos;
        _monsters.Add(tilePos, monsterInstance);
    }

    public void AllMonstersMove()
    {
        foreach(GameObject monster in _monsters.Values)
        {
            Monster monsterScript = monster.GetComponent<Monster>();
            monsterScript.MonsterMove();
        }
    }

    public void RemoveMonster(GameObject monster)
    {
        var pos = monster.GetComponent<Monster>().Position;
        if (_monsters.ContainsKey(pos))
        {
            _monsters.Remove(pos);
        }
        Destroy(monster);
    }
}
