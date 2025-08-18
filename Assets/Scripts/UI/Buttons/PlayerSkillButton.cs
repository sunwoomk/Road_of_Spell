using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerSkillButton : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private Player _player;
    private Spell _spell;
    private Canvas _uiCanvas;

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
        _spell = Resources.Load<Spell>("Spells/None/FastStrike");
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

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("OnPointerDown");
        _isDragging = true;
        UpdateSpellTilePosition(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("OnDrag");
        if (_isDragging)
        {
            UpdateSpellTilePosition(eventData);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
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
        Vector3 screenPos = eventData.position;
        screenPos.z = Mathf.Abs(Camera.main.transform.position.z);
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(eventData.position);
        Vector2 worldPos2D = new Vector2(worldPos.x, worldPos.y);

        Collider2D hitCollider = Physics2D.OverlapPoint(worldPos2D);
        if (hitCollider != null && hitCollider.CompareTag("Tile"))
        {
            Tile tileComp = hitCollider.GetComponent<Tile>();
            if (tileComp != null)
            {
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

        foreach (Vector2Int pos in _spellHitPositions)
        {
            Vector3 spawnWorldPos = TileManager.Instance.GetTileWorldPosition(pos);
            ShowEffect(spawnWorldPos);
            Debug.Log($"Casting spell at position: {pos} with damage: {_damage} and cost: {_cost}");
        }
    }

    private void ShowEffect(Vector3 position)
    {
        string effectName = "FastStrike";
        GameObject effectPrefab = Resources.Load<GameObject>("Prefabs/Effects/" + effectName);
        if (effectPrefab == null)
        {
            Debug.LogError($"Effect prefab '{effectName}' not found!");
            return;
        }
        if (effectPrefab != null)
        {
            //Debug.Log($"Spawning effect at position: {position}");
            GameObject effect = Instantiate(effectPrefab, position, Quaternion.identity);
            Destroy(effect, 2f);
        }
    }
}
