using UnityEngine;

public class Dish : MonoBehaviour {
    private Rigidbody2D _rb2D = null;
    private AudioSource _audioSource = null;

    [SerializeField] private float _forceX = -240.0f;
    [SerializeField] private float _forceY = 180.0f;

    protected virtual void Start() {
        _rb2D = this.GetComponent<Rigidbody2D>();
        _audioSource = this.GetComponent<AudioSource>();
        _rb2D.AddForce(new Vector2(_forceX, _forceY), ForceMode2D.Impulse);

        if (_audioSource.clip != null)
            _audioSource.PlayOneShot(_audioSource.clip);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (!collision.collider.CompareTag("Obstacle") && !collision.collider.CompareTag("CrusherWall"))
            Destroy(this.gameObject);
    }
}
