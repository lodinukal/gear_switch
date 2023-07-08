using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScoreDisplay : MonoBehaviour
{
    [SerializeField]
    private TMPro.TextMeshProUGUI _highScoreText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _highScoreText.text = $"HIGH SCORE: {ScoreStore.GetHighScore()}";
    }
}
