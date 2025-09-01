using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class SkillPanel : MonoBehaviour
{
    private const float SkillButtonSize = 200f;

    private List<string> _playerSkills = new List<string>();

    private GameObject _skillButtonPrefab;

    public void AddSkill(string skillName)
    {
        _playerSkills.Add(skillName);
    }

    public void SetSkills(List<string> playerSkills, Player player)
    {
        _playerSkills = playerSkills;

        _skillButtonPrefab = Resources.Load<GameObject>("Prefabs/UI/SkillButton");

        for (int i = 0; i < _playerSkills.Count; i++)
        {
            //List<string> _playerSkills �ȿ��ִ� ��ų �̸����� �������� ��ų ����
            string skillName = _playerSkills[i];
            GameObject skillButton = Instantiate(_skillButtonPrefab, transform);
            skillButton.name = skillName;

            // ��ġ ����: ���� ����, ���η� SkillButtonSize �������� ��ġ
            RectTransform rt = skillButton.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(-900 + i * SkillButtonSize, 0);

            PlayerSkillButton skillButtonScript = skillButton.GetComponent<PlayerSkillButton>();
            skillButtonScript.Init(skillName, player);
        }

        GameObject skillRangePanel = GameObject.Find("SkillRangePanel");
        skillRangePanel.SetActive(false);
    }
}
