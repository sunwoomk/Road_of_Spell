using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class TileManager : Singleton<TileManager>
{
    public static readonly Vector2 StartTilePos = new Vector2(-5f, 3.5f);

    public static int TileWidth = 10;
    public static int TileHeight = 5;
    public static float TileSize = 1.2f;

    [SerializeField] private Canvas _uiCanvas;

    private Vector2Int _spellTargetCenterPos;

    private Dictionary<Vector2Int, Tile> _tiles = new Dictionary<Vector2Int, Tile>();
    private List<GameObject> _skillPreviewOverlays = new List<GameObject>();

    private void Start()
    {
        SetTiles();
    }

    private void Update()
    {

    }

    public Vector2 WorldToCanvasPosition(Vector3 worldPos) // 월드좌표를 캔버스좌표로 변환
    {
        Canvas canvas = _uiCanvas;
        Vector2 screenPoint = Camera.main.WorldToScreenPoint(worldPos);
        RectTransform canvasRect = canvas.GetComponent<RectTransform>();
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            screenPoint,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : Camera.main,
            out localPoint);
        return localPoint;
    }

    public IEnumerable<Vector2Int> AllTilePositions()
    {
        return _tiles.Keys;
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

        if (rangeOffsets == null || rangeOffsets.Count == 0)
        {
            // 빈 리스트일 경우 모든 오버레이 활성화
            SetSkillPreviewActive(true);
            return;
        }

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
