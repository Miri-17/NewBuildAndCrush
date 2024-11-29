using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;

public class ModeSelectionController : MonoBehaviour {
    #region Private Fields
    private AudioSource _audioSourceSE = null;
    private AudioClip _audioClipSE = null;
    private GameObject _rulesPanel = null;
    private bool _isDisplayRulesPanel = false;
    // シーン遷移関係
    private bool _isChangingScene = false;
    private int _nextSceneIndex = 0;
    private int _previousSelectIndex = 0;
    #endregion

    #region Serialized Fields
    [SerializeField] private ModeSelectionUIController _modeSelectionUIController = null;
    [SerializeField] private ModeSelectionBook[] _modeSelectionBooks = new ModeSelectionBook[0];
    [SerializeField] private Image _rulesButton = null;
    [SerializeField] private Sprite[] _rulesButtonSprites = new Sprite[0];
    [SerializeField] private GameObject _rulesPanelPrefab = null;
    // シーン遷移関係
    [SerializeField] private string[] _nextSceneNames = new string[0];
    #endregion

    private void Start() {
        if (CrusherSE.Instance == null) {
            Debug.LogError("SE instance is not available.");
            return;
        }

        if (_nextSceneNames == null || !_nextSceneNames.Any()) {
            Debug.LogError("Next scene names list is not set or is empty.");
            return;
        }
        if (_nextSceneNames.Any(string.IsNullOrEmpty)) {
            Debug.LogError("One or more scene names in the list are not set.");
            return;
        }

        _audioSourceSE = CrusherSE.Instance.GetComponent<AudioSource>();
        _audioClipSE = CrusherSE.Instance.SEDB.AudioClips[0];
        if (_audioSourceSE == null || _audioClipSE == null)
            Debug.LogError("AudioSource of AudioClip is not assigned properly.");
        
        switch (GameDirector.Instance.PreviousSceneName) {
            case "SoundList":
                _nextSceneIndex = 1;
                break;
            case "Credits":
                _nextSceneIndex = 2;
                break;
            default:
                break;
        }
        _modeSelectionBooks[_nextSceneIndex].SetSelection(true);
        _previousSelectIndex = _nextSceneIndex;
        
        _rulesButton.sprite = _rulesButtonSprites[0];
    }

    private void Update() {
        if (_isDisplayRulesPanel || _isChangingScene || !_modeSelectionUIController.IsAnimationEnded)
            return;
        
        if (Input.GetButtonDown("Horizontal")) {
            var horizontalKey = Input.GetAxisRaw("Horizontal");
            if (horizontalKey > 0) {
                _nextSceneIndex++;
                if (_nextSceneIndex >= _nextSceneNames.Length)
                    _nextSceneIndex = 0;
            } else if (horizontalKey < 0) {
                _nextSceneIndex--;
                if (_nextSceneIndex < 0)
                    _nextSceneIndex = _nextSceneNames.Length - 1;
            }

            _modeSelectionBooks[_previousSelectIndex].SetSelection(false);
            _modeSelectionBooks[_nextSceneIndex].SetSelection(true);
            _previousSelectIndex = _nextSceneIndex;

            _audioSourceSE.PlayOneShot(CrusherSE.Instance.SEDB.AudioClips[1]);
        } else if (Input.GetButtonDown("Select")) {
            _isChangingScene = true;
            
            _audioSourceSE.PlayOneShot(CrusherSE.Instance.SEDB.AudioClips[0]);

            GoNextSceneAsync(0.5f, _nextSceneNames[_nextSceneIndex]).Forget();
        } else if (Input.GetButtonDown("Fire1")) {
            _isDisplayRulesPanel = true;
            _rulesPanel = Instantiate(_rulesPanelPrefab, GameObject.Find("MiddlePanel").transform);
            _rulesPanel.transform.localPosition = Vector3.zero;
            _rulesButton.sprite = _rulesButtonSprites[1];
        }
    }

    private async UniTaskVoid GoNextSceneAsync(float duration, string nextSceneName) {
        try {
            await UniTask.Delay((int)(duration * 1000), cancellationToken: this.GetCancellationTokenOnDestroy());
            SceneManager.LoadScene(nextSceneName);
        } catch (System.Exception e) {
            Debug.LogError($"Scene transition failed: {e.Message}");
        }
    }

    public void CloseRulesPanel() {
        _isDisplayRulesPanel = false;
        _rulesButton.sprite = _rulesButtonSprites[0];
        Destroy(_rulesPanel);
    }
}
