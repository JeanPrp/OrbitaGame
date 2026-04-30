using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CustomizationUIController : MonoBehaviour
{
    [SerializeField] private MainMenuUIController mainMenuUI;
    [SerializeField] private Transform colorsContentRoot;
    [SerializeField] private ColorOptionView colorOptionPrefab;
    [SerializeField] private Image shipPreviewImage;
    [SerializeField] private TMP_Text shipNameLabel;
    [SerializeField] private TMP_Text feedbackLabel;
    [SerializeField] private int colorUnlockPrice = 120;

    private readonly List<ColorOptionView> spawnedColors = new();

    private void OnEnable()
    {
        Rebuild();
    }

    public void Rebuild()
    {
        ClearColorViews();

        if (MetaGameManager.Instance == null) return;

        PlayerProfileData profile = MetaGameManager.Instance.Profile;
        ShipDefinition equippedShip = FindShip(profile.equippedShipId);
        if (equippedShip == null) return;

        if (shipNameLabel != null) shipNameLabel.text = equippedShip.DisplayName;
        if (shipPreviewImage != null) shipPreviewImage.sprite = equippedShip.PreviewSprite;

        Color equippedColor = Color.white;
        ColorUtility.TryParseHtmlString(profile.equippedColorHex, out equippedColor);
        if (shipPreviewImage != null) shipPreviewImage.color = equippedColor;

        if (colorsContentRoot != null && colorOptionPrefab != null)
        {
            foreach (Color color in equippedShip.AvailableColors)
            {
                bool unlocked = profile.unlockedColorHexes.Contains($"#{ColorUtility.ToHtmlStringRGBA(color)}");
                bool isEquipped = profile.equippedColorHex == $"#{ColorUtility.ToHtmlStringRGBA(color)}";

                ColorOptionView view = Instantiate(colorOptionPrefab, colorsContentRoot);
                view.Bind(color, unlocked, isEquipped, colorUnlockPrice, this);
                spawnedColors.Add(view);
            }
        }

        mainMenuUI?.RefreshHeader();
    }

    public void HandleColorAction(Color color)
    {
        if (MetaGameManager.Instance == null) return;

        string key = $"#{ColorUtility.ToHtmlStringRGBA(color)}";
        bool unlocked = MetaGameManager.Instance.Profile.unlockedColorHexes.Contains(key);

        if (!unlocked)
        {
            bool purchased = MetaGameManager.Instance.UnlockColor(color, colorUnlockPrice);
            if (!purchased)
            {
                SetFeedback("Moedas insuficientes para desbloquear cor.");
                mainMenuUI?.RefreshHeader();
                return;
            }
        }

        bool equipped = MetaGameManager.Instance.EquipColor(color);
        SetFeedback(equipped ? "Cor equipada!" : "Não foi possível equipar a cor.");
        Rebuild();
    }

    private ShipDefinition FindShip(string shipId)
    {
        IReadOnlyList<ShipDefinition> ships = MetaGameManager.Instance.GetShipCatalog();
        for (int i = 0; i < ships.Count; i++)
        {
            if (ships[i] != null && ships[i].ShipId == shipId)
                return ships[i];
        }

        return null;
    }

    private void SetFeedback(string message)
    {
        if (feedbackLabel != null)
            feedbackLabel.text = message;
    }

    private void ClearColorViews()
    {
        for (int i = 0; i < spawnedColors.Count; i++)
        {
            if (spawnedColors[i] != null)
                Destroy(spawnedColors[i].gameObject);
        }

        spawnedColors.Clear();
    }
}
