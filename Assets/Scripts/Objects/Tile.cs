using UnityEngine;

public class Tile : MonoBehaviour
{
    private Vector2Int _tilePos;

    public Vector2Int TilePos
    {
        get { return _tilePos; }
    }

    public void SetTilePos(Vector2Int tilePos)
    {
        _tilePos = tilePos;
    }
}
