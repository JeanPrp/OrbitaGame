using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PlayerShipVisualApplier : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        Apply();
    }

    public void Apply()
    {
        if (MetaGameManager.Instance == null || spriteRenderer == null) return;

        PlayerProfileData profile = MetaGameManager.Instance.Profile;
        ShipDefinition ship = FindShip(profile.equippedShipId);
        if (ship != null && ship.PreviewSprite != null)
            spriteRenderer.sprite = ship.PreviewSprite;

        if (ColorUtility.TryParseHtmlString(profile.equippedColorHex, out Color color))
            spriteRenderer.color = color;
    }

    private ShipDefinition FindShip(string shipId)
    {
        var catalog = MetaGameManager.Instance.GetShipCatalog();
        for (int i = 0; i < catalog.Count; i++)
        {
            if (catalog[i] != null && catalog[i].ShipId == shipId)
                return catalog[i];
        }

        return null;
    }
}
