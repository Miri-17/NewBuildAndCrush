using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class CreditsController : MonoBehaviour {
    #region Private Fields
    private AudioSource _audioSource_BGM = null;
    private AudioSource _audioSource_SE = null;
    private AudioClip _audioClip_SE = null;
    // シーン遷移関係.
    private int _nextSceneIndex = 0;
    private bool _isChangingScene = false;
    #endregion

    [SerializeField] private CreditsUIController _creditsUIController = null;
    // シーン遷移関係.
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

        _audioSource_BGM = BGM.Instance.GetComponent<AudioSource>();
        _audioSource_SE = CrusherSE.Instance.GetComponent<AudioSource>();
        _audioClip_SE = CrusherSE.Instance.SEDB.AudioClips[2];
        if (_audioSource_SE == null || _audioClip_SE == null)
            Debug.LogError("AudioSource of AudioClip is not assigned properly.");
        
        GameDirector.Instance.PreviousSceneName = "Credits";
    }

    private void Update() {
        if (_isChangingScene || !Input.GetButtonDown("Jump"))
            return;
            
        _audioSource_SE.PlayOneShot(_audioClip_SE);
        ChangeScene();
    }

    // 次のシーンに遷移する.
    private async UniTaskVoid GoNextSceneAsync(float duration, string nextSceneName) {
        try {
            await UniTask.Delay((int)(duration * 1000), cancellationToken: this.GetCancellationTokenOnDestroy());
            SceneManager.LoadScene(nextSceneName);
        } catch (System.Exception e) {
            Debug.LogError($"Scene transition failed: {e.Message}");
        }
    }

    /// <summary>
    /// シーン遷移とその際のUIの動きや音楽の制御を行う.
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
}
