using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildersDB", menuName = "ScriptableObjects/Builders Database")]
public class BuildersDB : ScriptableObject {
    [SerializeField] private List<BuilderDB> _builderDBs = new List<BuilderDB>();
    [SerializeField] private List<BuilderInfoDB> _builderInfoDBs = new List<BuilderInfoDB>();

    // public List<BuilderDB> BuilderDatabases { get => _builderDBs; set => _builderDBs = value; }
    // public List<BuilderInfoDB> BuilderInfoDBs { get => _builderInfoDBs; set => _builderInfoDBs = value; }

    /// <summary>
    /// ビルダーの数を返す
    /// </summary>
    public int BuilderCount {
        get { return _builderDBs.Count; }
    }

    /// <summary>
    /// さまざまなシーンで使うビルダーの情報を返す
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public BuilderDB GetBuilder(int index) {
        return _builderDBs[index];
    }

    /// <summary>
    /// PlayerSelectionで使うビルダーの情報を返す
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public BuilderInfoDB GetBuilderInfo(int index) {
        return _builderInfoDBs[index];
    }
}
