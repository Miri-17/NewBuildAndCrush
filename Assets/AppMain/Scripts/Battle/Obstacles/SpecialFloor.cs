using UnityEngine;

public class SpecialFloor : MonoBehaviour {
    private AudioSource _audioSource = null;

    private void Start() {
        _audioSource = this.GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Crusher"))
            _audioSource.Play();
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Crusher"))
            _audioSource.Stop();
    }
}
