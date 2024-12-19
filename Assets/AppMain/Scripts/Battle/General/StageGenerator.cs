using UnityEngine;

public class StageGenerator : MonoBehaviour {
    #region Private Fields
    private GameObject _crusher = null;
    private CrusherController _crusherController = null;
    private GameObject _startPosition = null;
    private bool _isContinue2StartPosition = true;
    private bool _isCrusherOn = false;
    #endregion

    #region Serialized Fields
    [SerializeField] private BuildersDB _buildersDB = null;
    [SerializeField] private CrushersDB _crushersDB = null;
    [SerializeField] private BuilderController _builderController = null;
    [SerializeField] private SpriteRenderer _bg;
    [SerializeField] private SpriteRenderer _ground;
    [SerializeField, Header("初期はStartPosition")] private GameObject _continuePosition = null;
    #endregion
    
    private void Start() {
        UpdateCrusherInfo(GameDirector.Instance.CrusherIndex);
        UpdateBuilderInfo(GameDirector.Instance.BuilderIndex);
        _startPosition = _continuePosition;
    }
    
    private void Update() {
        // カメラに関する処理.
        var crusherPosition = _crusher.transform.position;
        if (crusherPosition.x > -25.0f && crusherPosition.x < 5070.0f)
            this.transform.position = new Vector3(crusherPosition.x, this.transform.position.y, this.transform.position.z);
        
        if (_crusherController.IsContinueWaiting()) {
            if (!_isCrusherOn)
                _builderController.WagonControllerRun.CrusherEnterCheck.CrusherIsOn();
            
            _crusher.transform.position = _continuePosition.transform.position;
            _crusherController.ContinueCrusher();
        }

        // TODO 時間があれば, このコードはBattleControllerに移す.
        if (_builderController.WagonControllerRun != null) {
            // ワゴンが発車したら, ワゴンの先頭にコンティニューするようにする.
            // これはワゴンの近くで死ぬこと以外がほぼ起こり得らないためである.
            if (_isContinue2StartPosition) {
                Debug.Log("hello A");
                _isContinue2StartPosition = false;
                _continuePosition = _builderController.WagonControllerRun.CrusherContinuePosition;
            }
            if (_builderController.WagonControllerRun.CrusherEnterCheck.IsOn)
                _isCrusherOn = true;
            // ワゴン外で死んだ時に起こり得る. EnterCheckのIsOnフラグを強制的に降ろす.
            if (!_isCrusherOn && _builderController.WagonControllerRun.CrusherEnterCheck.IsOn) {
                _builderController.WagonControllerRun.CrusherEnterCheck.CrusherIsOff();
                _isCrusherOn = true;
            }
        } else {
            // 確率的にほぼあり得ないのだが,　ワゴンが壊されてから次のワゴンが発車するまでに死んだ場合は
            // StartPositionからコンティニューするようにする.
            if (!_isContinue2StartPosition) {
                Debug.Log("hello B");
                _isCrusherOn = false;
                _isContinue2StartPosition = true;
                _continuePosition = _startPosition;
            }
        }
    }

    private void UpdateCrusherInfo(int crusherIndex) {
        var battleCrusher = _crushersDB.GetBattleCrusher(crusherIndex);

        _crusher = Instantiate(battleCrusher.CrusherPrefab, _continuePosition.transform.position, Quaternion.identity, GameObject.Find("Crusher").transform);
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
