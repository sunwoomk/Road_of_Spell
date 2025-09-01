using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class SkillPanel : MonoBehaviour
{
    private const float SkillButtonSize = 200f;
    private const float SkillSelectButtonSpacing = 250f;
    private const int MaxSkillSelectButtons = 2;

    private List<string> _playerSkills = new List<string>();
    private List<GameObject> _skillSelectButtons = new List<GameObject>(); 

    private GameObject _skillButtonPrefab;
    private GameObject _skillSelectButtonPrefab;
    private Player _player;

    public void AddSkill(string skillName)
    {
        GameObject skillButton = Instantiate(_skillButtonPrefab, transform);
        skillButton.name = skillName;

        RectTransform rt = skillButton.GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector2(-900 + _playerSkills.Count * SkillButtonSize, 0);

        PlayerSkillButton skillButtonScript = skillButton.GetComponent<PlayerSkillButton>();
        skillButtonScript.Init(skillName, _player);

        _playerSkills.Add(skillName);

        foreach (GameObject button in _skillSelectButtons)
        {
            button.SetActive(false);
        }
    }

    public void SetSkills(List<string> playerSkills, Player player)
    {
        _playerSkills = playerSkills;
        _player = player;

        _skillButtonPrefab = Resources.Load<GameObject>("Prefabs/UI/SkillButton");
        _skillSelectButtonPrefab = Resources.Load<GameObject>("Prefabs/UI/SkillSelectButton");

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
            skillButtonScript.Init(skillName, _player);
        }

        GameObject skillRangePanel = GameObject.Find("SkillRangePanel");
        skillRangePanel.SetActive(false);
    }

    public void CreateSkillSelectButton()
    {
        for(int i = 0; i < MaxSkillSelectButtons; i++)
        {
            GameObject skillSelectButton = Instantiate(_skillSelectButtonPrefab, InGameManager.Instance.Canvas.transform);
            _skillSelectButtons.Add(skillSelectButton);

            //float x = SkillSelectButtonSpacing * (i * 2 - 1);
            //RectTransform rectTransform = skillSelectButton.GetComponent<RectTransform>();
            //rectTransform.anchoredPosition = new Vector2(x, 0);

            skillSelectButton.SetActive(false);
        }
    }

    public void SetSkillSelectButton(List<Spell> nextSpells, List<string> spellNames)
    {
        if (nextSpells.Count == 1)
        {
            SetSkillData(nextSpells, spellNames, 0);
            _skillSelectButtons[0].GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 140);
        }

        else if (nextSpells.Count == 2)
        {
            for (int i = 0; i < nextSpells.Count; i++)
            {
                SetSkillData(nextSpells, spellNames, i);
                _skillSelectButtons[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(SkillSelectButtonSpacing * (i * 2 - 1), 140);
            }
        }
    }

    //private void DeactivateSkillSelectButtons()
    //{
    //    foreach (GameObject button in _skillSelectButtons)
    //    {
    //        button.SetActive(false);
    //    }
    //}

    private void SetSkillData(List<Spell> nextSpells, List<string> spellNames, int index)
    {
        _skillSelectButtons[index].GetComponent<SkillSelectButton>().SetSpellData(nextSpells[index], spellNames[index]);
        _skillSelectButtons[index].SetActive(true);
        _skillSelectButtons[index].GetComponent<Button>().onClick.RemoveAllListeners();
        _skillSelectButtons[index].GetComponent<Button>().onClick.AddListener(() => AddSkill(spellNames[index]));
    }
}
