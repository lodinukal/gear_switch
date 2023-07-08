using UnityEngine;
using Unity.Burst;
using System.Collections;
using System.Collections.Generic;

[BurstCompile]
public class RoadManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _roadPrefab;
    [SerializeField]
    private float _roadLength = 10;
    [SerializeField]
    private int _roadCount = 10;
    private List<RoadPiece> _pieces;

    public float RoadExtent => _roadLength * _pieces.Count;
    public float RoadRange = 2.4f;

    public float TravelSpeed = 1;

    [SerializeField]
    private List<GameObject> _obstacles;
    private List<Obstacle> _spawnedObstacles;

    public float TimeToNextObstacle = 2f;
    private float _obstacleTimer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        _pieces = new List<RoadPiece>();
        _spawnedObstacles = new List<Obstacle>();
        for (int i = 0; i < _roadCount; i++)
        {
            var road = AddRoadPiece();
            road.SetPosition(new Vector3(i * _roadLength, 0, 0));
        }
    }

    // Update is called once per frame
    void Update()
    {
        float delta = Time.deltaTime;
        Cycle(delta);
    }

    RoadPiece AddRoadPiece()
    {
        var road = Instantiate(_roadPrefab, transform.position, Quaternion.identity);
        road.transform.parent = transform;
        var roadPiece = road.GetComponent<RoadPiece>();
        _pieces.Add(roadPiece);

        return roadPiece;
    }

    // Shifts the road pieces by `TravelSpeed` and cycles them back if they go off screen
    void Cycle(float deltaTime)
    {
        float speed = TravelSpeed * deltaTime;
        foreach (var piece in _pieces)
        {
            var position = piece.transform.localPosition;
            position.x -= speed;
            if (position.x < -_roadLength)
            {
                position.x += RoadExtent;
            }
            else if (position.x > RoadExtent)
            {
                position.x -= RoadExtent;
            }
            piece.SetPosition(position);
        }

        // Move obstacles
        // We iterate backwards because we may remove items from the list
        for (var index = _spawnedObstacles.Count - 1; index >= 0; index--)
        {
            var obstacle = _spawnedObstacles[index];
            if (obstacle == null) {
                _spawnedObstacles.RemoveBySwap(index);
                continue;
            }
            var position = obstacle.transform.localPosition;
            position.x -= speed;
            if (position.x < -_roadLength)
            {
                _spawnedObstacles.RemoveBySwap(index);
                Destroy(obstacle.gameObject);
                break;
            }
            obstacle.transform.localPosition = position;
        }
        
        // Spawn obstacles
        _obstacleTimer += deltaTime;
        if (_obstacleTimer > TimeToNextObstacle)
        {
            var obstanceIndex = Random.Range(0, _obstacles.Count);
            var obstacle = Instantiate(_obstacles[obstanceIndex]);
            obstacle.transform.localPosition = new Vector3(RoadExtent, Random.Range(-RoadRange, RoadRange), 0);
            obstacle.transform.parent = transform;
            _spawnedObstacles.Add(obstacle.GetComponent<Obstacle>());
            _obstacleTimer -= TimeToNextObstacle;
        }
    }

    public Vector2 Clamp(Vector2 inp, float inset = 0f) {
        var boundXMin = transform.position.x + inset;
        var boundXMax = transform.position.x + RoadExtent - inset;
        var boundYMin = transform.position.y - RoadRange;
        var boundYMax = transform.position.y + RoadRange;

        return new Vector2(
            Mathf.Clamp(inp.x, boundXMin, boundXMax),
            Mathf.Clamp(inp.y, boundYMin, boundYMax)
        );
    }
}
