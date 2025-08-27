using UnityEngine;

public class EditStageManager : MonoBehaviour
{
    private int _currentMonsterIndex = -1;

    public int CurrentMonsterIndex { get; set; }

    public static EditStageManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
