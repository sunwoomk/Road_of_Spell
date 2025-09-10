using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ContinueButton : MonoBehaviour
{
    private string _selectedAreaName;
    private string _selectedAreaType;

    private Button _continueButton;

    private void Start()
    {
        _continueButton = GetComponent<Button>();
        _continueButton.onClick.AddListener(OnContinueButtonClicked);
    }

    public void SetAreaData(string areaName, string areaType)
    {
        _selectedAreaName = areaName;
        _selectedAreaType = areaType;
    }

    private void OnContinueButtonClicked()
    {
        if(_selectedAreaType == "Dungeon")
        {
            GameDataManager.Instance.StageName = _selectedAreaName;
            SceneManager.LoadScene("ReadyScene");
        }
    }
}
