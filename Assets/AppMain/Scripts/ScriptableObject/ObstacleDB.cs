using UnityEngine;

[CreateAssetMenu(fileName = "ObstacleDB", menuName = "ScriptableObjects/Obstacle Database")]
public class ObstacleDB : ScriptableObject {
    [SerializeField] private Sprite _obstacleImage = null;
    [SerializeField] private GameObject _obstaclePrefab = null;
    [SerializeField] private float _obstacleWeight = 0;
    [SerializeField] private float _generationTime = 0;

    public Sprite ObstacleImage { get => _obstacleImage; set => _obstacleImage = value; }
    public GameObject ObstaclePrefab { get => _obstaclePrefab; set => _obstaclePrefab = value; }
    public float ObstacleWeight { get => _obstacleWeight; set => _obstacleWeight = value; }
    public float GenerationTime { get => _generationTime; set => _generationTime = value; }
}
