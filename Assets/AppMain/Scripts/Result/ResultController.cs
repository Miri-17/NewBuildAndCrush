using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;
using TMPro;

public class ResultController : MonoBehaviour {
    #region Private Fields
    private AudioSource _audioSourceBGM = null;
    private AudioSource _audioSourceSE = null;
    private AudioClip _audioClipSE = null;
    // シーン遷移関係
    private int _nextSceneIndex = 0;
    private bool _isChangingScene = false;
    #endregion

    #region Serialized Fields
    [SerializeField] private ResultUIController _resultUIController = null;
    [SerializeField] private BuildersDB _buildersDB = null;
    [SerializeField] private CrushersDB _crushersDB = null;
    [SerializeField] private TextMeshProUGUI _builderName = null;
    [SerializeField] private TextMeshProUGUI _crusherName = null;
    [SerializeField] private AudioClip _audioClipLoop = null;
    // シーン遷移関係
    [SerializeField] private List<string> _nextSceneNames = new List<string>();
    #endregion

    private void Start() {
        _audioSourceBGM = BGM.Instance?.GetComponent<AudioSource>();
        _audioSourceSE = CrusherSE.Instance?.GetComponent<AudioSource>();
        if (_audioSourceBGM == null || _audioSourceSE == null || CrusherSE.Instance?.SEDB.AudioClips.Count < 3) {
            Debug.LogError("AudioSource of AudioClip is not set up correctly.");
            return;
        }

        _audioClipSE = CrusherSE.Instance.SEDB.AudioClips[0];
        UpdateBuilder(GameDirector.Instance.BuilderIndex);
        UpdateCrusher(GameDirector.Instance.CrusherIndex);

        if (!_nextSceneNames?.Any() ?? true)
            Debug.LogError("Next scene names list is not set or contains invalid entries.");
        
        if (GameDirector.Instance.BuilderIndex == GameDirector.Instance.CrusherIndex)
            _nextSceneIndex = 0;
        else
            _nextSceneIndex = 1;
    }

    private void Update() {
        if (!_audioSourceBGM.isPlaying)
            PlayLoopingBGM();

        if (_resultUIController.IsFadeOut && !_isChangingScene && Input.GetButtonDown("Select"))
            TransitionNextScene();
    }

    private void UpdateBuilder(int builderIndex) {
        var builder = _buildersDB.GetBuilder(builderIndex);

        _builderName.text = builder.Name;
        var builderL2D = Instantiate(builder.Live2D, new Vector3(0, 0, 0), Quaternion.identity);
        builderL2D.transform.parent = GameObject.Find("Builder").transform;
        builderL2D.transform.localPosition = GetBuilderPosition(builderIndex);
        string animationState = GameDirector.Instance.IsBuilderWin ? "L2D_Win" : "L2D_Lose";
        builderL2D.GetComponent<Animator>().Play($"{builder.EnglishName}{animationState}");
    }

    private void UpdateCrusher(int crusherIndex) {
        var crusher = _crushersDB.GetCrusher(crusherIndex);

        _crusherName.text = crusher.Name;
        var crusherL2D = Instantiate(crusher.Live2D, new Vector3(0, 0, 0), Quaternion.identity);
        crusherL2D.transform.parent = GameObject.Find("Crusher").transform;
        crusherL2D.transform.localPosition = GetCrusherPosition(crusherIndex);
        string animationState = GameDirector.Instance.IsBuilderWin ? "L2D_Lose" : "L2D_Win";
        crusherL2D.GetComponent<Animator>().Play($"{crusher.EnglishName}{animationState}");
    }
    
    private Vector3 GetBuilderPosition(int index) {
        switch (index) {
            case 0: return new Vector3(5.0f, 0, 80.0f);
            case 1: return new Vector3(3.2f, 0, 80.0f);
            case 2: return new Vector3(5.0f, 0, 80.0f);
            default: return new Vector3(4.5f, 0, 80.0f);
        }
    }
    
    private Vector3 GetCrusherPosition(int index) {
        switch (index) {
            case 0: return new Vector3(-2.5f, 0, 80.0f);
            case 1: return new Vector3(-4.5f, 0, 80.0f);
            case 2: return new Vector3(-4.5f, 0, 80.0f);
            default: return new Vector3(-3.5f, 0, 80.0f);
        }
    }

    private void PlayLoopingBGM() {
        _audioSourceBGM.clip = _audioClipLoop;
        _audioSourceBGM.loop = true;
        _audioSourceBGM.Play();
    }

    private void TransitionNextScene() {
        _isChangingScene = true;
        // PlayLoopingBGMに入る前にストーリーに行くと、BGMが止まるのを防ぐ.
        _audioSourceBGM.loop = true;
        _audioSourceSE.PlayOneShot(_audioClipSE);
        GameDirector.Instance.IsOpening = false;
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
