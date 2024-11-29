using UnityEngine;

[CreateAssetMenu(fileName = "CrushersDB", menuName = "ScriptableObjects/Crushers Database")]
public class CrushersDB : ScriptableObject {
    [SerializeField] private CrusherDB[] _crusherDBs = new CrusherDB[0];
    [SerializeField] private CrusherInfoDB[] _crusherInfoDBs = new CrusherInfoDB[0];
    [SerializeField] private BattleCrusherDB[] _battleCrusherDBs = new BattleCrusherDB[0];
    
    /// <summary>
    /// クラッシャーの数を返す
    /// </summary>
    public int CrusherCount {
        get { return _crusherDBs.Length; }
    }

    /// <summary>
    /// さまざまなシーンで使うクラッシャーの情報を返す
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public CrusherDB GetCrusher(int index) {
        return _crusherDBs[index];
    }

    /// <summary>
    /// PlayerSelectionで使うクラッシャーの情報を返す
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public CrusherInfoDB GetCrusherInfo(int index) {
        return _crusherInfoDBs[index];
    }

    /// <summary>
    /// Battleで使うクラッシャーの情報を返す
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public BattleCrusherDB GetBattleCrusher(int index) {
        return _battleCrusherDBs[index];
    }
}
