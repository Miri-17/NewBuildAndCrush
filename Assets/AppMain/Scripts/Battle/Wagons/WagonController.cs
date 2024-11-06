using UnityEngine;

public class WagonController : MonoBehaviour {
    #region Private Fields
    private BattleBuilderUIController _battleBuilderUIController = null;
    private BattleController _battleController = null;
    private Rigidbody2D _rb2D = null;
    private float _xSpeed = 0;
    private AudioSource _audioSourceSE = null;
    private bool _isChangingScene = false;
    #endregion

    #region Serialized Fields
    [SerializeField] private CrusherTriggerCheck _crusherEnterCheck = null;
    [SerializeField] private CrusherTriggerCheck _crusherExitCheck = null;
    [SerializeField] private GameObject _crusherContinuePosition = null;
    [SerializeField] private GameObject _wagonExplosionPrefab = null;
    [SerializeField] private float _speed = 180.0f;
    #endregion

    #region
    // BuilderControllerのRunWagon()で値を変更する変数
    public float XSpeed { get => _xSpeed; set => _xSpeed = value; }
    public float Speed { get => _speed; set => _speed = value; }
    // BuilderController, BattleBGMControllerでも使う変数
    public CrusherTriggerCheck CrusherEnterCheck { get => _crusherEnterCheck; set => _crusherEnterCheck = value; }
    public CrusherTriggerCheck CrusherExitCheck { get => _crusherExitCheck; set => _crusherExitCheck = value; }
    // StageGeneratorで使用
    public GameObject CrusherContinuePosition { get => _crusherContinuePosition; set => _crusherContinuePosition = value; }
    #endregion

    private void Start() {
        _battleBuilderUIController = GameObject.Find("BuilderUIController").GetComponent<BattleBuilderUIController>();
        _battleController = GameObject.Find("BattleController").GetComponent<BattleController>();
        _rb2D = this.GetComponent<Rigidbody2D>();
        _audioSourceSE = CrusherSE.Instance.GetComponent<AudioSource>();
    }

    private void Update() {
        if (_crusherExitCheck.IsOn)
            DestroyWagon();
    }

    private void FixedUpdate() {
        _rb2D.velocity = new Vector2(_xSpeed, 0.0f);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (!_isChangingScene && collision.gameObject.tag == "GameOver") {
            _isChangingScene = true;
            _xSpeed = 0;    // ワゴンを止める.
            GameDirector.Instance.IsBuilderWin = true;
            _audioSourceSE.PlayOneShot(CrusherSE.Instance.SEDB.AudioClips[7]);
            _battleController.GoNextScene();
        }
    }
    
    private void DestroyWagon() {
        // WagonController自体は消えてしまうので, null参照しないようにデータベースに入れたAudioClipを再生.
        _audioSourceSE.PlayOneShot(CrusherSE.Instance.SEDB.AudioClips[5]);
        _battleBuilderUIController.SetGoButtonInteractive(true);
        GameDirector.Instance.WagonCrushCounts++;
        Instantiate(_wagonExplosionPrefab, this.transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }

    /// <summary>
    /// ワゴンの速さを返す
    /// </summary>
    /// <returns>xSpeed</returns>
    public float GetWagonVelocity() {
        return _xSpeed;
    }
}
