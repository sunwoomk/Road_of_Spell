using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MapPointButton : MonoBehaviour
{
    private GameObject _selectionOverlay;
    private GameObject _previewImageObject;
    private Image _previewImage;

    private Button _mapPointButton;

    private GameObject _areaTextBackground;
    private TextMeshProUGUI _areaText;

    private string _areaName;

    private MapPointController _mapPointController;

    public void Init(MapPointController mapPointController, string areaName)
    {
        _areaName = areaName;

        _selectionOverlay = transform.Find("SelectionOverlay").gameObject;
        _selectionOverlay.SetActive(false);

        _previewImageObject = transform.Find("PreviewImage").gameObject;
        _previewImageObject.SetActive(false);
        _previewImage = _previewImageObject.GetComponent<Image>();
        _previewImage.sprite = Resources.Load<Sprite>("Textures/PreviewImages/" + _areaName);
        SetRectPos(_previewImageObject.GetComponent<RectTransform>());

        _areaTextBackground = transform.Find("AreaTextBackground").gameObject;
        _areaTextBackground.SetActive(false);
        _areaText = _areaTextBackground.transform.Find("AreaText").GetComponent<TextMeshProUGUI>();
        _areaText.text = _areaName;
        SetRectPos( _areaTextBackground.GetComponent<RectTransform>());

        _mapPointButton = GetComponent<Button>();
        _mapPointButton.onClick.AddListener(OpenUI);

        _mapPointController = mapPointController;
    }

    private void SetRectPos(RectTransform rect)
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        //만약 버튼의 위치가 중앙 기준 왼쪽에 있다면
        if(rectTransform.anchoredPosition.x < 0)
        {
            //해당 UI를 오른쪽에 배치(기본값은 왼쪽)
            Vector2 newRectPos = new Vector2(-rect.anchoredPosition.x, rect.anchoredPosition.y);
            rect.anchoredPosition = newRectPos;
        }
    }

    public void OpenUI()
    {
        _mapPointController.CloseAllButtons();
        gameObject.SetActive(true);
        _selectionOverlay.SetActive(true);
        _previewImageObject.SetActive(true);
        _areaTextBackground.SetActive(true);
    }

    public void CloseUI()
    {
        _selectionOverlay.SetActive(false);
        _previewImageObject.SetActive(false);
        _areaTextBackground.SetActive(false);
    }
}
