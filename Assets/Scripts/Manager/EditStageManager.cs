using UnityEngine;

public class EditStageManager : Singleton<EditStageManager>
{
    private int _currentMonsterIndex = -1;

    public int CurrentMonsterIndex { get; set; }
}
