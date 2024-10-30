using UnityEngine;

[CreateAssetMenu(fileName = "BattleCrusherDB", menuName = "ScriptableObjects/BattleCrusher Database")]
public class BattleCrusherDB : ScriptableObject {
    [SerializeField] private GameObject _crusherPrefab = null;

    public GameObject CrusherPrefab { get => _crusherPrefab; set => _crusherPrefab = value; }
}
