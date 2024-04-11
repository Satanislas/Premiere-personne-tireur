using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    public static SaveLoadManager instance;

    private string highScoreKey = "HighScoreSavedValue";
    
    private void Start()
    {
        if (instance is not null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
        
        DontDestroyOnLoad(this);
    }

    public int LoadHighScore()
    {
        return PlayerPrefs.HasKey(highScoreKey) ? PlayerPrefs.GetInt(highScoreKey) : 0;
    }
    public void SaveHighScore(int score)
    {
        PlayerPrefs.SetInt(highScoreKey,score);
    }
}
