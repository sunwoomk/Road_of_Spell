using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class ManaPanel : MonoBehaviour
{
    private const float StartPosX = -67f;
    private const float ManaCrystalWidth = 26f;
    private const int MaxManaCrystal = 10;

    private GameObject _deactiveManaCrystalPrefab;
    private GameObject _activeManaCrystalPrefab;
    private Player _player;
    private TextMeshProUGUI _manaText;

    private List<GameObject> _deactiveManaCrystals = new List<GameObject>();
    private List<GameObject> _activeManaCrystals = new List<GameObject>();

    private void Start()
    {
        _deactiveManaCrystalPrefab = Resources.Load<GameObject>("Prefabs/UI/DeactiveManaCrystal");
        _activeManaCrystalPrefab = Resources.Load<GameObject>("Prefabs/UI/ActiveManaCrystal");
        _manaText = transform.Find("ManaText").GetComponent<TextMeshProUGUI>();
        SetManaCrystals();
    }

    private void Update()
    {
        _manaText.text = _player.CurMana + " / " + _player.MaxMana;

        for (int i = 0; i < _deactiveManaCrystals.Count; i++)
        {
            _deactiveManaCrystals[i].SetActive(i < _player.MaxMana);
        }

        for (int i = 0; i < _activeManaCrystals.Count; i++)
        {
            _activeManaCrystals[i].SetActive(i < _player.CurMana);
        }
    }

    public void SetPlayer(Player player)
    {
        _player = player;
    }

    private void SetManaCrystals()
    {
        for (int i = 0; i < MaxManaCrystal; i++)
        {
            CreateManaCrystal(_deactiveManaCrystalPrefab, _deactiveManaCrystals, i);
            CreateManaCrystal(_activeManaCrystalPrefab, _activeManaCrystals, i);
        }
    }

    private void CreateManaCrystal(GameObject manaCrystalPrefab, List<GameObject> manaCrystals, int index)
    {
        GameObject deactiveManaCrystal = Instantiate(manaCrystalPrefab, transform);
        RectTransform rectTransform = deactiveManaCrystal.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(StartPosX + ManaCrystalWidth * index, 0);
        manaCrystals.Add(deactiveManaCrystal);
    }
}
