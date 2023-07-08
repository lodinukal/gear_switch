using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreStore : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static int GetHighScore() {
        return PlayerPrefs.GetInt("HighScore", 0);
    }

    public static void SetHighScore(int score) {
        PlayerPrefs.SetInt("HighScore", Mathf.Max(GetHighScore(), score));
    }
}
