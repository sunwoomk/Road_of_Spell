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

    private void Awake()
    {
        _selectionOverlay = transform.Find("SelectionOverlay").gameObject;
        _selectionOverlay.SetActive(false);

        _previewImageObject = transform.Find("PreviewImage").gameObject;
        _previewImageObject.SetActive(false);
        _previewImage = _previewImageObject.GetComponent<Image>();
        _previewImage.sprite = Resources.Load<Sprite>("Textures/PreviewImages/" + _areaName);

        _areaTextBackground = transform.Find("AreaTextBackground").gameObject;
        _areaTextBackground.SetActive(false);
        _areaText = _areaTextBackground.transform.Find("AreaText").GetComponent<TextMeshProUGUI>();
        _areaText.text = _areaName;

        _mapPointButton = GetComponent<Button>();
        _mapPointButton.onClick.AddListener(OpenUI);
    }

    public void OpenUI()
    {
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

    public void SetAreaName(string areaName)
    {
        _areaName = areaName;
    }
}
