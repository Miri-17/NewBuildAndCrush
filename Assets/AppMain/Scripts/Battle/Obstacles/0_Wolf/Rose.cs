using UnityEngine;

public class Rose : MonoBehaviour {
    private Animator _animator = null;
    private bool _isDamageAnimation = false;

    [SerializeField] private DestroyableObstacle _destroyableObstacle = null;
    [SerializeField] private BoxCollider2D _spiny = null;

    private void Start() {
        _animator = this.GetComponent<Animator>();
        _spiny.enabled = false;
    }

    private void Update() {
        if (_isDamageAnimation || !_destroyableObstacle.IsDamaged) return;

        _isDamageAnimation = true;
        _animator.Play("Rose_Damage");
    }

    /// <summary>
    /// 棘のコライダーを有効にする.
    /// </summary>
    public void ExtendThorns() {
        if (_spiny == null) return;
        _spiny.enabled = true;
    }

    /// <summary>
    /// 棘のコライダーを無効にする.
    /// </summary>
    public void RetractThorns() {
        if (_spiny == null) return;
        _spiny.enabled = false;
    }
}
