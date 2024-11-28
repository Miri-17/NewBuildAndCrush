using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningPanelController : MonoBehaviour {
    private RulesPanelController _rulesPanelController = null;
    private AchievementsController _achievementsController = null;
    private AudioSource _audioSource_SE = null;
    private int _warningIndex = 0;

    [SerializeField] private ModeSelectionButton[] _modeSelectionButtons = new ModeSelectionButton[2];

    private void Start() {
        _rulesPanelController = GameObject.FindWithTag("RulesPanel").GetComponent<RulesPanelController>();
        _achievementsController = GameObject.Find("AchievementsController").transform.GetComponent<AchievementsController>();

        _audioSource_SE = CrusherSE.Instance.GetComponent<AudioSource>();

        // ModeSelectionButton の初期化.
        _modeSelectionButtons[0].SetSelection(true);
        _modeSelectionButtons[1].SetSelection(false);
    }

    private void Update() {
        if (Input.GetButtonDown("Horizontal")) {
            if (_warningIndex == 0) {
                _modeSelectionButtons[0].SetSelection(false);
                _modeSelectionButtons[1].SetSelection(true);
                _warningIndex = 1;
            } else {
                _modeSelectionButtons[1].SetSelection(false);
                _modeSelectionButtons[0].SetSelection(true);
                _warningIndex = 0;
            }

            _audioSource_SE.PlayOneShot(CrusherSE.Instance.SEDB.AudioClips[1]);
        } else if (Input.GetButtonDown("Select")) {
            if (_warningIndex == 0) {
                _rulesPanelController.CloseWarningPanel();
            } else {
                _audioSource_SE.PlayOneShot(CrusherSE.Instance.SEDB.AudioClips[3]);
                ResetData();
                _rulesPanelController.CloseWarningPanel();
            }
        }
    }

    private void ResetData() {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        _achievementsController.LoadPlayerPrefs();
        _achievementsController.UpdateAchievementImages();
    }
}
