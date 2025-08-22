using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;

public class PlayerSkillButton : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private const float EffectDuration = 2f;

    private Player _player;
    private Spell _spell;
    [SerializeField] private Canvas _uiCanvas;
    [SerializeField] private Material fadeOverlayMaterial;

    private GameObject _levelIcon;
    private GameObject _costIcon;
    private GameObject _skillRangePanel;
    private TextMeshProUGUI _levelText;
    private TextMeshProUGUI _costText;

    private float _damage;
    private int _skillLevel = 0;
    private List<Vector2Int> _baseRangeOffsets = new List<Vector2Int>();
    private Vector2Int _center = new Vector2Int();
    private List<Vector2Int> _spellHitPositions = new List<Vector2Int>();

    private string _element;
    private int _tier;
    private float _baseDamage;
    private float _damagePerLevel;
    private float _damageRatio;
    private int _cost;

    private bool _isDragging = false;

    private void Start()
    {
        SetIcons();

        //_spell = Resources.Load<Spell>("Spells/None/FastStrike");
        _spell = Resources.Load<Spell>("Spells/Electric/ThunderStrike");
        LoadSpellData(_spell);

        SetSkillRangePanel();
    }

    private void Update()
    {
        _levelText.text = _skillLevel.ToString();
        _costText.text = _cost.ToString();
    }

    public void SetPlayer()
    {
        GameObject player = GameObject.FindWithTag("Player");
        _player = player.GetComponent<Player>();
    }

    public void SetIcons()
    {
        _levelIcon = transform.Find("LevelIcon").gameObject;
        _costIcon = transform.Find("CostIcon").gameObject;

        _levelText = _levelIcon.transform.Find("LevelText").gameObject.GetComponent<TextMeshProUGUI>();
        _costText = _costIcon.transform.Find("CostText").gameObject.GetComponent<TextMeshProUGUI>();

        _levelIcon.SetActive(false);
        _costIcon.SetActive(false);
    }

    private void LoadSpellData(Spell spell)
    {
        _element = spell.element;
        _tier = spell.tier;
        _baseDamage = spell.baseDamage;
        _damagePerLevel = spell.damagePerLevel;
        _damageRatio = spell.damageRatio;
        _cost = spell.cost;

        _baseRangeOffsets.Clear();
        for(int i = 0; i < spell.range.Length; i++)
        {
            _baseRangeOffsets.Add(spell.range[i]);
        }
    }

    private void SetSkillRangePanel()
    {
        _skillRangePanel = transform.Find("SkillRangePanel").gameObject;
        SkillRangePanel skillRangePanel = _skillRangePanel.GetComponent<SkillRangePanel>();
        skillRangePanel.SetTiles(_baseRangeOffsets);

        _skillRangePanel.SetActive(false);
    }

    private void SetDamage()
    {
        float damage = 0;

        damage = _baseDamage + _damagePerLevel * _skillLevel + _player.Power * _damageRatio;

        _damage = damage;
    }

    public void OnPointerDown(PointerEventData eventData) // ó�� ��ġ���� ��
    {
        _levelIcon.SetActive(true);
        _costIcon.SetActive(true);
        _skillRangePanel.SetActive(true);

        Debug.Log("OnPointerDown");
        _isDragging = true;
        UpdateSpellTilePosition(eventData);
        StartFadeIn();
    }

    public void OnDrag(PointerEventData eventData) //�巡�� �ϴ� ������ ��
    {
        Debug.Log("OnDrag");
        if (_isDragging)
        {
            //������ ������ �߽� ��ǥ ������Ʈ
            UpdateSpellTilePosition(eventData);
        }
    }

    public void OnPointerUp(PointerEventData eventData) //���� ���� ��
    {
        _levelIcon.SetActive(false);
        _costIcon.SetActive(false);
        _skillRangePanel.SetActive(false);

        Debug.Log("OnPointerUp");
        if (_isDragging)
        {
            _isDragging = false;
            CastSpell();
            TileManager.Instance.SetSkillPreviewActive(false);
        }

        StartCoroutine(DelayedFadeOut(EffectDuration));
    }

    private IEnumerator DelayedFadeOut(float delay)
    {
        yield return new WaitForSeconds(delay);
        StartFadeOut();
    }

    private void UpdateSpellTilePosition(PointerEventData eventData)
    {
        // ȭ�� ��ǥ�� ���� ��ǥ�� ��ȯ
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(eventData.position);
        Vector2 worldPos2D = new Vector2(worldPos.x, worldPos.y);

        //Ray�� ���� Colliderüũ
        Collider2D hitCollider = Physics2D.OverlapPoint(worldPos2D);

        //���� �ݶ��̴��� �±װ� "Tile" �� �� 
        if (hitCollider != null && hitCollider.CompareTag("Tile"))
        {
            Tile tileComp = hitCollider.GetComponent<Tile>();
            if (tileComp != null)
            {
                //������ ������ �߽� ��ǥ�� ���� Ray�� ����Ű�� Ÿ���� ��ǥ�� ����
                Vector2Int tilePos = tileComp.TilePos;
                TileManager.Instance.UpdateCurSpellTilePos(tilePos, _baseRangeOffsets);
                _center = tilePos;
            }
        }
    }

    private void CastSpell()
    {
        //SetDamage();

        _spellHitPositions.Clear();
        foreach (Vector2Int offset in _baseRangeOffsets)
        {
            Vector2Int hitPos = new Vector2Int(_center.x + offset.x, _center.y + offset.y);
            _spellHitPositions.Add(hitPos);
        }

        if(_spell.effectType == "Single")
        {
            Vector3 spawnWorldPos = TileManager.Instance.GetTileWorldPosition(_center);
            ShowEffect(spawnWorldPos);
        }
        else if(_spell.effectType == "Multy")
        {
            foreach (Vector2Int pos in _spellHitPositions)
            {
                Vector3 spawnWorldPos = TileManager.Instance.GetTileWorldPosition(pos);
                ShowEffect(spawnWorldPos);
            }
        }
    }

    private void ShowEffect(Vector3 worldPos) // ����Ʈ ȣ�� �Լ�
    {
        string effectName = "ThunderStrike";
        //string effectName = "FastStrike";
        GameObject effectPrefab = Resources.Load<GameObject>("Prefabs/Effects/Electric/" + effectName);
        //GameObject effectPrefab = Resources.Load<GameObject>("Prefabs/Effects/None/" + effectName);

        Vector2 canvasPos = WorldToCanvasPosition(worldPos); // ������ǥ�� ĵ������ǥ�� ��ȯ
        GameObject effect = Instantiate(effectPrefab, _uiCanvas.transform);
        effect.GetComponent<RectTransform>().anchoredPosition = canvasPos;
        Destroy(effect, EffectDuration);
    }


    private Vector2 WorldToCanvasPosition(Vector3 worldPos) // ������ǥ�� ĵ������ǥ�� ��ȯ
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
