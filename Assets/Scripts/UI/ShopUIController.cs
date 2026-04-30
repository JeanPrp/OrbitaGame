using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopUIController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private MainMenuUIController mainMenuUI;
    [SerializeField] private Transform contentRoot;
    [SerializeField] private ShopItemView shopItemPrefab;
    [SerializeField] private TMP_Text feedbackLabel;

    private readonly List<ShopItemView> spawnedItems = new();

    private void OnEnable()
    {
        Rebuild();
    }

    public void Rebuild()
    {
        ClearItems();

        if (MetaGameManager.Instance == null || contentRoot == null || shopItemPrefab == null) return;

        PlayerProfileData profile = MetaGameManager.Instance.Profile;
        IReadOnlyList<ShipDefinition> ships = MetaGameManager.Instance.GetShipCatalog();

        foreach (ShipDefinition ship in ships)
        {
            if (ship == null) continue;

            bool unlocked = profile.unlockedShipIds.Contains(ship.ShipId);
            bool equipped = profile.equippedShipId == ship.ShipId;

            ShopItemView view = Instantiate(shopItemPrefab, contentRoot);
            view.Bind(ship, unlocked, equipped, this);
            spawnedItems.Add(view);
        }

        mainMenuUI?.RefreshHeader();
    }

    public void HandleShipAction(string shipId)
    {
        if (MetaGameManager.Instance == null) return;

        PlayerProfileData profile = MetaGameManager.Instance.Profile;
        bool unlocked = profile.unlockedShipIds.Contains(shipId);

        if (!unlocked)
        {
            bool purchased = MetaGameManager.Instance.TryUnlockShip(shipId);
            SetFeedback(purchased ? "Nave comprada com sucesso!" : "Moedas insuficientes.");

            if (!purchased)
            {
                mainMenuUI?.RefreshHeader();
                return;
            }
        }

        bool equipped = MetaGameManager.Instance.EquipShip(shipId);
        SetFeedback(equipped ? "Nave equipada!" : "Não foi possível equipar.");

        Rebuild();
    }

    private void SetFeedback(string message)
    {
        if (feedbackLabel != null)
            feedbackLabel.text = message;
    }

    private void ClearItems()
    {
        for (int i = 0; i < spawnedItems.Count; i++)
        {
            if (spawnedItems[i] != null)
                Destroy(spawnedItems[i].gameObject);
        }

        spawnedItems.Clear();
    }
}
