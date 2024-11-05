using UnityEngine;

public class Spike : MonoBehaviour {
    private BuilderController _builderController = null;
    private Rigidbody2D _rb2D = null;
    // private bool _canMove = false;
    // private bool _key = false;

    private void Start() {
        _builderController = GameObject.Find("BuilderController").GetComponent<BuilderController>();
        _rb2D = this.GetComponent<Rigidbody2D>();
        // _rb2D.gravityScale = 0;
    }

    private void FixedUpdate() {
        if (_builderController.WagonControllerRun != null)
            _rb2D.velocity = new Vector2(_builderController.WagonControllerRun.GetWagonVelocity(), 0);
            // _rb2D.constraints
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Crusher"))
            _rb2D.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
}
