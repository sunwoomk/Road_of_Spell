using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;

public class PlayerSkillButton : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private const float EffectDuration = 2f;
    private int[] _requiredLevels = { 3, 5, 7 };

    private Player _player;
    private Spell _spell;
    private Canvas _uiCanvas;
    [SerializeField] private Material fadeOverlayMaterial;

    private GameObject _levelIcon;
    private GameObject _costIcon;
    private GameObject _skillLevelUpButton;
    [SerializeField] private GameObject _skillRangePanel;
    private TextMeshProUGUI _levelText;
    private TextMeshProUGUI _costText;
    private Button _button;

    private float _damage;
    private int _skillLevel = 1;
    private List<Vector2Int> _baseRangeOffsets = new List<Vector2Int>();
    private Vector2Int _center = new Vector2Int();
    private List<Vector2Int> _spellHitPositions = new List<Vector2Int>();

    private string _spellName;
    private string _element;
    private int _tier;
    private float _baseDamage;
    private float _damagePerLevel;
    private float _damageRatio;
    private int _cost;
    private string _effectType;
    private List<Spell> _nextSpells = new List<Spell>();

    private bool _isDragging = false;

    private void Update()
    {
        _levelText.text = _skillLevel.ToString();
        _costText.text = _cost.ToString();
    }

    public void OnPointerDown(PointerEventData eventData) // 처음 터치했을 때
    {
        if (_player.CurMana < _cost) return;

        _levelIcon.SetActive(true);
        _costIcon.SetActive(true);
        _skillRangePanel.SetActive(true);
        SkillRangePanel skillRangePanel = _skillRangePanel.GetComponent<SkillRangePanel>();
        skillRangePanel.SetTiles(_baseRangeOffsets);

        _isDragging = true;
        UpdateSpellTilePosition(eventData);
        StartFadeIn();
    }

    public void OnDrag(PointerEventData eventData) //드래그 하는 도중일 때
    {
        if (_isDragging)
        {
            //스펠을 시전할 중심 좌표 업데이트
            UpdateSpellTilePosition(eventData);
        }
    }

    public void OnPointerUp(PointerEventData eventData) //손을 땠을 때
    {
        _levelIcon.SetActive(false);
        _costIcon.SetActive(false);
        _skillRangePanel.SetActive(false);

        if (_isDragging)
        {
            _isDragging = false;
            CastSpell();
            TileManager.Instance.SetSkillPreviewActive(false);
        }

        StartCoroutine(DelayedFadeOut(EffectDuration));

        _player.UseMana(_cost);
    }

    public void Init(string skillName, Player player)
    {
        SetIcons();

        _player = player.GetComponent<Player>();

        _skillRangePanel = InGameManager.Instance.SkillRangePanel;
        _uiCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();

        _spellName = skillName;
        string element = GameDataManager.Instance.AllSkillElements.FirstOrDefault(e => skillName.StartsWith(e));
        _spell = Resources.Load<Spell>("Spells/"+ GameDataManager.Instance.PlayerName + "/" + element + "/" + _spellName);
        LoadSpellData(_spell);
    }

    public void SetIcons()
    {
        _levelIcon = transform.Find("LevelIcon").gameObject;
        _costIcon = transform.Find("CostIcon").gameObject;
        _skillLevelUpButton = transform.Find("SkillLevelUpButton").gameObject;
        InGameManager.Instance.AddSkillLevelUpButton(_skillLevelUpButton);

        _levelText = _levelIcon.transform.Find("LevelText").gameObject.GetComponent<TextMeshProUGUI>();
        _costText = _costIcon.transform.Find("CostText").gameObject.GetComponent<TextMeshProUGUI>();
        _button = _skillLevelUpButton.GetComponent<Button>();
        _button.onClick.AddListener(SkillLevelUp);

        _levelIcon.SetActive(false);
        _costIcon.SetActive(false);
        _skillLevelUpButton.SetActive(false);
    }

    private void LoadSpellData(Spell spell)
    {
        _element = spell.element;
        _tier = spell.tier;
        _baseDamage = spell.baseDamage;
        _damagePerLevel = spell.damagePerLevel;
        _damageRatio = spell.damageRatio;
        _cost = spell.cost;
        _effectType = spell.effectType;
        _nextSpells = spell.nextSpells;

        Sprite newSprite = Resources.Load<Sprite>("Textures/SkillIcon/" + _element + "/" + _spellName);
        Image image = gameObject.GetComponent<Image>();
        image.sprite = newSprite;

        if (spell.range.Length == 0) return;

        _baseRangeOffsets.Clear();
        for(int i = 0; i < spell.range.Length; i++)
        {
            _baseRangeOffsets.Add(spell.range[i]);
        }
    }

    private void SetDamage()
    {
        float damage = 0;

        damage = _baseDamage + _damagePerLevel * (_skillLevel - 1) + _player.Power * _damageRatio;
        //damage = _baseDamage + _damagePerLevel * _skillLevel;

        _damage = damage;
    }

    private void SkillLevelUp()
    {
        _skillLevel += 1;
        Debug.Log("SkillLevelUp!" + _spellName + "SkillLevel : " + _skillLevel);
        _player.UseSkillLevelUpPoint();

        if (_tier >= 1 && _tier <= 3)
        {
            if (_skillLevel == _requiredLevels[_tier - 1] && _nextSpells.Count > 0)
            {
                AddNextSkill();
            }
        }
    }

    private void AddNextSkill()
    {
        InGameSkillPanel inGameSkillPanel = GameObject.Find("InGameSkillPanel").GetComponent<InGameSkillPanel>();
        List<string> spellNames = new List<string>();
        foreach (Spell nextSpell in _nextSpells)
        {
            spellNames.Add(nextSpell.name);
        }
        inGameSkillPanel.SetSkillSelectButton(_nextSpells, spellNames);
    }

    private IEnumerator DelayedFadeOut(float delay)
    {
        yield return new WaitForSeconds(delay);
        StartFadeOut();
    }

    private void UpdateSpellTilePosition(PointerEventData eventData)
    {
        // 화면 좌표를 월드 좌표로 변환
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(eventData.position);
        Vector2 worldPos2D = new Vector2(worldPos.x, worldPos.y);

        //Ray를 쏴서 Collider체크
        Collider2D hitCollider = Physics2D.OverlapPoint(worldPos2D);

        //만약 콜라이더의 태그가 "Tile" 일 때 
        if (hitCollider != null && hitCollider.CompareTag("Tile"))
        {
            Tile tileComp = hitCollider.GetComponent<Tile>();
            if (tileComp != null)
            {
                //스펠을 시전할 중심 좌표를 현재 Ray가 가리키는 타일의 좌표로 변경
                Vector2Int tilePos = tileComp.TilePos;
                TileManager.Instance.UpdateCurSpellTilePos(tilePos, _baseRangeOffsets);
                _center = tilePos;
            }
        }
    }

    private void CastSpell()
    {
        _player.CastSpellAnimation();

        SetDamage();

        _spellHitPositions.Clear();

        //범위가 따로 없다면
        if (_baseRangeOffsets == null || _baseRangeOffsets.Count == 0)
        {
            foreach (Vector2Int hitPos in TileManager.Instance.AllTilePositions())
            {
                //타일 전체 타격
                _spellHitPositions.Add(hitPos);
            }
        }
        else
        {
            foreach (Vector2Int offset in _baseRangeOffsets)
            {
                Vector2Int hitPos = new Vector2Int(_center.x + offset.x, _center.y + offset.y);
                _spellHitPositions.Add(hitPos);
            }
        }

        foreach (Vector2Int pos in _spellHitPositions)
        {
            if (MonsterManager.Instance.Monsters.TryGetValue(pos, out GameObject monster))
            {
                monster.GetComponent<Monster>().TakeDamage(_damage);
            }
        }

        if (_effectType == "Single")
        {
            Vector3 spawnWorldPos = TileManager.Instance.GetTileWorldPosition(_center);
            ShowEffect(spawnWorldPos);
        }
        else if(_effectType == "Multy")
        {
            foreach (Vector2Int pos in _spellHitPositions)
            {
                Vector3 spawnWorldPos = TileManager.Instance.GetTileWorldPosition(pos);
                ShowEffect(spawnWorldPos);
            }
        }
        else if(_effectType == "Global")
        {
            Vector3 spawnWorldPos = Vector3.zero;            
            ShowEffect(spawnWorldPos);
        }
    }

    private void ShowEffect(Vector3 worldPos) // 이펙트 호출 함수
    {
        GameObject effectPrefab = Resources.Load<GameObject>("Prefabs/Effects/" + _element + "/" + _spellName);

        Vector2 canvasPos = WorldToCanvasPosition(worldPos); // 월드좌표를 캔버스좌표로 변환
        GameObject effect = Instantiate(effectPrefab, _uiCanvas.transform);
        effect.GetComponent<RectTransform>().anchoredPosition = canvasPos;
        Destroy(effect, EffectDuration);
    }


    private Vector2 WorldToCanvasPosition(Vector3 worldPos) // 월드좌표를 캔버스좌표로 변환
    {
        Canvas canvas = _uiCanvas;
        Vector2 screenPoint = Camera.main.WorldToScreenPoint(worldPos);
        RectTransform canvasRect = canvas.GetComponent<RectTransform>();
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            screenPoint,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : Camera.main,
            out localPoint);
        return localPoint;
    }

    private IEnumerator FadeOverlay(float from, float to, float duration)
    {
        float elapsed = 0f;
        Color color = fadeOverlayMaterial.GetColor("_Color");

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(from, to, elapsed / duration);
            color.a = alpha;
            fadeOverlayMaterial.SetColor("_Color", color);
            yield return null;
        }
        color.a = to;
        fadeOverlayMaterial.SetColor("_Color", color);
    }

    public void StartFadeIn()
    {
        StartCoroutine(FadeOverlay(0f, 0.5f, 0.3f));
    }

    public void StartFadeOut()
    {
        StartCoroutine(FadeOverlay(0.5f, 0f, 0.3f));
    }
}
