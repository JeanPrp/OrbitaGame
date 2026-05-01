using UnityEngine;

[CreateAssetMenu(fileName = "DailyMissionConfig", menuName = "Orbita/Daily Mission Config")]
public class DailyMissionConfig : ScriptableObject
{
    [SerializeField] private int targetScore = 150;
    [SerializeField] private int rewardCurrency = 80;

    public int TargetScore => Mathf.Max(1, targetScore);
    public int RewardCurrency => Mathf.Max(0, rewardCurrency);
}
