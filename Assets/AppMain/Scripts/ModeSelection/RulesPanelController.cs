using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class RulesPanelController : MonoBehaviour {
    private int _rulesIndex = 0;
    private AudioSource _audioSource_SE = null;
    private int _previousRulesIndex = 0;

    [SerializeField] private List<ExplanationButton> _explanationButtons = new List<ExplanationButton>();
    [SerializeField] private Image _rulesButton = null;
    [SerializeField] private Sprite _rulesButtonSprite = null;
    [SerializeField] private Image _rulesPanelImage = null;
    [SerializeField] private List<Sprite> _rulesPanelSprites = new List<Sprite>();
    [SerializeField] private GameObject _warningPanel = null;
    [SerializeField] private Image _glowImageY = null;

    private void Start() {
        _audioSource_SE = CrusherSE.Instance.GetComponent<AudioSource>();

        _explanationButtons[_rulesIndex].SetSelection(true);
        _previousRulesIndex = _rulesIndex;

        _glowImageY.DOFade(1.0f, 2.0f)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Yoyo)
            .SetLink(_glowImageY.gameObject);
    }

    private void Update() {
        if (_warningPanel.activeSelf)
            return;
        
        if (Input.GetButtonDown("Horizontal")) {
            var horizontalKey = Input.GetAxisRaw("Horizontal");
            if (horizontalKey > 0) {
                _rulesIndex++;
                if (_rulesIndex >= _rulesPanelSprites.Count)
                    _rulesIndex = 0;
            } else if (horizontalKey < 0) {
                _rulesIndex--;
                if (_rulesIndex < 0)
                    _rulesIndex = _rulesPanelSprites.Count - 1;
            }

            _rulesPanelImage.sprite = _rulesPanelSprites[_rulesIndex];
            _explanationButtons[_previousRulesIndex].SetSelection(false);
            _explanationButtons[_rulesIndex].SetSelection(true);
            _previousRulesIndex = _rulesIndex;
            if (_rulesIndex == 3)
                _glowImageY.enabled = true;
            else
                _glowImageY.enabled = false;

            _audioSource_SE.PlayOneShot(CrusherSE.Instance.SEDB.AudioClips[1]);
        } else if (Input.GetButtonDown("Fire1")) {
            this.gameObject.SetActive(false);
            _rulesButton.sprite = _rulesButtonSprite;
        } else if (_rulesIndex == 3 && Input.GetButtonDown("Select")) {
            _warningPanel.SetActive(true);
        }
    }
}
