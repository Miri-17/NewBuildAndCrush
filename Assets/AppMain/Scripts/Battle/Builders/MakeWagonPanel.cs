using UnityEngine;
using UnityEngine.UI;

public class MakeWagonPanel : MonoBehaviour {
    private DirectionController _directionController = null;

    [SerializeField] private Button _wagonReadyButton = null;

    private void Start() {
        _directionController = GameObject.FindGameObjectWithTag("Direction").GetComponent<DirectionController>();

        _wagonReadyButton.onClick.AddListener(() => OnWagonReadyButtonClicked());
    }

    private void OnWagonReadyButtonClicked() {
        _directionController.BuilderReady();
    }
}
