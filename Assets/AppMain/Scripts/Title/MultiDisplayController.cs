using UnityEngine;

// TODO 起動時のみ実行するように変えたほうがいいかもしれない
public class MultiDisplayController : MonoBehaviour {
    [SerializeField] private int _maxDisplayCount = 2;

    private void Awake() {
        var displayCount = Mathf.Min(Display.displays.Length, _maxDisplayCount);
        for (int i = 0; i < displayCount; i++) {
            try {
                Display.displays[i].Activate();
            } catch (System.Exception e) {
                Debug.LogError($"Failed to activate display {i}: {e.Message}");
            }
        }
    }
}
