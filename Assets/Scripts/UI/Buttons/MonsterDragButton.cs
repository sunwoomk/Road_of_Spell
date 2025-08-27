using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MonsterDragButton : MonoBehaviour
{
    private int _monsterIndex = -1;

    private Button _button;

    public void Init(int monsterIndex)
    {
        _monsterIndex = monsterIndex;
        _button = GetComponent<Button>();
        _button.onClick.AddListener(SetCurMonsterIndex);
    }

    private void SetCurMonsterIndex()
    {
        EditStageManager.Instance.CurrentMonsterIndex = _monsterIndex;
    }
}
