using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;
using System.Threading;

public class TitleController : MonoBehaviour {
    #region Private Fields
    private AudioSource _audioSource_SE = null;
    private AudioClip _audioClip_SE = null;
    // シーン遷移関係
    private bool _isChangingScene = false;
    #endregion

    [SerializeField] private TitleUIController _titleUIController = null;
    // シーン遷移関係
    [SerializeField] private string _nextSceneName = "";

    private void Start() {
        if (SE.Instance == null) {
            Debug.LogError("SE instance is not available.");
            return;
        }

        if (string.IsNullOrEmpty(_nextSceneName)) {
            Debug.LogError("Next scene name is not set.");
            return;
        }

        _audioSource_SE = SE.Instance.GetComponent<AudioSource>();
        _audioClip_SE = SE.Instance.SEDB.AudioClips[0];
        if (_audioSource_SE == null || _audioClip_SE == null)
            Debug.LogError("AudioSource of AudioClip is not assigned properly.");
    }

    private void Update() {
        if (_isChangingScene || !Input.GetButtonDown("Select"))
            return;
        
        _isChangingScene = true;

        _audioSource_SE.PlayOneShot(_audioClip_SE);
        _titleUIController.TransitionUI();

        GoNextSceneAsync(0.5f, _nextSceneName).Forget();
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
