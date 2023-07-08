using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentScore : MonoBehaviour
{
    [SerializeField]
    private GameManager _gameManager;

    [SerializeField]
    private TMPro.TextMeshProUGUI _scoreText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _scoreText.text = $"Score: {_gameManager.Score.ToString("N1")}";
    }
}
