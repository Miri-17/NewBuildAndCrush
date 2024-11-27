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
    
    private void Start() {
        UpdateCrusherInfo(GameDirector.Instance.CrusherIndex);
        UpdateBuilderInfo(GameDirector.Instance.BuilderIndex);
        // TODO 死んだ位置から少し前にコンティニューするよう変更.
        _continuePoint = _startPoint;
    }
    
    private void Update() {
        // カメラに関する処理.
        var crusherPosition = _crusher.transform.position;
        if (crusherPosition.x > -25.0f && crusherPosition.x < 5070.0f)
            this.transform.position = new Vector3(crusherPosition.x, this.transform.position.y, this.transform.position.z);
        
        if (_crusherController.IsContinueWaiting()) {
            _crusher.transform.position = _continuePoint.transform.position;
            _crusherController.ContinueCrusher();
        }

        // TODO 時間があれば, このコードはBattleControllerに移す.
        if (_builderController.WagonControllerRun != null) {
            // ワゴンに乗ったらコンティニューポイントを変更する.
            if (_builderController.WagonControllerRun.CrusherEnterCheck.IsOn)
                _continuePoint = _builderController.WagonControllerRun.CrusherContinuePosition;
            // TODO ワゴンから降りたら、死んだ位置から少し前にコンティニューするよう変更.
        }
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
            case 0: return new Vector3(5270f, -136f, -2f);
            case 1: return new Vector3(5270f, -132f, -2f);
            case 2: return new Vector3(5270f, -143.7f, -2f);
            default: return new Vector3(5270f, -144f, -2f);
        }
    }
}
