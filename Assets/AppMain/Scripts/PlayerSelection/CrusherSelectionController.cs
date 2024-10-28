using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;

public class CrusherSelectionController : MonoBehaviour {
    private GameObject _crusherL2D = null;
    private CrusherDB _crusher = null;
    private int _crusherIndex = 0;
    private int _crusherCount = 0;
    private bool _isCrusherSelected = false;
    private Animator _crusherAnimator = null;
    private AudioSource _audioSource_SE = null;
    private AudioClip _audioClip_SE = null;
    // シーン遷移関係
    private bool _isChangingScene = false;

    [SerializeField] private CrushersDB _crushersDB = null;
    [SerializeField] private TextMeshProUGUI _crusherName = null;
    [SerializeField] private TextMeshProUGUI _crusherNickname = null;
    [SerializeField] private TextMeshProUGUI _crusherDescription = null;
    // シーン遷移関係
    [SerializeField] private List<string> _nextSceneNames = new List<string>();

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
        
        _crusherCount = _crushersDB.CrusherCount;
        UpdateCrusher(_crusherIndex);
    }

    private void Update() {
        if (_isChangingScene)
            return;
        
        if (_isCrusherSelected) {
            if (Input.GetButtonDown("Jump")) {
                _isCrusherSelected = false;

                _crusherAnimator.Play(_crusher.EnglishName + "L2D_Idle");
            }
        } else {
            if (Input.GetButtonDown("Select")) {
                _isCrusherSelected = true;

                _audioClip_SE = CrusherSE.Instance.SEDB.AudioClips[0];
                _audioSource_SE.PlayOneShot(_audioClip_SE);
                
                _crusherAnimator.Play(_crusher.EnglishName + "L2D_Select");
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

                _audioClip_SE = CrusherSE.Instance.SEDB.AudioClips[1];
                _audioSource_SE.PlayOneShot(_audioClip_SE);

                if (_crusherL2D != null)
                    Destroy(_crusherL2D);
                UpdateCrusher(_crusherIndex);
            } else if (Input.GetButtonDown("Jump")) {
                _isChangingScene = true;

                _audioClip_SE = CrusherSE.Instance.SEDB.AudioClips[2];
                _audioSource_SE.PlayOneShot(_audioClip_SE);

                GoNextSceneAsync(0.5f, _nextSceneNames[0]).Forget();
            }
        }
    }

    private void UpdateCrusher(int crusherIndex) {
        _crusher = _crushersDB.GetCrusher(crusherIndex);
        var crusherInfo = _crushersDB.GetCrusherInfo(crusherIndex);

        _crusherName.text = _crusher.Name;
        _crusherL2D = Instantiate(_crusher.Live2D, new Vector3(0, 0, 80.0f), Quaternion.identity);
        _crusherL2D.transform.SetParent(GameObject.Find("Crusher").transform, false);
        _crusherAnimator = _crusherL2D.GetComponent<Animator>();

        _crusherNickname.text = crusherInfo.Nickname;
        _crusherDescription.text = crusherInfo.Description;
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
