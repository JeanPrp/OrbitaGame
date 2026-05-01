using UnityEngine;

[CreateAssetMenu(fileName = "EconomyConfig", menuName = "Orbita/Economy Config")]
public class EconomyConfig : ScriptableObject
{
    [Header("Run rewards")]
    [SerializeField] private float scorePerSecond = 10f;
    [SerializeField] private float currencyPerSecond = 2.5f;

    [Header("Shop")]
    [SerializeField] private int colorUnlockPrice = 120;

    public float ScorePerSecond => Mathf.Max(0f, scorePerSecond);
    public float CurrencyPerSecond => Mathf.Max(0f, currencyPerSecond);
    public int ColorUnlockPrice => Mathf.Max(0, colorUnlockPrice);
}
