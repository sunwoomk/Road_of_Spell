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

    [SerializeField]
    private string _areaName;

    private void Awake()
    {
        _selectionOverlay = transform.Find("SelectionOverlay").gameObject;
        _selectionOverlay.SetActive(false);

        _previewImageObject = transform.Find("PreviewImage").gameObject;
        _previewImageObject.SetActive(false);
        _previewImage = _previewImageObject.GetComponent<Image>();
        _previewImage.sprite = Resources.Load<Sprite>("Textures/PreviewImages/" + _areaName);

        _mapPointButton = GetComponent<Button>();
        _mapPointButton.onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        _selectionOverlay.SetActive(true);
        _previewImageObject.SetActive(true);
    }
}
