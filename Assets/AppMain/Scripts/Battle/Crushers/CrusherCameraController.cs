using UnityEngine;

public class CrusherCameraController : MonoBehaviour {
    private GameObject _crusher = null;

    private void Start() {
        _crusher = GameObject.FindGameObjectWithTag("Crusher");
    }

    private void Update() {
        var crusherPosition = _crusher.transform.position;
        if (crusherPosition.x > -25.0f && crusherPosition.x < 5070.0f)
            this.transform.position = new Vector3(crusherPosition.x, this.transform.position.y, this.transform.position.z);
    }
}
