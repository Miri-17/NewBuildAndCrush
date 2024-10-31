using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SoundListMusic : MonoBehaviour {
    private Tween _noteTween;
    
    [SerializeField] private Image _note = null;

    private void Start() {
        _note.enabled = false;
        InitializeTween();
    }

    private void InitializeTween() {
        // もし既にTweenが存在する場合は再利用
        if (_noteTween == null) {
            _noteTween = _note.DOFade(0.2f, 1.0f)
                .SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Yoyo)
                .SetAutoKill(false) // Tweenを破棄せず再利用
                .Pause()
                .SetLink(_note.gameObject);
        }
    }

    /// <summary>
    /// 音符の表示とTweenの再生処理の制御を行う.
    /// </summary>
    /// <param name="isOn"></param>
    public void NoteOnOff(bool isOn) {
        _note.enabled = isOn;

        if (isOn) {
            _noteTween.Play();
        } else {
            _noteTween.Pause();
        }
    }
}
