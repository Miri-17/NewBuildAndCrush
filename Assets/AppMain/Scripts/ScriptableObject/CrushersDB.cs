using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CrushersDB", menuName = "ScriptableObjects/Crushers Database")]
public class CrushersDB : ScriptableObject {
    [SerializeField] private List<CrusherDB> _crusherDBs = new List<CrusherDB>();
    [SerializeField] private List<CrusherInfoDB> _crusherInfoDBs = new List<CrusherInfoDB>();
    [SerializeField] private List<BattleCrusherDB> _battleCrusherDBs = new List<BattleCrusherDB>();
    
    /// <summary>
    /// クラッシャーの数を返す
    /// </summary>
    public int CrusherCount {
        get { return _crusherDBs.Count; }
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
