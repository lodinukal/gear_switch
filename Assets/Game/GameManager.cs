using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private VanManager _vanManager;
    [SerializeField] private RoadManager _roadManager;

    [SerializeField] private Ephemeral _eph;
    [SerializeField] private GameObject _itsSoOver;

    public float TravelSpeed = 1f;
    public float Score = 0f;

    // Start is called before the first frame update
    void Start()
    {
        _vanManager.OnGameOver.AddListener(Die);
    }

    // Update is called once per frame
    void Update()
    {
        // Update road speed
        _roadManager.TravelSpeed = TravelSpeed;
        // Update wheel speed
        foreach (var wheel in _vanManager.Van.Wheels)
        {
            wheel.Speed = TravelSpeed / 5;
        }

        AdjustDifficulty();
    }

    void AdjustDifficulty()
    {
        // Adjust difficulty based on time
        Score += Time.deltaTime;
        TravelSpeed = 1f + Score / 10f;
        _roadManager.TimeToNextObstacle = 10f - Score / 50f;
    }

    public void Die() {
        ScoreStore.SetHighScore(Mathf.CeilToInt(Score));
        _eph.Enabled = true;
        _itsSoOver.SetActive(true);
    }
}
