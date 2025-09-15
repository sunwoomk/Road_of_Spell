using UnityEngine;
using System.Collections.Generic;

public class MonsterManager : Singleton<MonsterManager>
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
        private int _dropExp;

        public MonsterData(int key, string name, int power, int hp, int baseDefense, int speed, int levelPerHp, int levelPerDefence, int dropExp)
        {
            this.key = key;
            this.name = name;
            this.power = power;
            this.hp = hp;
            this._baseDefense = baseDefense;
            this._speed = speed;
            this._levelPerHp = levelPerHp;
            this._levelPerDefence = levelPerDefence;
            this._dropExp = dropExp;
        }

        public int Key => key;
        public string Name => name;
        public int Power => power;
        public int Hp => hp;
        public int BaseDefense => _baseDefense;
        public int Speed => _speed;
        public int LevelPerHp => _levelPerHp;
        public int LevelPerDefence => _levelPerDefence;
        public int DropExp => _dropExp;
    }

    [SerializeField] private Canvas _uiCanvas;
    private Player _player;
    private List<MonsterData> _allMonsterData = new List<MonsterData>();
    public List<MonsterData> AllMonsterData => _allMonsterData;

    private Dictionary<Vector2Int, GameObject> _monsters = new Dictionary<Vector2Int, GameObject>();
    public Dictionary<Vector2Int, GameObject> Monsters => _monsters;

    protected override void Awake()
    {
        base.Awake();
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
                int.Parse(tokens[7]),
                int.Parse(tokens[8])
            );
            monsterList.Add(monster);
        }
        return monsterList;
    }

    public void SetPlayer(Player player)
    {
        _player = player;
    }

    public void CreateMonster(int key, Vector2 worldPos, Vector2Int tilePos) // ������ Ÿ�� ���� ���͸� �����ϴ� �Լ�
    {
        //�Ű������� ���� ������ ���� ������ �ε�
        MonsterData monsterData = _allMonsterData.Find(m => m.Key == key + 100);
        string name = monsterData.Name;
        GameObject monster = Resources.Load<GameObject>("Prefabs/Monsters/" + name);

        // ���� �ν��Ͻ� ���� �� �ʱ�ȭ
        Vector2 canvasPos = TileManager.Instance.WorldToCanvasPosition(worldPos);
        Transform monsters = _uiCanvas.transform.Find("Monsters").transform;
        GameObject monsterInstance = Instantiate(monster, monsters);
        monsterInstance.GetComponent<RectTransform>().anchoredPosition = canvasPos;

        //�ش� ���� ��ũ��Ʈ�� ������ ���� �� Dictionary�� �߰�
        Monster monsterScript = monsterInstance.GetComponent<Monster>();
        monsterScript.SetDatas(monsterData);
        monsterScript.SetPlayer(_player);
        monsterScript.Position = tilePos;
        _monsters.Add(tilePos, monsterInstance);
    }


    public void AllMonstersMove()
    {
        Dictionary<Vector2Int, GameObject> newMonsters = new Dictionary<Vector2Int, GameObject>();

        foreach (GameObject monster in _monsters.Values)
        {
            Monster monsterScript = monster.GetComponent<Monster>();

            //����ǥ ���
            Vector2Int newPos = monsterScript.Position - new Vector2Int(monsterScript.Speed, 0);

            //�̹� ��ǥ��ġ�� ���Ͱ� �ִٸ� 1ĭ �� �̵�
            if (newMonsters.ContainsKey(newPos))
            {
                newPos += new Vector2Int(-1, 0);
            }

            //������ ��ǥ �Ÿ���ŭ �̵�
            int moveDistance = newPos.x - monsterScript.Position.x;
            monsterScript.MonsterMove(moveDistance);

            //���ο� Dictionary�� �߰�
            newMonsters[newPos] = monster;
        }

        // ���ο� Dictionary�� ��ü
        _monsters = newMonsters;
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
