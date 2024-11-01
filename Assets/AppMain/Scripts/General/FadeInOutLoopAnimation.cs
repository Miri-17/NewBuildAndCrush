using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FadeInOutLoopAnimation : MonoBehaviour {
    private Tween _imageTween;
    
    [SerializeField] private Image _image = null;
    [SerializeField] private float _minColorAlpha = 0.2f;
    [SerializeField] private float _tweenDuration = 1.0f;

    private void Start() {
        _image.enabled = false;
        InitializeTween();
    }

    private void InitializeTween() {
        // もし既にTweenが存在する場合は再利用
        if (_imageTween == null) {
            _imageTween = _image.DOFade(_minColorAlpha, _tweenDuration)
                .SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Yoyo)
                .SetAutoKill(false) // Tweenを破棄せず再利用
                .Pause()
                .SetLink(_image.gameObject);
        }
    }

    /// <summary>
    /// 画像の表示とTweenの再生処理の制御を行う.
    /// </summary>
    /// <param name="isEnabled"></param>
    public void AnimationOnOff(bool isEnabled) {
        if (_image == null || _imageTween == null) return;

        _image.enabled = isEnabled;
        if (isEnabled) {
            _imageTween.Play();
        } else {
            _imageTween.Pause();
        }
    }

    private void OnDestroy() {
        if (_imageTween != null)
            _imageTween.Kill();
    }
}
