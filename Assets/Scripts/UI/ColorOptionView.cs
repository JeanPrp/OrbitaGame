using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ColorOptionView : MonoBehaviour
{
    [SerializeField] private Image swatch;
    [SerializeField] private TMP_Text statusText;
    [SerializeField] private Button actionButton;

    private Color color;
    private CustomizationUIController owner;

    public void Bind(Color c, bool unlocked, bool equipped, int unlockPrice, CustomizationUIController controller)
    {
        color = c;
        owner = controller;

        if (swatch != null)
            swatch.color = c;

        if (statusText != null)
        {
            if (equipped) statusText.text = "Equipada";
            else if (unlocked) statusText.text = "Desbloqueada";
            else statusText.text = $"{unlockPrice} moedas";
        }

        if (actionButton != null)
        {
            actionButton.onClick.RemoveAllListeners();
            actionButton.onClick.AddListener(OnClickAction);
        }
    }

    private void OnClickAction()
    {
        owner?.HandleColorAction(color);
    }
}
