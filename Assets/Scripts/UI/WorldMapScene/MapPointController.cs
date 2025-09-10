using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class MapPointController : MonoBehaviour
{
    private const int _areaCount = 6;

    private Vector3[] _mapPointPositions = new Vector3[6];
    private string[] _areaNames = new string[6];

    private List<MapPointButton> _mapPointButtons = new List<MapPointButton>();

    private GameObject _mapPointButtonPrefab;
    private Transform _mapPointButtonsParent;

    private void Start()
    {
        _mapPointButtonsParent = GameObject.Find("Canvas").transform.Find("MapPointButtons").transform;
        SetMapPointButtons();
    }

    private void SetMapPointButtons()
    {
        _mapPointButtons.Clear();

        _mapPointButtonPrefab = Resources.Load<GameObject>("Prefabs/UI/MapPointButton");
        for(int i = 0;  i < _areaCount; i++)
        {
            GameObject mapPointButtonObject = Instantiate(_mapPointButtonPrefab, _mapPointPositions[i], Quaternion.identity, _mapPointButtonsParent);
            MapPointButton mapPointButton = mapPointButtonObject.GetComponent<MapPointButton>();
            mapPointButton.SetAreaName(_areaNames[i]);
            _mapPointButtons.Add(mapPointButton);
        }
    }
}
