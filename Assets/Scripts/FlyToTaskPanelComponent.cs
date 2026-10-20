using System;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(GameItem))]
public class FlyToTaskPanelComponent : MonoBehaviour
{
    private GameItem _gameItem;
    
    private void Awake()
    {
        _gameItem = GetComponent<GameItem>();
    }

    public void Invoke()
    {
        var targetPosition = Vector3.zero;

        _gameItem.SetSortingOrder(999);

        transform.DOMove(targetPosition, 1f).OnComplete(() =>
        {
            EventManager.GameItemBroken(_gameItem);
            Destroy(_gameItem.gameObject);
        });
    }
    
}
