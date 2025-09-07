using UnityEngine;
using UnityEngine.UI;

public class SkillSetUpPanel : MonoBehaviour
{
    private Image _classSkillImage;
    private Image _commonSkillImage;

    private void Start()
    {
        _classSkillImage = GameObject.Find("ClassSkillImage").GetComponent<Image>();
        _commonSkillImage = GameObject.Find("CommonSkillImage").GetComponent<Image>();
    }

    public void SetClassSkillImage(string element, string skillName)
    {
        _classSkillImage.sprite = Resources.Load<Sprite>("Textures/SkillIcon/" + element + "/" + skillName);
    }
}
