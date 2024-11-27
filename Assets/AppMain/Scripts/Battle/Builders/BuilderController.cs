using UnityEngine;

public class BuilderController : MonoBehaviour {
    private Vector3 _wagonStartPosition = new Vector3(5734, 11, 0);
    private BattleBuilderDB _battleBuilder = null;

    #region Serialized Fields
    [SerializeField] private BuildersDB _buildersDB = null;
    [SerializeField] private SetBaseBlock _setBaseBlock = null;
    [SerializeField] private BattleBuilderUIController _battleBuilderUIController = null;
    #endregion


    #region Public Fields
    // SetBaseblock~~のスクリプトでも使用する変数.
    [HideInInspector] public GameObject Wagon { get; private set; } = null;
    // 走っているwagonのWagonController.
    // BGM, CrusherControllerでも使用する変数.
    [HideInInspector] public WagonController WagonControllerRun { get; private set; } = null;
    // 生成されたPrefabのWagonController.
    [HideInInspector] public WagonController WagonController { get; private set; } = null;
    #endregion

    private void Start() {
        _battleBuilder = _buildersDB.GetBattleBuilder(GameDirector.Instance.BuilderIndex);
        Debug.Log("wagon set");
        SetWagon(_wagonStartPosition);
        GameDirector.Instance.BuilderPosition = 5734.0f;
    }

    private void Update() {
        if (Wagon.transform.position.x < 5060.0f)
            SetWagon(new Vector3(5734, 11, 0));
        
        if (WagonControllerRun != null)
            GameDirector.Instance.BuilderPosition = WagonControllerRun.transform.position.x;
    }

    // ワゴンを配置する座標を受け取りワゴンを配置する.
    private void SetWagon(Vector3 position) {
        Wagon = Instantiate(_battleBuilder.WagonPrefab, position, Quaternion.identity, GameObject.Find("Builder").transform);
        _setBaseBlock.SetSpawnPoint();

        WagonController = Wagon.GetComponent<WagonController>();
        
        _battleBuilderUIController.SetObstaclePlaceable();
    }

    /// <summary>
    /// ワゴンを走らせる.
    /// </summary>
    public void RunWagon(float weight) {
        WagonControllerRun = WagonController;
        // 重量1の分だけ, 速さも1遅くする.
        WagonControllerRun.XSpeed = -WagonControllerRun.Speed + weight;
    }
}
