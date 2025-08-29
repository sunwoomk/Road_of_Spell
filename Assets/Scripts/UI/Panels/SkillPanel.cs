using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class SkillPanel : MonoBehaviour
{
    private const float SkillButtonSize = 200f;

    [SerializeField] private List<string> _playerSkills = new List<string>();

    private GameObject _skillButtonPrefab;

    private void Start()
    {
        _skillButtonPrefab = Resources.Load<GameObject>("Prefabs/UI/SkillButton");
        SetSkills();
    }


    public void AddSkill(string skillName)
    {
        _playerSkills.Add(skillName);
    }

    public void SetSkills()
    {
        for (int i = 0; i < _playerSkills.Count; i++)
        {
            //List<string> _playerSkills 안에있는 스킬 이름들을 바탕으로 스킬 생성
            string skillName = _playerSkills[i];
            GameObject skillButton = Instantiate(_skillButtonPrefab, transform);
            skillButton.name = skillName;

            // 위치 설정: 왼쪽 정렬, 가로로 SkillButtonSize 간격으로 배치
            RectTransform rt = skillButton.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(-900 + i * SkillButtonSize, 0);

            PlayerSkillButton skillButtonScript = skillButton.GetComponent<PlayerSkillButton>();
            skillButtonScript.Init(skillName);
        }

        GameObject skillRangePanel = GameObject.Find("SkillRangePanel");
        skillRangePanel.SetActive(false);
    }
}
