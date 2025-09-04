using UnityEngine;
using UnityEngine.UI;

public class EditTileButton : MonoBehaviour
{
    private Vector2Int _position;

    private GameObject _stagePreview;
    private GameObject _monsterPreview;

    private Button _button;

    public Vector2Int Position { get; set; }

    private void Start()
    {
        _stagePreview = GameObject.Find("StagePreview");
    }

    public void Init(int x, int y)
    {
        _button = GetComponent<Button>();
        _position = new Vector2Int(x, y);
        _button.onClick.AddListener(() => SetMonsterData(x, y));

        Transform monsterPreviewTransform = transform.Find("MonsterPreview");
        _monsterPreview = monsterPreviewTransform.gameObject;
        _monsterPreview.SetActive(false);
    }

    private void SetMonsterData(int x, int y)
    {
        int currentMonsterIndex = EditStageManager.Instance.CurrentMonsterIndex;
        _stagePreview.GetComponent<StagePreview>().AddMonsterSpawnData(x, y, currentMonsterIndex);
        SetMonsterPreview(currentMonsterIndex);
        _monsterPreview.SetActive(true);
    }

    private void SetMonsterPreview(int key)
    {
        //SpriteRenderer image = _monsterPreview.GetComponent<SpriteRenderer>();
        //image = Resources.Load<SpriteRenderer>("Textures/Monster" + key);

        Sprite newSprite = Resources.Load<Sprite>("Textures/MonsterIcon/Monster" + key);
        Image image = _monsterPreview.GetComponent<Image>();
        image.sprite = newSprite;
    }
}
