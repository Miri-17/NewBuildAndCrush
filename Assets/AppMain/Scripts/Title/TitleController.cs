using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleController : MonoBehaviour {
    #region Private Fields
    private AudioSource _audioSource_SE = null;
    private AudioClip _audioClip_SE = null;
    // シーン遷移関係
    private int _nextSceneIndex = 0;
    private bool _isChangingScene = false;
    #endregion

    [SerializeField] private TitleUIController _titleUIController = null;
    // シーン遷移関係
    [SerializeField] private string[] _nextSceneNames = new string[0];

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
    }

    private void Update() {
        if (_isChangingScene || !Input.GetButtonDown("Select"))
            return;
        
        _isChangingScene = true;

        _audioSource_SE.PlayOneShot(_audioClip_SE);
        _titleUIController.TransitionUI();

        GoNextSceneAsync(TitleUIController.TransitionDuration, _nextSceneNames[_nextSceneIndex]).Forget();
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
