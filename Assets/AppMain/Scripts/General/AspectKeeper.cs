using UnityEngine;

public class AspectKeeper : MonoBehaviour {
    [SerializeField] private Camera _targetCamera = null;
    [SerializeField] private Vector2 _aspectVector = new Vector2(0, 0);

    private void Update() {
        var screenAspect = Screen.width / (float)Screen.height;
        var targetAspect = _aspectVector.x / _aspectVector.y;

        var magnification = targetAspect / screenAspect; // 目的アスペクト比にするための倍率
        var viewportRect = new Rect(0, 0, 1, 1);
        if (magnification < 1) {
            viewportRect.width = magnification;
            viewportRect.x = 0.5f - viewportRect.width * 0.5f;
        } else {
            viewportRect.height = 1 / magnification;
            viewportRect.y = 0.5f - viewportRect.height * 0.5f;
        }
        _targetCamera.rect = viewportRect;
    }
}
