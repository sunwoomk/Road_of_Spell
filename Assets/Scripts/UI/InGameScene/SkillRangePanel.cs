using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillRangePanel : MonoBehaviour
{
    private const float TileSize = 38;

    private GameObject _normalTile;
    private GameObject _highlightTile;

    private Dictionary<Vector2Int, GameObject> _tiles = new Dictionary<Vector2Int, GameObject>();

    private void Start()
    {
        _normalTile = Resources.Load<GameObject>("Prefabs/UI/SkillRangeTile_Normal");
        _highlightTile = Resources.Load<GameObject>("Prefabs/UI/SkillRangeTile_Highlight");

        for (int y = -2; y <= 2; y++)
        {
            for (int x = -2; x <= 2; x++)
            {
                Vector2Int pos = new Vector2Int(x, y);
                GameObject tile = Instantiate(_normalTile, new Vector3(TileSize * x, TileSize * y, 0), Quaternion.identity);
                tile.transform.SetParent(transform, false);
                _tiles.Add(pos, tile);
            }
        }
    }

    public void SetTiles(List<Vector2Int> baseRangeOffsets) // 스킬의 유효 타격 범위를 매개변수로 받기
    {
        foreach (KeyValuePair<Vector2Int, GameObject> kvp in _tiles)
        {
            Vector2Int pos = kvp.Key;
            GameObject tile = kvp.Value;

            // 범위 안이면 하이라이트, 아니면 일반 타일 모양 적용
            if (baseRangeOffsets.Contains(pos))
            {
                tile.GetComponent<Image>().sprite = _highlightTile.GetComponent<Image>().sprite;
            }
            else
            {
                tile.GetComponent<Image>().sprite = _normalTile.GetComponent<Image>().sprite;
            }
        }
    }
}
