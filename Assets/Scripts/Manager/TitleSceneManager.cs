using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleSceneManager : MonoBehaviour
{
    private GameObject _pressToStart;
    private GameObject _worldMapButton;
    private GameObject _gameGuideButton;
    private GameObject _optionsButton;
    private GameObject _pressToStartBgArea;

    private void Start()
    {
        SetUI();
    }

    private void SetUI()
    {
        Transform canvas = GameObject.Find("Canvas").transform;

        _pressToStart = canvas.Find("PressToStart").gameObject;

        _worldMapButton = canvas.Find("WorldMapButton").gameObject;
        _worldMapButton.GetComponent<Button>().onClick.AddListener(LoadToWorldMapScene);
        _worldMapButton.SetActive(false);

        _gameGuideButton = canvas.Find("GameGuideButton").gameObject;
        _gameGuideButton.SetActive(false);

        _optionsButton = canvas.Find("OptionsButton").gameObject;
        _optionsButton.SetActive(false);

        _pressToStartBgArea = canvas.Find("PressToStartBackgroundArea").gameObject;
        _pressToStartBgArea.GetComponent<Button>().onClick.AddListener(PressToStartEvent);
    }

    private void PressToStartEvent()
    {
        _pressToStart.SetActive(false);
        _pressToStartBgArea.SetActive(false);
        _worldMapButton.SetActive(true);
        _gameGuideButton.SetActive(true);
        _optionsButton.SetActive(true);
    }

    private void LoadToWorldMapScene()
    {
        SceneManager.LoadScene("WorldMapScene");
    }
}
