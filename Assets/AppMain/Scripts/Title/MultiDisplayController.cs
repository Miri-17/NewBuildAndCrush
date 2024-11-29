using UnityEngine;
using TMPro;

public class MultiDisplayController : MonoBehaviour {
    private int _displayCount = 0;

    [SerializeField] private int _maxDisplayCount = 2;
    [SerializeField] private TextMeshProUGUI _warningText = null;

    private void Awake() {
        _displayCount = Mathf.Min(Display.displays.Length, _maxDisplayCount);
        for (int i = 0; i < _displayCount; i++) {
            try {
                Display.displays[i].Activate();
            } catch (System.Exception e) {
                Debug.LogError($"Failed to activate display {i}: {e.Message}");
            }
        }
    }

    private void Start() {
        if (_displayCount == 1) {
            _warningText.text = "2画面目を起動するために、一旦ゲームを終了し、\n"
                + "デュアルディスプレイにしてからの再起動をよろしくお願いします\n\n"
                + "詳細はWebサイトまたはREADMEをご確認ください";
            _warningText.fontSize = 80;
        } else {
            _warningText.text = "もう1つのディスプレイをご覧ください";
            _warningText.fontSize = 100;
        }
    }
}
