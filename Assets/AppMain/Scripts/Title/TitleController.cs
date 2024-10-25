using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Cysharp.Threading.Tasks;
using System.Threading;

public class TitleController : MonoBehaviour {
    #region
    private bool _firstPushY = false;
    private CancellationToken _token = default;
    private AudioSource _audioSource_SE = null;
    private AudioClip _audioClip_SE = null;
    #endregion

    #region
    [SerializeField] private TextMeshProUGUI _text = null;
    [SerializeField] private RectTransform _title = null;
    [SerializeField, Header("横・縦に拡大する速さ")] private float _expandSpeed = 5000.0f;
    [SerializeField] private string _nextSceneName = "";
    #endregion

    private void Start() {
        _audioSource_SE = SE.Instance.GetComponent<AudioSource>();
        _audioClip_SE = SE.Instance.SEDB.AudioClips[0];
    }

    private void Update() {
        if (!_firstPushY && Input.GetButtonDown("Select")) {
            _firstPushY = true;

            _audioSource_SE.PlayOneShot(_audioClip_SE);
            _text.enabled = false;

            _token = this.GetCancellationTokenOnDestroy();
            GoNextSceneAsync(0.5f, _nextSceneName).Forget();
        }

        // TODO DoTweenを使う様に変更
        if (_firstPushY) {
            Vector2 newSize = _title.sizeDelta;
            newSize.x += _expandSpeed * Time.deltaTime * 5;
            newSize.y += _expandSpeed * Time.deltaTime * 5;
            _title.sizeDelta = newSize;
        }
    }

    private async UniTaskVoid GoNextSceneAsync(float duration, string nextSceneName) {

        await UniTask.Delay((int)(duration * 1000), cancellationToken: _token);

        SceneManager.LoadScene(nextSceneName);
    }
}
