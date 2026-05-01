using System;
using UnityEngine;

public static class DailyMissionService
{
    private const string DefaultPath = "GameData/DailyMissionConfig";

    private static DailyMissionConfig cachedConfig;

    public static DailyMissionConfig GetConfig()
    {
        if (cachedConfig != null) return cachedConfig;
        cachedConfig = Resources.Load<DailyMissionConfig>(DefaultPath);
        return cachedConfig;
    }

    public static void EnsureDailyState(PlayerProfileData profile)
    {
        if (profile == null) return;

        string today = DateTime.UtcNow.ToString("yyyy-MM-dd");
        if (profile.missionDateIsoUtc == today) return;

        profile.missionDateIsoUtc = today;
        profile.missionProgressScore = 0;
        profile.missionClaimed = false;
    }

    public static void RegisterRun(PlayerProfileData profile, int runScore)
    {
        if (profile == null) return;

        EnsureDailyState(profile);
        profile.missionProgressScore = Mathf.Max(profile.missionProgressScore, runScore);
    }

    public static bool CanClaim(PlayerProfileData profile)
    {
        DailyMissionConfig cfg = GetConfig();
        if (cfg == null || profile == null) return false;

        EnsureDailyState(profile);
        return !profile.missionClaimed && profile.missionProgressScore >= cfg.TargetScore;
    }

    public static bool TryClaim(PlayerProfileData profile, out int reward)
    {
        reward = 0;
        DailyMissionConfig cfg = GetConfig();
        if (cfg == null || profile == null) return false;

        if (!CanClaim(profile)) return false;

        profile.missionClaimed = true;
        reward = cfg.RewardCurrency;
        return true;
    }
}
