using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class AudioSettings : MonoBehaviour {
    #region Private Fields
    private Slider _slider = null;
    private List<RectTransform> _arrows = new List<RectTransform>();
    private float _nextChangeTime = 0;
    #endregion

    #region Serialized Fields
    [SerializeField, Header("順にMaster, BGM, SE")] private int _audioIndex = 0;
    [SerializeField] private string _audioMixerName = "VolumeMaster";
    [SerializeField] private AudioMixer _audioMixer = null;
    [SerializeField] private Image _frame = null;
    [SerializeField] private Sprite[] _frameSprites = new Sprite[0];
    [SerializeField] private TextMeshProUGUI _valueText = null;
    [SerializeField] private Image _glowImage = null;
    [SerializeField] private Image[] _arrowImages = new Image[0];
    [SerializeField] private float[] _arrowPositions = new float[0];
    [SerializeField, Header("1秒に何ボリューム変えられるか")] private float _changeRate = 3.0f;
    #endregion

    public bool IsSelected { get; private set; } = false;

    private void Start() {
        _slider = this.GetComponent<Slider>();
        _slider.onValueChanged.AddListener((x) => ChangeAudioVolume(x));
        _slider.value = GameDirector.Instance.AudioValue[_audioIndex];
        _valueText.text = _slider.value.ToString();

        _glowImage.DOFade(1.0f, 2.0f)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Yoyo)
            .SetLink(_glowImage.gameObject);
        
        int i = 0;
        foreach (var arrowImage in _arrowImages) {
            RectTransform arrow = arrowImage.GetComponent<RectTransform>();
            _arrows.Add(arrow);

            arrow.DOAnchorPosY(_arrowPositions[i], 1.0f)
                .SetEase(Ease.OutCubic)
                .SetLoops(-1, LoopType.Yoyo)
                .SetLink(arrow.gameObject);
            i++;
        }
    }

    private void Update() {
        if (IsSelected && Time.time >= _nextChangeTime && Input.GetButton("Horizontal")) {
            var horizontalKey = Input.GetAxisRaw("Horizontal");
            if (horizontalKey < 0) {
                // GameDirector.Instance.AudioValue[_audioIndex]--;
                _slider.value--;
                if (_slider.value < 0)
                    _slider.value = 0;
            } else if (horizontalKey > 0) {
                _slider.value++;
                if (_slider.value >= _slider.maxValue)
                    _slider.value = _slider.maxValue;
            }
            
            _nextChangeTime = Time.time + 1.0f / _changeRate;
        }
    }

    private void ChangeAudioVolume(float value) {
        value /= 100;
        var volume = Mathf.Clamp(Mathf.Log10(value) * 20f, -80f, 0f);
        _audioMixer.SetFloat(_audioMixerName, volume);
        Debug.Log($"{_audioMixerName}: {volume}");
        // GameDirector.Instance.MasterSliderValue = _masterSlider.value;
        GameDirector.Instance.AudioValue[_audioIndex] = _slider.value;
        // Debug.Log("Master: " + _masterSlider.value);
        _valueText.text = _slider.value.ToString();
    }

    public void SetSelection(bool isSelected) {
        IsSelected = isSelected;

        if (isSelected)
            _frame.sprite = _frameSprites[1];
        else
            _frame.sprite = _frameSprites[0];
        
        _glowImage.enabled = IsSelected;
        
        foreach (var arrowImage in _arrowImages)
            arrowImage.enabled = IsSelected;
    }
}
