using UnityEngine;

[CreateAssetMenu(fileName = "ObstacleDB", menuName = "ScriptableObjects/Obstacle Database")]
public class ObstacleDB : ScriptableObject {
    #region Serialized Fields
    [SerializeField] private Sprite _obstacleImage = null;
    [SerializeField] private GameObject _obstaclePrefab = null;
    [SerializeField] private float _generationTime = 0;
    [SerializeField] private float _obstacleWeight = 0;
    #endregion

    #region Public Properties
    public Sprite ObstacleImage { get => _obstacleImage; set => _obstacleImage = value; }
    public GameObject ObstaclePrefab { get => _obstaclePrefab; set => _obstaclePrefab = value; }
    public float GenerationTime { get => _generationTime; set => _generationTime = value; }
    public float ObstacleWeight { get => _obstacleWeight; set => _obstacleWeight = value; }
    #endregion
}
