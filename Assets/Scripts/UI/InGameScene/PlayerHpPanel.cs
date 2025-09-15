using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class PlayerHpPanel : MonoBehaviour
{
    private const float StartPosX = -100f;
    private const float PlayerHpWidth = 50f;

    private Player _player;

    private GameObject _deactivePlayerHpPrefab;
    private GameObject _activePlayerHpPrefab;

    private List<GameObject> _deactivePlayerHp = new List<GameObject>();
    private List<GameObject> _activePlayerHp = new List<GameObject>();

    private void Start()
    {
        _deactivePlayerHpPrefab = Resources.Load<GameObject>("Prefabs/UI/DeactivePlayerHp");
        _activePlayerHpPrefab = Resources.Load<GameObject>("Prefabs/UI/ActivePlayerHp");
    }

    private void Update()
    {
        //충분히 업데이트 호출 안하고 구현 가능
        for (int i = 0; i < _activePlayerHp.Count; i++)
        {
            _activePlayerHp[i].SetActive(i < _player.CurHp);
        }
    }

    public void SetPlayer(Player player)
    {
        _player = player;
    }

    public void SetPlayerHp()
    {
        for(int i = 0; i < _player.MaxHp; i++)
        {
            CreatePlayerHp(_deactivePlayerHpPrefab, _deactivePlayerHp, i);
            CreatePlayerHp(_activePlayerHpPrefab, _activePlayerHp, i);
        }
    }

    private void CreatePlayerHp(GameObject playerHpPrefab, List<GameObject> playerHpList, int index)
    {
        GameObject playerHp = Instantiate(playerHpPrefab, transform);
        RectTransform rectTransform = playerHp.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(StartPosX + PlayerHpWidth * index, 0);
        playerHpList.Add(playerHp);
    }
}
