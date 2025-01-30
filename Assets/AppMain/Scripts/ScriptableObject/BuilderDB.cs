using UnityEngine;

[CreateAssetMenu(fileName = "BuilderDB", menuName = "ScriptableObjects/Builder Database")]
public class BuilderDB : ScriptableObject {
    #region Serialized Fields
    [SerializeField] private string _name = "";
    [SerializeField] private string _englishName = "";
    [SerializeField] private GameObject _live2D = null;
    #endregion

    #region Public Properties
    public string Name { get => _name; set => _name = value; }
    public string EnglishName { get => _englishName; set => _englishName = value; }
    public GameObject Live2D { get => _live2D; set => _live2D = value; }
    #endregion
}
