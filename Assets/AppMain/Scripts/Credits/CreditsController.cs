using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;
using DG.Tweening;

public class CreditsController : MonoBehaviour {
    #region Private Fields
    private AudioSource _audioSource_BGM = null;
    private AudioSource _audioSource_SE = null;
    private AudioClip _audioClip_SE = null;
    // シーン遷移関係
    private int _nextSceneIndex = 0;
    private bool _isChangingScene = false;
    #endregion

    [SerializeField] private CreditsUIController _creditsUIController = null;
    // シーン遷移関係
    [SerializeField] private List<string> _nextSceneNames = new List<string>();

    private void Start() {
        if (SE.Instance == null) {
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

        _audioSource_BGM = BGM.Instance.GetComponent<AudioSource>();
        _audioSource_SE = SE.Instance.GetComponent<AudioSource>();
        _audioClip_SE = SE.Instance.SEDB.AudioClips[2];
        if (_audioSource_SE == null || _audioClip_SE == null)
            Debug.LogError("AudioSource of AudioClip is not assigned properly.");
    }

    private void Update() {
        if (_isChangingScene || !Input.GetButtonDown("Jump"))
            return;
        
        _audioSource_SE.PlayOneShot(_audioClip_SE);
        ChangeScene();
    }

    /// <summary>
    /// シーン遷移する
    /// </summary>
    public void ChangeScene() {
        if (_isChangingScene)
            return;
        
        _isChangingScene = true;
        _creditsUIController.FadeInImage();
        var duration = CreditsUIController.TransitionDuration;
        _audioSource_BGM.DOFade(0, duration)
            .SetLink(_audioSource_BGM.gameObject);
        GoNextSceneAsync(duration, _nextSceneNames[_nextSceneIndex]).Forget();
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
