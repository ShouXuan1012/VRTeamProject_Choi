using UnityEngine;

public static class LocalHighScore
{
    private const string Key = "BestScore";

    public static int BestScore
    {
        get => PlayerPrefs.GetInt(Key, 0);
        set
        {
            PlayerPrefs.SetInt(Key, value);
            PlayerPrefs.Save();
        }
    }

    public static bool UpdateIfHigher(int score)
    {
        if (score > BestScore)
        {
            BestScore = score;
            return true;
        }
        return false;
    }
}
