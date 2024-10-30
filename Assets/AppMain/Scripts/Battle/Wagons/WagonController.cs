using UnityEngine;

public class WagonController : MonoBehaviour {
    #region Private Fields
    private BuilderController _builderController = null;
    private BattleController _battleController = null;
    private Rigidbody2D _rb2D = null;
    private float _xSpeed = 0;
    #endregion

    #region Serialized Fields
    [SerializeField] private CrusherTriggerCheck _crusherEnterCheck = null;
    [SerializeField] private CrusherTriggerCheck _crusherExitCheck = null;
    [SerializeField] private GameObject _crusherContinuePosition = null;
    [SerializeField] private GameObject explosionEffect = null;
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
        _builderController = GameObject.Find("BuilderController").GetComponent<BuilderController>();
        _battleController = GameObject.Find("BattleController").GetComponent<BattleController>();
        _rb2D = this.GetComponent<Rigidbody2D>();
    }

    private void Update() {
        if (_crusherExitCheck.IsOn) {
            DestroyWagon();
        }
    }

    private void FixedUpdate() {
        _rb2D.velocity = new Vector2(_xSpeed, 0.0f);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "GameOver")
            _battleController.GoNextScene();
    }
    
    private void DestroyWagon() {
        // _builderController.goButtonFirstClick = false;
        GameDirector.Instance.WagonCrushCounts++;
        Instantiate(explosionEffect, this.transform.position, Quaternion.identity);
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
