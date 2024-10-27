using UnityEngine;

[CreateAssetMenu(fileName = "CrusherInfoDB", menuName = "ScriptableObjects/CrusherInfo Database")]
public class CrusherInfoDB : ScriptableObject {
    [SerializeField] private string _nickname = "";
    [SerializeField, TextArea(1, 10)] private string _description = "";

    public string Nickname { get => _nickname; set => _nickname = value; }
    public string Description { get => _description; set => _description = value; }
}
