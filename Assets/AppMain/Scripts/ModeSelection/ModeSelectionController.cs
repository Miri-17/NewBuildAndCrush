using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;

public class ModeSelectionController : MonoBehaviour {
    #region Private Fields
    private AudioSource _audioSource_SE = null;
    private AudioClip _audioClip_SE = null;
    // シーン遷移関係
    private int _nextSceneIndex = 0;
    private int _previousSelectIndex = 0;
    private bool _isChangingScene = false;
    #endregion

    // シーン遷移関係
    [SerializeField] private List<ModeSelectionBook> _modeSelectionBooks = new List<ModeSelectionBook>();
    [SerializeField] private ModeSelectionUIController _modeSelectionUIController = null;
    [SerializeField] private List<string> _nextSceneNames = new List<string>();
    // TODO ルールパネル系 (UIの方に移すかも)
    [SerializeField] private GameObject _rulesPanel = null;
    [SerializeField] private Image _rulesButton = null;
    [SerializeField] private List<Sprite> _rulesButtonSprites = new List<Sprite>();

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

        _audioSource_SE = CrusherSE.Instance.GetComponent<AudioSource>();
        _audioClip_SE = CrusherSE.Instance.SEDB.AudioClips[0];
        if (_audioSource_SE == null || _audioClip_SE == null)
            Debug.LogError("AudioSource of AudioClip is not assigned properly.");
        
        _modeSelectionBooks[_nextSceneIndex].SetSelection(true);
        _previousSelectIndex = _nextSceneIndex;
        
        // TODO ルールパネル系 (UIの方に移すかも)
        _rulesPanel.SetActive(false);
        _rulesButton.sprite = _rulesButtonSprites[0];
    }

    private void Update() {
        if (_isChangingScene || !_modeSelectionUIController.IsAnimationEnded)
            return;
        
        if (Input.GetButtonDown("Horizontal")) {
            var horizontalKey = Input.GetAxisRaw("Horizontal");
            if (horizontalKey > 0) {
                _nextSceneIndex++;
                if (_nextSceneIndex >= _nextSceneNames.Count)
                    _nextSceneIndex = 0;
            } else if (horizontalKey < 0) {
                _nextSceneIndex--;
                if (_nextSceneIndex < 0)
                    _nextSceneIndex = _nextSceneNames.Count - 1;
            }

            _modeSelectionBooks[_previousSelectIndex].SetSelection(false);
            _modeSelectionBooks[_nextSceneIndex].SetSelection(true);
            _previousSelectIndex = _nextSceneIndex;

            _audioClip_SE = CrusherSE.Instance.SEDB.AudioClips[1];
            _audioSource_SE.PlayOneShot(_audioClip_SE);
        }

        if (Input.GetButtonDown("Select")) {
            _isChangingScene = true;

            _audioClip_SE = CrusherSE.Instance.SEDB.AudioClips[0];
            _audioSource_SE.PlayOneShot(_audioClip_SE);

            GoNextSceneAsync(0.5f, _nextSceneNames[_nextSceneIndex]).Forget();
        }

        // TODO ルールパネル系 (UIの方に移すかも)
        if (!_rulesPanel.activeSelf && Input.GetButtonDown("Fire1")) {
            _rulesPanel.SetActive(true);
            _rulesButton.sprite = _rulesButtonSprites[1];
        } else if (_rulesPanel.activeSelf && Input.GetButtonDown("Jump")) {
            _rulesPanel.SetActive(false);
            _rulesButton.sprite = _rulesButtonSprites[0];
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
}
