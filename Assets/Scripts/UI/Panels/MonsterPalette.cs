using UnityEngine;
using UnityEngine.UI;

public class MonsterPalette : MonoBehaviour
{
    private GameObject _monsterDragButtonPrefab;

    private void Start()
    {
        _monsterDragButtonPrefab = Resources.Load<GameObject>("Prefabs/UI/MonsterDragButton");
        SetButtons();
    }

    private void SetButtons()
    {
        RectTransform content = GetComponent<ScrollRect>().content.GetComponent<RectTransform>();
        for(int i = 1; i <= MonsterManager.Instance.AllMonsterData.Count; i++)
        {
            GameObject monsterDragButton = Instantiate(_monsterDragButtonPrefab, content);
            monsterDragButton.GetComponent<MonsterDragButton>().Init(i);

            Sprite newSprite = Resources.Load<Sprite>("Textures/MonsterIcon/Monster" + i);
            Image image = monsterDragButton.GetComponent<Image>();
            image.sprite = newSprite;
        }
    }
}
