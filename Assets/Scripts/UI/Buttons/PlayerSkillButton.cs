using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;

public class PlayerSkillButton : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private Player _player;
    private Spell _spell;
    [SerializeField] private Canvas _uiCanvas;

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
        //_spell = Resources.Load<Spell>("Spells/None/FastStrike");
        _spell = Resources.Load<Spell>("Spells/Electric/ThunderStrike");
        LoadSpellData(_spell);
    }

    public void SetPlayer()
    {
        GameObject player = GameObject.FindWithTag("Player");
        _player = player.GetComponent<Player>();
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

    private void SetDamage()
    {
        float damage = 0;

        damage = _baseDamage + _damagePerLevel * _skillLevel + _player.Power * _damageRatio;

        _damage = damage;
    }

    public void OnPointerDown(PointerEventData eventData) // ó�� ��ġ���� ��
    {
        Debug.Log("OnPointerDown");
        _isDragging = true;
        UpdateSpellTilePosition(eventData);
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
        Debug.Log("OnPointerUp");
        if (_isDragging)
        {
            _isDragging = false;
            CastSpell();
        }
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
                TileManager.Instance.SetCurSpellTilePos(tilePos);
                _center = tilePos;
                Debug.Log($"Spell center position updated to: {_center}");
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
        Destroy(effect, 2f);
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
}
