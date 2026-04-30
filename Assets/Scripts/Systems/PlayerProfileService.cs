using System;
using System.IO;
using UnityEngine;

public static class PlayerProfileService
{
    private const string SaveFileName = "player_profile.json";
    private static readonly string SavePath = Path.Combine(Application.persistentDataPath, SaveFileName);

    public static PlayerProfileData LoadOrCreate()
    {
        try
        {
            if (!File.Exists(SavePath))
            {
                PlayerProfileData created = new PlayerProfileData();
                Save(created);
                return created;
            }

            string json = File.ReadAllText(SavePath);
            if (string.IsNullOrWhiteSpace(json))
            {
                PlayerProfileData fallback = new PlayerProfileData();
                Save(fallback);
                return fallback;
            }

            PlayerProfileData data = JsonUtility.FromJson<PlayerProfileData>(json);
            if (data == null)
            {
                data = new PlayerProfileData();
                Save(data);
            }

            return data;
        }
        catch (Exception ex)
        {
            Debug.LogWarning($"[PlayerProfileService] Failed to load profile. Creating default. Reason: {ex.Message}");
            return new PlayerProfileData();
        }
    }

    public static void Save(PlayerProfileData data)
    {
        if (data == null) return;

        try
        {
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(SavePath, json);
        }
        catch (Exception ex)
        {
            Debug.LogError($"[PlayerProfileService] Failed to save profile. Reason: {ex.Message}");
        }
    }
}
