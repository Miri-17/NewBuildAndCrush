using UnityEngine;

[CreateAssetMenu(fileName = "BuilderDB", menuName = "ScriptableObjects/Builder Database")]
public class BuilderDB : ScriptableObject {
    [SerializeField] private string _name = "";
    [SerializeField] private string _englishName = "";
    [SerializeField] private GameObject _live2D = null;

    public string Name { get => _name; set => _name = value; }
    public string EnglishName { get => _englishName; set => _englishName = value; }
    public GameObject Live2D { get => _live2D; set => _live2D = value; }
}
