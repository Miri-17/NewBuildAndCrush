using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections.Generic;

public class SoundListUIController : MonoBehaviour {
    #region Private Fields
    private Image _recordImage = null;
    private Tween _recordTween = null;
    private Tween _recordPlayerNeedleTween = null;
    private Color _illustrationColor = Color.white;
    private List<float> _selectionPositionsY = new List<float>() {
        481.0f, 391.0f, 301.0f, 211.0f, 121.0f, 31.0f, -59.0f, -149.0f, -239.0f, -329.0f, -419.0f, -509.0f, -599.0f, -599.0f, 
    };
    #endregion

    #region Serialized Fields
    [SerializeField] private SoundListController _soundListController = null;
    [SerializeField] private RectTransform _record = null;
    [SerializeField] private List<Sprite> _recordSprites = new List<Sprite>();
    [SerializeField] private RectTransform _recordPlayerNeedle = null;
    [SerializeField] private Image _frontIllustrationImage = null;
    [SerializeField] private Image _backIllustrationImage = null;
    [SerializeField] private List<Sprite> _illustrations = new List<Sprite>();
    [SerializeField] private RectTransform _texts = null;
    [SerializeField] private RectTransform _selection = null;
    #endregion

    private void Start() {
        InitializeRecord();
        InitializeIllustration();
    }

    private void InitializeRecord() {
        _recordImage = _record.GetComponent<Image>();
        _recordImage.sprite = _recordSprites[0];
        _recordTween = _record.DOLocalRotate(new Vector3(0, 0, -360f), 6.0f, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Restart)
            .SetLink(_record.gameObject)
            .Pause();
        _recordPlayerNeedleTween = _recordPlayerNeedle.DOLocalRotate(new Vector3(0, 0, -2.0f), 3.0f, RotateMode.Fast)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Yoyo)
            .SetLink(_recordPlayerNeedle.gameObject)
            .Pause();
    }
    
    private void InitializeIllustration() {
        _frontIllustrationImage.enabled = false;
        _illustrationColor = _frontIllustrationImage.color;
        _illustrationColor.a = 0;
        _frontIllustrationImage.color = _illustrationColor;
        _frontIllustrationImage.sprite = _illustrations[0];
        _backIllustrationImage.enabled = false;
    }

    public void StartPlayback() {
        _recordImage.sprite = _recordSprites[_soundListController.SoundIndex];
        _recordTween.Play();
        _recordPlayerNeedleTween.Play();
        _frontIllustrationImage.sprite = _illustrations[_soundListController.SoundIndex];
        _frontIllustrationImage.enabled = true;

        // IsFadingIllustration = true;
        _frontIllustrationImage.DOFade(1.0f, 0.8f)
            .SetEase(Ease.Linear)
            .SetLink(_frontIllustrationImage.gameObject)
            .OnComplete(() => {
                _backIllustrationImage.sprite = _illustrations[_soundListController.SoundIndex];
                _backIllustrationImage.enabled = true;
                // IsFadingIllustration = false;
            });
    }

    public void ResetPlayback() {
        _recordTween.Pause();
        _recordPlayerNeedleTween.Pause();
        _frontIllustrationImage.enabled = false;
        _illustrationColor = _frontIllustrationImage.color;
        _illustrationColor.a = 0;
        _frontIllustrationImage.color = _illustrationColor;
    }

    public void SetAnchoredPosition(Vector2 position) {
        _texts.anchoredPosition = position;
        _selection.anchoredPosition = new Vector2(0, _selectionPositionsY[_soundListController.SoundIndex]);
    }

    public void SetRecordSprite(int index) {
        _recordImage.sprite = _recordSprites[index];
    }
}
