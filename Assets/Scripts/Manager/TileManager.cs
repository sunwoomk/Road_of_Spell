using UnityEngine;

public class TileManager : MonoBehaviour
{
    public static readonly Vector2 StartTilePos = new Vector2(-6.0f, 3f);

    public const int TileWidth = 10;
    public const int TileHeight = 5;
    public const float TileSize = 1.2f;

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
                Vector2 tilePos = StartTilePos + new Vector2(x * TileSize, -y * TileSize);
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
        return StartTilePos + new Vector2(tilePos.x * TileSize, -tilePos.y * TileSize);
    }
}
