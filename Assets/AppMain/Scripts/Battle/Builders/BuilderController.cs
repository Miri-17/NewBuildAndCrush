using UnityEngine;
using UnityEngine.UI;

public class BuilderController : MonoBehaviour {
    private Vector3 _wagonStartPosition = new Vector3(5734, 11, 0);

    #region Serialized Fields
    [SerializeField] private BuildersDB _buildersDB = null;
    [SerializeField] private SetBaseBlock _setBaseBlock = null;
    [SerializeField] private BattleBuilderUIController _battleBuilderUIController = null;
    // [Header("ワゴン爆発エフェクト"), SerializeField]
    // private GameObject explosionEffect;
    // [SerializeField] private Clicker clicker;
    // [SerializeField]
    // private Image fillWeighingImage;
    #endregion


    #region Public Fields
    public BattleBuilderDB BattleBuilder { get; private set; } = null;


    // ObstacleButtonManagerでも使用する変数.
    // [HideInInspector] public bool[] isSetWagon = { false, false, false, false, false, false, };
    [HideInInspector] public bool[] isRunningWagon = { false, false, false, false, false, false, };
    // SetBaseblock~~のスクリプトでも使用する変数.
    [HideInInspector] public GameObject _wagon;
    // 走っているwagonのWagonController.
    // BGM, CrusherControllerでも使用する変数.
    [HideInInspector] public WagonController WagonControllerRun = null;
    // WagonControllerでも使用する変数.
    [HideInInspector] public bool goButtonFirstClick = false;
    // 生成されたPrefabのWagonController.
    // ObstacleButtonManagerでも使用する変数.
    [HideInInspector] public WagonController WagonController = null;
    #endregion

    private void Start() {
        BattleBuilder = _buildersDB.GetBattleBuilder(GameDirector.Instance.BuilderIndex);
        SetWagon(_wagonStartPosition);
        GameDirector.Instance.BuilderPosition = 5734.0f;
    }

    private void Update() {
        if (_wagon.transform.position.x < 5060.0f)
            SetWagon(new Vector3(5734, 11, 0));
        
        if (WagonControllerRun != null)
            GameDirector.Instance.BuilderPosition = WagonControllerRun.transform.position.x;
    }

    // ワゴンを配置する座標を受け取りワゴンを配置する.
    private void SetWagon(Vector3 position) {
        _wagon = Instantiate(BattleBuilder.WagonPrefab, position, Quaternion.identity, GameObject.Find("Builder").transform);
        _setBaseBlock.SetSpawnPoint();

        WagonController = _wagon.GetComponent<WagonController>();

        Debug.Log("placable");
        _battleBuilderUIController.SetObstaclePlaceable();
        // for (int i = 0; i < 6; i++) {
        //     isSetWagon[i] = true;
        // }
    }

    /// <summary>
    /// ワゴンを走らせる.
    /// </summary>
    public void RunWagon() {
        WagonControllerRun = WagonController;
        WagonControllerRun.XSpeed = -WagonControllerRun.Speed;
        for (int i = 0; i < 6; i++) {
            isRunningWagon[i] = true;
        }
    }
}
