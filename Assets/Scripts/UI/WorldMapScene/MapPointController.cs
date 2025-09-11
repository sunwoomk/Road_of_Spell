using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class MapPointController : MonoBehaviour
{
    private const int _areaCount = 6;

    private struct AreaData
    {
        public Vector3 position;
        public string areaName;
        public string areaType;
        public AreaData(Vector3 pos, string name, string type)
        {
            position = pos;
            areaName = name;
            areaType = type;
        }
    }

    //후에 스테이지 및 다른 씬 늘어나면 여기에 추가
    private AreaData[] _areaDatas = new AreaData[6]
    {
        new AreaData(new Vector3(777, 350, 0), "EldaraDesert", "Dungeon"),
        new AreaData(new Vector3(-295, -32, 0), "NobrickTown", "Town"),
        new AreaData(new Vector3(-800, 166, 0), "SilmardinSwamp", "Dungeon"),
        new AreaData(new Vector3(747, -87, 0), "SilvaronHills", "Dungeon"),
        new AreaData(new Vector3(614, -335, 0), "TaberonPeaks", "Dungeon"),
        new AreaData(new Vector3(120, -366, 0), "TariasForest", "Dungeon")
    };

    private List<GameObject> _mapPointButtons = new List<GameObject>();

    private GameObject _mapPointButtonPrefab;
    private Transform _mapPointButtonsParent;

    private GameObject _backgroundCloseArea;
    private ContinueButton _continueButton;

    private void Start()
    {
        Transform canvas = GameObject.Find("Canvas").transform;
        _mapPointButtonsParent = canvas.Find("MapPointButtons").transform;
        _backgroundCloseArea = canvas.Find("BackgroundCloseArea").gameObject;
        _backgroundCloseArea.GetComponent<Button>().onClick.AddListener(CloseAllButtonsUI);
        _continueButton = canvas.Find("ContinueButton").gameObject.GetComponent<ContinueButton>();
        SetMapPointButtons();
    }

    private void SetMapPointButtons()
    {
        _mapPointButtons.Clear();

        _mapPointButtonPrefab = Resources.Load<GameObject>("Prefabs/UI/MapPointButton");
        for(int i = 0;  i < _areaCount; i++)
        {
            AreaData areaData = _areaDatas[i];
            GameObject mapPointButtonObject = Instantiate(_mapPointButtonPrefab, _mapPointButtonsParent);
            mapPointButtonObject.transform.localPosition = areaData.position;
            MapPointButton mapPointButton = mapPointButtonObject.GetComponent<MapPointButton>();
            mapPointButtonObject.GetComponent<Button>().onClick.AddListener
                (() => OnClickEventMapButton(mapPointButton, areaData.areaName, areaData.areaType));
            mapPointButton.Init(this, areaData.areaName);
            _mapPointButtons.Add(mapPointButtonObject);
        }
    }

    //각 버튼에 이벤트 등록
    private void OnClickEventMapButton(MapPointButton mapPointButton, string areaName, string areaType)
    {
        mapPointButton.OpenUI();
        _continueButton.SetAreaData(areaName, areaType);
    }

    //버튼의 하위 UI들 비활성화
    private void CloseAllButtonsUI()
    {
        foreach (GameObject mapPointButton in _mapPointButtons)
        {
            mapPointButton.GetComponent<MapPointButton>().CloseUI();
            mapPointButton.SetActive(true);
        }
    }

    //모든 버튼 비활성화
    public void CloseAllButtons()
    {
        foreach (GameObject mapPointButton in _mapPointButtons)
        {
            mapPointButton.SetActive(false);
        }
    }
}
