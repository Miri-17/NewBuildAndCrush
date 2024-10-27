using UnityEngine;
using UnityEngine.SceneManagement;

public class CrusherSelectAnimationCompleted : MonoBehaviour {
    private GameObject _panel = null;
    
    [SerializeField] private GameObject _warningPanel = null;
    [SerializeField] private string _parentName = "";

    private void Start() {
        if (SceneManager.GetActiveScene().name != "PlayerSelection")
            Destroy(this.GetComponent<CrusherSelectAnimationCompleted>());
    }
    
    private void Update() {
        if (Input.GetButtonDown("Jump"))
            Destroy(_panel);
    }

    public void OnCompleteAnimation() {
        _panel = Instantiate(_warningPanel);
        _panel.transform.SetParent(GameObject.Find(_parentName).transform, false);
        _panel.SetActive(true);
    }
}
