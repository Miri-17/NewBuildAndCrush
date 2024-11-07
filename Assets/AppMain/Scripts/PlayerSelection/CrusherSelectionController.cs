using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;
using Cysharp.Threading.Tasks.CompilerServices;
using DG.Tweening;

public class CrusherSelectionController : MonoBehaviour {
    #region Private Fields
    private GameObject _crusherL2D = null;
    private CrusherDB _crusher = null;
    private int _crusherIndex = 0;
    private int _previousCrusherIndex = 0;
    private int _crusherCount = 0;
    private bool _isCrusherSelected = false;
    private Animator _crusherAnimator = null;
    private AudioSource _audioSourceBGM = null;
    private AudioSource _audioSourceSE = null;
    private AudioClip _audioClipSE = null;
    // シーン遷移関係
    private bool _isChangingScene = false;
    #endregion

    #region Serialized Fields
    [SerializeField] private CrushersDB _crushersDB = null;
    [SerializeField] private CrusherSelectionUIController _crusherSelectionUIController = null;
    [SerializeField] private BuilderSelectionController _builderSelectionController = null;
    [SerializeField] private TextMeshProUGUI _crusherNickname = null;
    [SerializeField] private TextMeshProUGUI _crusherName = null;
    [SerializeField] private TextMeshProUGUI _crusherDescription = null;
    [SerializeField] private GameObject _statusPanel = null;
    // WaitingPanel
    [SerializeField, Header("0...Crusher, 1...Builder")] private List<GameObject> _waitingPanels = new List<GameObject>();
    [SerializeField] private List<GameObject> _waitingCharacters = new List<GameObject>();
    // ConfirmPanel
    [SerializeField, Header("0...Crusher, 1...Builder")] private List<GameObject> _confirmPanels = new List<GameObject>();
    // FadeInImage
    [SerializeField] private List<Image> _fadeInImages = new List<Image>();
    // シーン遷移関係
    [SerializeField] private List<string> _nextSceneNames = new List<string>();
    #endregion

    [HideInInspector] public bool IsSetConfirmPanel = false;

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

        _audioSourceBGM = BGM.Instance.GetComponent<AudioSource>();
        _audioSourceSE = CrusherSE.Instance.GetComponent<AudioSource>();
        
        _crusherCount = _crushersDB.CrusherCount;
        UpdateCrusher(_crusherIndex);

        _statusPanel.SetActive(false);
        _waitingPanels[0].SetActive(false);
        foreach (var confirmPanel in _confirmPanels)
            confirmPanel.SetActive(false);

        GameDirector.Instance.PreviousSceneName = "PlayerSelection";
    }

    private void Update() {
        if (_isChangingScene) return;

        if (_isCrusherSelected) {
            // Debug.Log("クラッシャー選択状態です");
            if (_confirmPanels[0].activeSelf) {
                if (Input.GetButtonDown("Select")) {
                    _isChangingScene = true;

                    _audioSourceBGM.Stop();

                    _audioClipSE = CrusherSE.Instance.SEDB.AudioClips[4];
                    _audioSourceSE.PlayOneShot(_audioClipSE);
                    foreach (var confirmPanel in _confirmPanels)
                        confirmPanel.SetActive(false);
                    _crusherAnimator.Play(_crusher.EnglishName + "L2D_Select");
                    _builderSelectionController.BuilderAnimator.Play(_builderSelectionController.Builder.EnglishName + "L2D_Select");
                    
                    GameDirector.Instance.BuilderIndex = _builderSelectionController.BuilderIndex;
                    GameDirector.Instance.CrusherIndex = _crusherIndex;
                    GameDirector.Instance.IsOpening = true;
                    GameDirector.Instance.BuilderScore = 0;
                    GameDirector.Instance.CrusherScore = 0;
                    GameDirector.Instance.CrusherKillCounts = 0;
                    GameDirector.Instance.WagonCrushCounts = 0;
                    FadeInImageAsync(1.8f, GameDirector.Instance.BuilderIndex == GameDirector.Instance.CrusherIndex).Forget();
                } else if (Input.GetButtonDown("Jump")) {
                    foreach (var confirmPanel in _confirmPanels)
                        confirmPanel.SetActive(false);
                    
                    _isCrusherSelected = false;
                    _builderSelectionController.IsBuilderSelected = false;
                    IsSetConfirmPanel = false;

                    _audioClipSE = CrusherSE.Instance.SEDB.AudioClips[2];
                    _audioSourceSE.PlayOneShot(_audioClipSE);
                }
            } else {
                if (!_confirmPanels[0].activeSelf && _builderSelectionController.IsBuilderSelected) {
                    // IsSettingConfirmPanel = true;
                    IsSetConfirmPanel = true;   // ビルダーに選択の余地を与えない用
                    SetConfirmPanel();
                } else if (Input.GetButtonDown("Jump")) {
                    _isCrusherSelected = false;

                    _audioClipSE = CrusherSE.Instance.SEDB.AudioClips[2];
                    _audioSourceSE.PlayOneShot(_audioClipSE);

                    _crusherAnimator.enabled = true;
                    _waitingPanels[0].SetActive(false);
                }
            }
        } else {
            if (Input.GetButtonDown("Select")) {
                _isCrusherSelected = true;

                _audioClipSE = CrusherSE.Instance.SEDB.AudioClips[0];
                _audioSourceSE.PlayOneShot(_audioClipSE);

                _crusherAnimator.enabled = false;
                _waitingPanels[0].SetActive(true);
            } else if (Input.GetButtonDown("Horizontal")) {
                var horizontalKey = Input.GetAxisRaw("Horizontal");
                if (horizontalKey > 0) {
                    _crusherIndex++;
                    if (_crusherIndex >= _crusherCount)
                        _crusherIndex = 0;
                } else if (horizontalKey < 0) {
                    _crusherIndex--;
                    if (_crusherIndex < 0)
                        _crusherIndex = _crusherCount - 1;
                }

                _audioClipSE = CrusherSE.Instance.SEDB.AudioClips[1];
                _audioSourceSE.PlayOneShot(_audioClipSE);

                if (_crusherL2D != null)
                    Destroy(_crusherL2D);
                UpdateCrusher(_crusherIndex);
            } else if (Input.GetButtonDown("Jump")) {
                _isChangingScene = true;

                _audioClipSE = CrusherSE.Instance.SEDB.AudioClips[2];
                _audioSourceSE.PlayOneShot(_audioClipSE);

                GoNextSceneAsync(0.5f, _nextSceneNames[0]).Forget();
            } else if (Input.GetButtonDown("Fire1")) {
                _statusPanel.SetActive(!_statusPanel.activeSelf);
            }
        }
    }

    private void UpdateCrusher(int crusherIndex) {
        _crusher = _crushersDB.GetCrusher(crusherIndex);
        var crusherInfo = _crushersDB.GetCrusherInfo(crusherIndex);

        _crusherName.text = _crusher.Name;
        _crusherL2D = Instantiate(_crusher.Live2D, new Vector3(0, 0, 0), Quaternion.identity);
        _crusherL2D.transform.parent = GameObject.Find("Crusher").transform;
        _crusherL2D.transform.localPosition = GetCrusherPosition(crusherIndex);
        _crusherAnimator = _crusherL2D.GetComponent<Animator>();

        _crusherNickname.text = crusherInfo.Nickname;
        _crusherDescription.text = crusherInfo.Description;
        if (crusherIndex == 0 || crusherIndex == 1)
            _crusherDescription.lineSpacing = 80;
        else
            _crusherDescription.lineSpacing = 60;
        
        _crusherSelectionUIController.SetSprites(crusherIndex);
        
        _waitingCharacters[_previousCrusherIndex].gameObject.SetActive(false);
        _waitingCharacters[crusherIndex].gameObject.SetActive(true);

        _previousCrusherIndex = crusherIndex;
    }

    private Vector3 GetCrusherPosition(int index) {
        switch (index) {
            case 0: return new Vector3(-2.7f, 0, 80.0f);
            case 1: return new Vector3(-3.9f, 0, 80.0f);
            case 2: return new Vector3(-4.25f, 0, 80.0f);
            default: return new Vector3(-3.2f, 0, 80.0f);
        }
    }

    private async UniTaskVoid FadeInImageAsync(float duration, bool isStory) {
        try {
            await UniTask.Delay((int)(duration * 1000), cancellationToken: this.GetCancellationTokenOnDestroy());
            if (isStory) {
                _fadeInImages[0].DOFade(1.0f, 0.5f)
                    .SetEase(Ease.Linear)
                    .SetLink(_fadeInImages[0].gameObject);
                _fadeInImages[1].DOFade(1.0f, 0.5f)
                    .SetEase(Ease.Linear)
                    .SetLink(_fadeInImages[1].gameObject)
                    .OnComplete(() => GoNextSceneAsync(0.5f, _nextSceneNames[1]).Forget());
            } else {
                _fadeInImages[0].DOFade(1.0f, 0.5f)
                    .SetEase(Ease.Linear)
                    .SetLink(_fadeInImages[0].gameObject);
                _fadeInImages[1].DOFade(1.0f, 0.5f)
                    .SetEase(Ease.Linear)
                    .SetLink(_fadeInImages[1].gameObject)
                    .OnComplete(() => GoNextSceneAsync(0.5f, _nextSceneNames[2]).Forget());
            }
        } catch (System.Exception e) {
            Debug.LogError($"Image fading-in failed: {e.Message}");
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

    private void SetConfirmPanel() {
        foreach (var waitingPanel in _waitingPanels)
            waitingPanel.SetActive(false);
        
        _builderSelectionController.BuilderAnimator.enabled = true;
        _crusherAnimator.enabled = true;
        
        foreach (var confirmPanel in _confirmPanels)
            confirmPanel.SetActive(true);
    }
}
