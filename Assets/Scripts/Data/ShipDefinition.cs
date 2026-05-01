using System;
using UnityEngine;

[CreateAssetMenu(fileName = "ShipDefinition", menuName = "Orbita/Ship Definition")]
public class ShipDefinition : ScriptableObject
{
    [Header("Identity")]
    [SerializeField] private string shipId = "ship_default";
    [SerializeField] private string displayName = "Default Ship";

    [Header("Economy")]
    [SerializeField] private int unlockPrice = 0;

    [Header("Preview")]
    [SerializeField] private Sprite previewSprite;
    [SerializeField] private Color[] availableColors = { Color.white };

    public string ShipId => shipId;
    public string DisplayName => displayName;
    public int UnlockPrice => Mathf.Max(0, unlockPrice);
    public Sprite PreviewSprite => previewSprite;
    public Color[] AvailableColors => availableColors ?? Array.Empty<Color>();
}
