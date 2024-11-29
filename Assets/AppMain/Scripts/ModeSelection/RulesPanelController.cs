using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class RulesPanelController : MonoBehaviour {
    #region Private Fields
    private ModeSelectionController _modeSelectionController = null;
    private AudioSource _audioSourceSE = null;
    private int _ruleIndex = 0;
    private int _previousRuleIndex = 0;
    private GameObject _optionsPanel = null;
    private GameObject _warningPanel = null;
    private bool _isDisplayOptionsPanel = false;
    private bool _isDisplayWarningPanel = false;
    #endregion

    #region Serialized Fields
    [SerializeField] private Image _rulesPanelImage = null;
    [SerializeField] private Sprite[] _rulesPanelSprites = new Sprite[0];
    [SerializeField] private ModeSelectionButton[] _modeSelectionButtons = new ModeSelectionButton[0];
    [SerializeField] private Image _glowImageY = null;
    [SerializeField] private GameObject _optionsPanelPrefab = null;
    [SerializeField] private GameObject _warningPanelPrefab = null;
    #endregion

    private void Start() {
        _modeSelectionController = GameObject.Find("ModeSelectionController").transform.GetComponent<ModeSelectionController>();
        _audioSourceSE = CrusherSE.Instance.GetComponent<AudioSource>();

        _modeSelectionButtons[_ruleIndex].SetSelection(true);
        _previousRuleIndex = _ruleIndex;

        _glowImageY.DOFade(1.0f, 2.0f)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Yoyo)
            .SetLink(_glowImageY.gameObject);
        
        // ModeSelectionButton の初期化.
        _modeSelectionButtons[0].SetSelection(true);
        _modeSelectionButtons[1].SetSelection(false);
        _modeSelectionButtons[2].SetSelection(false);
        _modeSelectionButtons[3].SetSelection(false);
        _modeSelectionButtons[4].SetSelection(false);
    }

    private void Update() {
        if (_isDisplayOptionsPanel || _isDisplayWarningPanel)
            return;
        
        if (Input.GetButtonDown("Horizontal")) {
            var horizontalKey = Input.GetAxisRaw("Horizontal");
            if (horizontalKey > 0) {
                _ruleIndex++;
                if (_ruleIndex >= _rulesPanelSprites.Length)
                    _ruleIndex = 0;
            } else if (horizontalKey < 0) {
                _ruleIndex--;
                if (_ruleIndex < 0)
                    _ruleIndex = _rulesPanelSprites.Length - 1;
            }

            _audioSourceSE.PlayOneShot(CrusherSE.Instance.SEDB.AudioClips[1]);

            _rulesPanelImage.sprite = _rulesPanelSprites[_ruleIndex];
            _modeSelectionButtons[_previousRuleIndex].SetSelection(false);
            _modeSelectionButtons[_ruleIndex].SetSelection(true);
            if (_ruleIndex == 3 || _ruleIndex == 4)
                _glowImageY.enabled = true;
            else
                _glowImageY.enabled = false;
            
            _previousRuleIndex = _ruleIndex;
        } else if (Input.GetButtonDown("Fire1")) {
            _modeSelectionController.CloseRulesPanel();
        } else if (_ruleIndex == 3 && Input.GetButtonDown("Select")) {
            _isDisplayOptionsPanel = true;
            _optionsPanel = Instantiate(_optionsPanelPrefab, GameObject.Find("CrusherCanvas").transform);
            _optionsPanel.transform.localPosition = Vector3.zero;
        } else if (_ruleIndex == 4 && Input.GetButtonDown("Select")) {
            _isDisplayWarningPanel = true;
            _warningPanel = Instantiate(_warningPanelPrefab, GameObject.Find("CrusherCanvas").transform);
            _warningPanel.transform.localPosition = Vector3.zero;
        }
    }

    public void CloseOptionPanel() {
        _isDisplayOptionsPanel = false;
        Destroy(_optionsPanel);
    }
    
    public void CloseWarningPanel() {
        _isDisplayWarningPanel = false;
        Destroy(_warningPanel);
    }
}
