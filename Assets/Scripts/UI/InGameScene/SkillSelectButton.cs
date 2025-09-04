using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillSelectButton : MonoBehaviour
{
    private TextMeshProUGUI _skillNameText;
    private TextMeshProUGUI _costText;
    private TextMeshProUGUI _elementText;
    private TextMeshProUGUI _tierText;
    private TextMeshProUGUI _damageText;

    private Image _skillImage;

    public void SetSpellData(Spell spell, string spellName)
    {
        SetChilds();

        _skillNameText.text = spellName;
        _costText.text = "Cost : " + spell.cost.ToString();
        _elementText.text = "Element : " + spell.element;
        _tierText.text = "Tier : " + spell.tier.ToString();
        _damageText.text = "Damage : " + (spell.baseDamage + spell.damageRatio * InGameManager.Instance.Player.Power).ToString();

        Sprite sprite = Resources.Load<Sprite>("Textures/SkillIcon/" + spell.element + "/" + spellName);
        _skillImage.sprite = sprite;
    }

    private void SetChilds()
    {
        if (_skillNameText != null) return;

        _skillNameText = transform.Find("SkillNameText").GetComponent<TextMeshProUGUI>();
        _costText = transform.Find("CostText").GetComponent<TextMeshProUGUI>();
        _elementText = transform.Find("ElementText").GetComponent<TextMeshProUGUI>();
        _tierText = transform.Find("TierText").GetComponent<TextMeshProUGUI>();
        _damageText = transform.Find("DamageText").GetComponent<TextMeshProUGUI>();

        _skillImage = transform.Find("SkillImage").GetComponent<Image>();
    }
}
