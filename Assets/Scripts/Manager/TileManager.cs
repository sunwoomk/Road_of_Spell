using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class TileManager : MonoBehaviour
{
    public static readonly Vector2 StartTilePos = new Vector2(-6.0f, 3f);

    public const int TileWidth = 10;
    public const int TileHeight = 5;
    public const float TileSize = 1.2f;

    private Vector2Int _spellTargetCenterPos;

    private Dictionary<Vector2Int, Tile> _tiles = new Dictionary<Vector2Int, Tile>();
    private List<GameObject> _skillPreviewOverlays = new List<GameObject>();

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
                Vector2Int tilePos = new Vector2Int(x, y);
                Vector2 tileWorldPos = StartTilePos + new Vector2(x * TileSize, -y * TileSize);
                Tile tile = Instantiate(tilePrefab, tileWorldPos, Quaternion.identity).GetComponent<Tile>();
                tile.SetTilePos(tilePos);
                _tiles.Add(tilePos, tile);

                GameObject skillPreviewOverlay = tile.transform.Find("SkillPreviewOverlay").gameObject;
                _skillPreviewOverlays.Add(skillPreviewOverlay);
            }
        }
        SetSkillPreviewActive(false);
    }

    //스킬 타겟 업데이트
    public void UpdateCurSpellTilePos(Vector2Int tilePos, List<Vector2Int> rangeOffsets)
    {
        _spellTargetCenterPos = tilePos;
        UpdateSkillPreviewOverlay(rangeOffsets);
    }

    public Vector3 GetTileWorldPosition(Vector2Int tilePos)
    {
        return StartTilePos + new Vector2(tilePos.x * TileSize, -tilePos.y * TileSize);
    }

    public void SetSkillPreviewActive(bool isActive)
    {
        foreach (var overlay in _skillPreviewOverlays)
        {
            overlay.SetActive(isActive);
        }
    }

    private void UpdateSkillPreviewOverlay(List<Vector2Int> rangeOffsets)
    {
        SetSkillPreviewActive(false);

        // _spellTargetPos를 중심으로 rangeOffsets 더한 타일들만 활성화
        foreach (Vector2Int offset in rangeOffsets)
        {
            Vector2Int targetPos = new Vector2Int(_spellTargetCenterPos.x + offset.x, _spellTargetCenterPos.y + offset.y);
            if (_tiles.ContainsKey(targetPos))
            {
                GameObject overlay = _tiles[targetPos].transform.Find("SkillPreviewOverlay").gameObject;
                overlay.SetActive(true);
            }
        }
    }
}
