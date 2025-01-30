using UnityEngine;

[CreateAssetMenu(fileName = "BuildersDB", menuName = "ScriptableObjects/Builders Database")]
public class BuildersDB : ScriptableObject {
    #region Serialized Fields
    [SerializeField] private BuilderDB[] _builderDBs = new BuilderDB[0];
    [SerializeField] private BuilderInfoDB[] _builderInfoDBs = new BuilderInfoDB[0];
    [SerializeField] private BattleBuilderDB[] _battleBuilderDBs = new BattleBuilderDB[0];
    #endregion

    /// <summary>
    /// ビルダーの数を返す
    /// </summary>
    public int BuilderLength {
        get { return _builderDBs.Length; }
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
