using UnityEngine;

[CreateAssetMenu(fileName = "BattleBuilderDB", menuName = "ScriptableObjects/BattleBuilder Database")]
public class BattleBuilderDB : ScriptableObject {
    [SerializeField] private Sprite _bg = null;
    [SerializeField] private Sprite _ground = null;

    public Sprite Bg { get => _bg; set => _bg = value; }
    public Sprite Ground { get => _ground; set => _ground = value; }
}
