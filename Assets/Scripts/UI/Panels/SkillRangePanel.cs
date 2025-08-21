using System.Collections.Generic;
using UnityEngine;

public class SkillRangePanel : MonoBehaviour
{
    private const float TileSize = 10;

    private GameObject _normalTile;
    private GameObject _highlightTile;

    private void Start()
    {
        _normalTile = Resources.Load<GameObject>("Prefabs/UI/SkillRangeTile_Normal");
        _highlightTile = Resources.Load<GameObject>("Prefabs/UI/SkillRangeTile_Highlight");
    }

    public void SetTiles(List<Vector2Int> baseRangeOffsets) // ��ų�� ��ȿ Ÿ�� ������ �Ű������� �ޱ�
    {
        for(int y = -2; y <= 2; y++)
        {
            for (int x = -2; x <= 2; x++)
            {
                Vector2Int currentPos = new Vector2Int(x, y);
                if(baseRangeOffsets.Contains(currentPos)) // ���� Ÿ�� ��ġ�� ��ų�� ��ȿ Ÿ�� ���� ���� ���ԵǾ� �ִ� ���
                {
                    GameObject highlightTile = Instantiate(_highlightTile, 
                        new Vector3(TileSize * x, TileSize * y, 0), Quaternion.identity);
                    highlightTile.transform.SetParent(transform, false);
                }
                else // ���� Ÿ�� ��ġ�� ��ų�� ��ȿ Ÿ�� ���� ���� ���ԵǾ� ���� ���� ���
                {
                    GameObject normalTile = Instantiate(_normalTile, 
                        new Vector3(TileSize * x, TileSize * y, 0), Quaternion.identity);
                    normalTile.transform.SetParent(transform, false);
                }
            }
        }
    }
}
