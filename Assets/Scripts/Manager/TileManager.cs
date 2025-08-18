using UnityEngine;

public class TileManager : MonoBehaviour
{
    private readonly Vector2 _startTilePos = new Vector2(-4.0f, 4.4f);

    private const int TileWidth = 10;
    private const int TileHeight = 5;
    private const float TileSize = 1.2f;

    private Vector2Int _curSpellTilePos;

    public static TileManager Instance { get; private set; }

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

    private void Start()
    {
        SetTiles();
    }

    private void Update()
    {

    }

    private void SetTiles()
    {
        GameObject tilePrefab = Resources.Load<GameObject>("Prefabs/Tile");
        for (int y = 0; y < TileHeight; y++)
        {
            for (int x = 0; x < TileWidth; x++)
            {
                Vector2 tilePos = _startTilePos + new Vector2(x * TileSize, -y * TileSize);
                Tile tile = Instantiate(tilePrefab, tilePos, Quaternion.identity).GetComponent<Tile>();
                tile.SetTilePos(new Vector2Int(x, y));
            }
        }
    }

    public void SetCurSpellTilePos(Vector2Int tilePos)
    {
        _curSpellTilePos = tilePos;
    }

    public Vector3 GetTileWorldPosition(Vector2Int tilePos)
    {
        return _startTilePos + new Vector2(tilePos.x * TileSize, -tilePos.y * TileSize);
    }
}
