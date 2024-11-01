using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;

// TODO 早く切り替えた時のバグどうにかしたい
public class SoundListController : MonoBehaviour {
    #region Private Fields
    private int _audioClipCount = 0;
    private AudioSource _audioSourceBGM = null;
    private int _previousSelectionIndex = -1;
    // シーン遷移関係.
    private bool _isChangingScene = false;
    private AudioSource _audioSourceSE = null;
    #endregion

    #region Serialized Fields
    [SerializeField] private SoundListUIController _soundListUIController = null;
    [SerializeField] private List<FadeInOutLoopAnimation> _fadeInOutLoopAnimations = null;
    [SerializeField] private List<string> _nextSceneNames = new List<string>();
    #endregion

    public int SoundIndex {get; private set; } = 0;

    private void Start() {
        if (CrusherSE.Instance == null || !_nextSceneNames?.Any() == true) {
            Debug.LogError("No objects required for initialisation.");
            return;
        }

        _audioSourceSE = CrusherSE.Instance.GetComponent<AudioSource>();
        _audioSourceBGM = BGM.Instance.GetComponent<AudioSource>();
        _audioClipCount = BGM.Instance.BGMDB.GetAudioClipCount();

        GameDirector.Instance.PreviousSceneName = "SoundList";
    }

    private void Update() {
        if (_isChangingScene) return;
        
        if (Input.GetButtonDown("Vertical")) {
            var verticalKey = Input.GetAxisRaw("Vertical");
            if (verticalKey < 0) {
                SoundIndex++;
                if (SoundIndex >= _audioClipCount)
                    SoundIndex = 0;
            } else if (verticalKey > 0) {
                SoundIndex--;
                if (SoundIndex < 0)
                    SoundIndex = _audioClipCount - 1;
            }

            if (!_audioSourceBGM.isPlaying)
                _soundListUIController.SetRecordSprite(SoundIndex);
            
            if (SoundIndex < 13)
                _soundListUIController.SetAnchoredPosition(new Vector2(0, -9.0f));
            else
                _soundListUIController.SetAnchoredPosition(new Vector2(0, 80.0f));

            _audioSourceSE.PlayOneShot(CrusherSE.Instance.SEDB.AudioClips[1]);
        }

        if (Input.GetButtonDown("Select")) {
            // if (_soundListUIController.IsFadingIllustration) return;
            if (_audioSourceBGM.isPlaying && _previousSelectionIndex == SoundIndex) {
                _audioSourceBGM.Stop();
                _soundListUIController.ResetPlayback();
                _fadeInOutLoopAnimations[_previousSelectionIndex].AnimationOnOff(false);
                return;
            }

            // 前回のBGMに対する処理
            _audioSourceBGM.Stop();
            _soundListUIController.ResetPlayback();
            if (_previousSelectionIndex != -1)
                _fadeInOutLoopAnimations[_previousSelectionIndex].AnimationOnOff(false);
            
            // 今回のBGMに対する処理
            _audioSourceBGM.clip = BGM.Instance.BGMDB.AudioClips[SoundIndex];
            _audioSourceBGM.Play();
            _soundListUIController.StartPlayback();
            _fadeInOutLoopAnimations[SoundIndex].AnimationOnOff(true);

            _previousSelectionIndex = SoundIndex;
        }

        if (Input.GetButtonDown("Jump")) {
            _isChangingScene = true;
            PlaySceneTransitionSound();
            GoNextSceneAsync(0.5f, _nextSceneNames[0]).Forget();
        }
    }

    private void PlaySceneTransitionSound() {
        _audioSourceSE.PlayOneShot(CrusherSE.Instance.SEDB.AudioClips[2]);
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
