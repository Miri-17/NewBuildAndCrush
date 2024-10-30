using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildersDB", menuName = "ScriptableObjects/Builders Database")]
public class BuildersDB : ScriptableObject {
    [SerializeField] private List<BuilderDB> _builderDBs = new List<BuilderDB>();
    [SerializeField] private List<BuilderInfoDB> _builderInfoDBs = new List<BuilderInfoDB>();
    [SerializeField] private List<BattleBuilderDB> _battleBuilderDBs = new List<BattleBuilderDB>();

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

    /// <summary>
    /// Battleで使うビルダーの情報を返す
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public BattleBuilderDB GetBattleBuilder(int index) {
        return _battleBuilderDBs[index];
    }
}
