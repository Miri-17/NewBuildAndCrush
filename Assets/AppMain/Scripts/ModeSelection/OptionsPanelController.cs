using UnityEngine;

public class OptionsPanelController : MonoBehaviour {
    private RulesPanelController _rulesPanelController = null;
    private AudioSource _audioSourceSE = null;
    private int _optionIndex = 0;
    private int _previousOptionIndex = 0;

    #region Serialized Fields
    [SerializeField] private AudioSettings[] _audioSettings = new AudioSettings[3];
    [SerializeField] private BattleTimeSettings _battleTimeSettings = null;
    [SerializeField] private ModeSelectionButton _modeSelectionButton = null;
    #endregion

    protected void Start() {
        _rulesPanelController = GameObject.FindWithTag("RulesPanel").GetComponent<RulesPanelController>();
        _audioSourceSE = CrusherSE.Instance.GetComponent<AudioSource>();

        // AudioSettings の初期化.
        _audioSettings[0].SetSelection(true);
        _audioSettings[1].SetSelection(false);
        _audioSettings[2].SetSelection(false);

        // BattleTimeSettings の初期化.
        _battleTimeSettings.SetSelection(false);

        // ModeSelectionButton の初期化.
        _modeSelectionButton.SetSelection(false);

        _previousOptionIndex = _optionIndex;
    }

    private void Update() {
        if (Input.GetButtonDown("Vertical")) {
            var verticalKey = Input.GetAxisRaw("Vertical");
            if (verticalKey < 0) {
                _optionIndex++;
                if (_optionIndex >= 5)
                    _optionIndex = 0;
            } else if (verticalKey > 0) {
                _optionIndex--;
                if (_optionIndex < 0)
                    _optionIndex = 4;
            }

            _audioSourceSE.PlayOneShot(CrusherSE.Instance.SEDB.AudioClips[1]);

            switch(_previousOptionIndex) {
                case 3:
                    _battleTimeSettings.SetSelection(false);
                    break;
                case 4:
                    _modeSelectionButton.SetSelection(false);
                    break;
                default:
                    _audioSettings[_previousOptionIndex].SetSelection(false);
                    break;
            }
            switch (_optionIndex) {
                case 3:
                    _battleTimeSettings.SetSelection(true);
                    break;
                case 4:
                    _modeSelectionButton.SetSelection(true);
                    break;
                default:
                    _audioSettings[_optionIndex].SetSelection(true);
                    break;
            }
            
            _previousOptionIndex = _optionIndex;
        } else if (_optionIndex == 4 && Input.GetButtonDown("Select")) {
            _rulesPanelController.CloseOptionPanel();
        }
    }
}
