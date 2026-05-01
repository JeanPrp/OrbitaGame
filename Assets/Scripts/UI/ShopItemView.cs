using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemView : MonoBehaviour
{
    [SerializeField] private TMP_Text shipNameLabel;
    [SerializeField] private TMP_Text priceOrStatusLabel;
    [SerializeField] private Image previewImage;
    [SerializeField] private Button actionButton;

    private string shipId;
    private ShopUIController owner;

    public void Bind(ShipDefinition ship, bool unlocked, bool equipped, ShopUIController controller)
    {
        shipId = ship.ShipId;
        owner = controller;

        if (shipNameLabel != null) shipNameLabel.text = ship.DisplayName;
        if (previewImage != null) previewImage.sprite = ship.PreviewSprite;

        if (priceOrStatusLabel != null)
        {
            if (equipped) priceOrStatusLabel.text = "Equipada";
            else if (unlocked) priceOrStatusLabel.text = "Desbloqueada";
            else priceOrStatusLabel.text = $"{ship.UnlockPrice} moedas";
        }

        if (actionButton != null)
        {
            actionButton.onClick.RemoveAllListeners();
            actionButton.onClick.AddListener(OnAction);
        }
    }

    private void OnAction()
    {
        owner?.HandleShipAction(shipId);
    }
}
