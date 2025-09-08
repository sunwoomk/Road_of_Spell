using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;

public class SkillSetUpPanel : MonoBehaviour
{
    private GameObject _skillImagePrefab;
    private List<Image> _skillImages = new List<Image>();

    private void Start()
    {
        SetSkillImages();
    }

    private void SetSkillImages()
    {
        _skillImagePrefab = Resources.Load<GameObject>("Prefabs/UI/SkillImage");
        for(int i = 0; i < 2; i++)
        {
            GameObject skillImage = Instantiate(_skillImagePrefab, transform);
            RectTransform rectTransform = skillImage.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(0, 100 - i * 300);
            _skillImages.Add(skillImage.GetComponent<Image>());
        }
    }

    public void SetClassSkillImage(string playerName)
    {
        GameDataManager.Instance.SkillNames.Clear();

        //�÷��̾� �̸����� �Ӽ� ��������
        List<string> elements = GameDataManager.Instance.PlayerElementPairs[playerName];

        for (int i = 0; i < elements.Count; i++)
        {
            string element = elements[i];
            string spellPath = "Spells/" + playerName + "/" + element + "/";

            //�ش� ��ο� �ִ� ���� ���� ��������
            Spell[] spells = Resources.LoadAll<Spell>(spellPath);

            //Ƽ�� 1�� ���� ã�Ƽ� SkillNames�� �߰�
            Spell spell = System.Array.Find(spells, spell => spell.tier == 1);
            string skillName = spell.name;
            GameDataManager.Instance.SkillNames.Add(skillName);

            //��ų ������ ����
            string iconPath = "Textures/SkillIcon/" + element + "/" + skillName;
            _skillImages[i].sprite = Resources.Load<Sprite>(iconPath);
        }
    }
}
