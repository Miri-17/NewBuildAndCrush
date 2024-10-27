using UnityEngine;

[CreateAssetMenu(fileName = "BuilderInfoDB", menuName = "ScriptableObjects/BuilderInfo Database")]
public class BuilderInfoDB : ScriptableObject {
    [SerializeField] private string _nickname = "";
    [SerializeField, TextArea(1, 10)] private string _description = "";

    public string Nickname { get => _nickname; set => _nickname = value; }
    public string Description { get => _description; set => _description = value; }
}
