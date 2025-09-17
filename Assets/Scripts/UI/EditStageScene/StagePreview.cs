using System.Collections.Generic;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class StagePreview : MonoBehaviour
{
    private TMP_InputField _inputStageCount;
    private ScrollRect _scrollRect;
    private RectTransform _content;
    private RectTransform _viewPort;
    private GameObject _editTilePrefab;

    private GameObject _saveStageButtonObject;
    private Button _saveStageButton;

    private GameObject _inputStageNameObject;
    private TMP_InputField _inputStageName;

    private List<StageManager.MonsterSpawnData> _monsterSpawnDatas = new List<StageManager.MonsterSpawnData>();

    private const int _buttonsPerColumn = 5;
    private float _buttonSize = 200f;
    private int _roundCount;

    private void Start()
    {
        _inputStageCount = GameObject.Find("InputRoundCount")?.GetComponent<TMP_InputField>();
        _scrollRect = GetComponent<ScrollRect>();
        _content = _scrollRect.content.GetComponent<RectTransform>();
        _viewPort = _scrollRect.viewport.GetComponent<RectTransform>();
        _editTilePrefab = Resources.Load<GameObject>("Prefabs/UI/EditTile");

        _saveStageButtonObject = GameObject.Find("SaveStageButton");
        _saveStageButton = _saveStageButtonObject.GetComponent<Button>();
        _inputStageNameObject = _saveStageButtonObject.transform.Find("InputStageName")?.gameObject;
        _inputStageName = _inputStageNameObject.GetComponent<TMP_InputField>();
        _inputStageNameObject.SetActive(false);

        _inputStageCount.onEndEdit.AddListener(CreateStagePreview);
        _saveStageButton.onClick.AddListener(SetInputStageNameTrue);
    }

    private void Update()
    {
        ChectScrollBounds();
    }

    private void ChectScrollBounds()
    {
        Vector2 pos = _content.anchoredPosition;

        // X�� ����
        float minX = Mathf.Min(0, _viewPort.rect.width - _content.rect.width);
        float maxX = 0;
        pos.x = Mathf.Clamp(pos.x, minX, maxX);

        // Y�� ����
        float minY = Mathf.Min(0, _viewPort.rect.height - _content.rect.height);
        float maxY = 0;
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        _content.anchoredPosition = pos;
    }

    public void CreateStagePreview(string text)
    {
        if (int.TryParse(text, out int inputCount))
        {
            _roundCount = inputCount;
            int buttonCount = _roundCount * 5;
            CreateEditTiles(inputCount);
            AdjustContentWidth(buttonCount);
            _inputStageName.onEndEdit.AddListener(SaveStageDataToJson);
        }
        else
        {
            Debug.LogWarning("�ùٸ� ���ڸ� �Է��ϼ���.");
        }
    }

    private void CreateEditTiles(int count)
    {
        // ���� ��ư ����
        foreach (Transform child in _content)
        {
            Destroy(child.gameObject);
        }

        // ��ư ����
        for(int x = 0; x < count; x++)
        {
            for(int y = 0; y < 5; y++)
            {
                GameObject editTileButtonInstance = Instantiate(_editTilePrefab, _content);
                EditTileButton editTileButton = editTileButtonInstance.GetComponent<EditTileButton>();
                editTileButton.Init(x, y);
            }
        }
    }

    private void AdjustContentWidth(int buttonCount)
    {
        // ��ư ������ ���� �ʿ��� �� �� ��� (�������� ���� ��� �ø� ó��)
        int totalRows = Mathf.CeilToInt(buttonCount / (float)_buttonsPerColumn);

        // �� ���� ��ư ũ�⸦ ������� Content�� ����/���� ũ�� ����
        float width = totalRows * _buttonSize;
        float fixedHeight = _buttonSize * _buttonsPerColumn;

        _content.sizeDelta = new Vector2(width, fixedHeight);
    }

    public void AddMonsterSpawnData(int x, int y, int key)
    {
        StageManager.MonsterSpawnData data = new StageManager.MonsterSpawnData();
        data.x = x; data.y = y; data.key = key;

        _monsterSpawnDatas.Add(data);
    }

    private void SetInputStageNameTrue() 
    {
        _inputStageNameObject.SetActive(true);
    }

    private void SaveStageDataToJson(string text)
    {
        StageManager.Stage stage = new StageManager.Stage();
        stage.roundCount = _roundCount;
        stage.monsterSpawns = _monsterSpawnDatas;

        string json = JsonUtility.ToJson(stage, true);

        //������ ����
        File.WriteAllText(Application.dataPath + "/Resources/Stages/" + text + ".json", json);
        //�ȵ���̵� ����
        File.WriteAllText(Application.dataPath + "/StreamingAssets/" + text + ".json", json);

        Debug.Log("Stage Created! : " + text);
    }
}
