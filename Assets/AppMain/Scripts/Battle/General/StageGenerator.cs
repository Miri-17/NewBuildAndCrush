using UnityEngine;

// TODO wagonに乗る前のクラッシャーコンティニュー位置
public class StageGenerator : MonoBehaviour {
    private GameObject _crusher = null;
    private CrusherController _crusherController = null;
    private GameObject _continuePoint = null;

    #region Serialized Fields
    [SerializeField] private BuildersDB _buildersDB = null;
    [SerializeField] private CrushersDB _crushersDB = null;
    [SerializeField] private BuilderController _builderController = null;
    [SerializeField] private SpriteRenderer _bg;
    [SerializeField] private SpriteRenderer _ground;
    [SerializeField] private GameObject _startPoint = null;
    #endregion

    private void Awake() {
        // Awakeに置かないと、CrusherCameraControllerでCrusherタグを見つけられない
        UpdateCrusherInfo(GameDirector.Instance.CrusherIndex);
    }
    
    private void Start() {
        UpdateBuilderInfo(GameDirector.Instance.BuilderIndex);
    }

    // TODO 時間があれば, このコードはBattleControllerに移す.
    // StageGeneratorという名前に見合わない仕事のため.
    private void Update() {
        if (_crusherController.IsContinueWaiting()) {
            _crusher.transform.position = _continuePoint.transform.position;
            _crusherController.ContinueCrusher();
        }

        if (_builderController.wagonControllerRun != null)
            // ワゴンに乗ったらコンティニューポイントを変更する.
            if (_builderController.wagonControllerRun.CrusherEnterCheck.IsOn)
                _continuePoint = _builderController.wagonControllerRun.CrusherContinuePosition;
    }

    private void UpdateCrusherInfo(int crusherIndex) {
        var battleCrusher = _crushersDB.GetBattleCrusher(crusherIndex);

        _crusher = Instantiate(battleCrusher.CrusherPrefab, _startPoint.transform.position, Quaternion.identity, GameObject.Find("Crusher").transform);
        _crusherController = _crusher.GetComponent<CrusherController>();
    }

    private void UpdateBuilderInfo(int builderIndex) {
        var battleBuilder = _buildersDB.GetBattleBuilder(builderIndex);

        _bg.sprite = battleBuilder.Bg;
        _ground.sprite = battleBuilder.Ground;

        var builderPosition = GetBuilderPosition(builderIndex);
        Instantiate(battleBuilder.BuilderPrefab, builderPosition, Quaternion.identity, GameObject.Find("Builder").transform);
    }

    private Vector3 GetBuilderPosition(int index) {
        switch (index) {
            // TODO zを変える以外の方法で、ビルダーの足を手前に持っていく.
            case 0: return new Vector3(5270.0f, -137.0f, -2.0f);
            case 1: return new Vector3(5270.0f, -132.7f, -2.0f);
            case 2: return new Vector3(5270.0f, -143.7f, -2.0f);
            default: return new Vector3(5270.0f, -144.7f, -2.0f);
        }
    }
}
