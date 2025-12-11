using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class TowerCard : MonoBehaviour
{
    [SerializeField] private Image towerImage;
    [SerializeField] private TMP_Text costText;

    private BaseTowerData _data;

    public static event Action<BaseTowerData> OnTowerSelected;

    public void Initialize(BaseTowerData data)
    {
        _data = data;
        towerImage.sprite = data.sprite;
        costText.text = data.cost.ToString();
    }

    public void PlaceTower()
    {
        OnTowerSelected?.Invoke(_data);
    }
}