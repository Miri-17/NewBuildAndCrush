using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;
using TMPro;

public class ResultController : MonoBehaviour {
    #region Private Fields
    private AudioSource _audioSource_BGM = null;
    private AudioSource _audioSource_SE = null;
    private AudioClip _audioClip_SE = null;
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
        _audioSource_BGM = BGM.Instance?.GetComponent<AudioSource>();
        _audioSource_SE = CrusherSE.Instance?.GetComponent<AudioSource>();
        if (_audioSource_BGM == null || _audioSource_SE == null || CrusherSE.Instance?.SEDB.AudioClips.Count < 3) {
            Debug.LogError("AudioSource of AudioClip is not set up correctly.");
            return;
        }

        _audioClip_SE = CrusherSE.Instance.SEDB.AudioClips[2];
        UpdateBuilder(GameDirector.Instance.BuilderIndex);
        UpdateCrusher(GameDirector.Instance.CrusherIndex);

        if (!_nextSceneNames?.Any() ?? true)
            Debug.LogError("Next scene names list is not set or contains invalid entries.");
    }

    private void Update() {
        if (!_audioSource_BGM.isPlaying)
            PlayLoopingBGM();

        if (_resultUIController.IsFadeOut && !_isChangingScene && Input.GetButtonDown("Select"))
            TransitionNextScene();
    }

    private void UpdateBuilder(int builderIndex) {
        var builder = _buildersDB.GetBuilder(builderIndex);

        _builderName.text = builder.Name;
        Vector3 builderPosition = GetBuilderPosition(builderIndex);
        var builderL2D = Instantiate(builder.Live2D, builderPosition, Quaternion.identity, GameObject.Find("Builder").transform);
        string animationState = GameDirector.Instance.IsBuilderWin ? "L2D_Win" : "L2D_Lose";
        builderL2D.GetComponent<Animator>().Play($"{builder.EnglishName}{animationState}");
    }

    private void UpdateCrusher(int crusherIndex) {
        var crusher = _crushersDB.GetCrusher(crusherIndex);

        _crusherName.text = crusher.Name;
        Vector3 crusherPosition = GetCrusherPosition(crusherIndex);
        var crusherL2D = Instantiate(crusher.Live2D, crusherPosition, Quaternion.identity, GameObject.Find("Crusher").transform);
        string animationState = GameDirector.Instance.IsBuilderWin ? "L2D_Lose" : "L2D_Win";
        crusherL2D.GetComponent<Animator>().Play($"{crusher.EnglishName}{animationState}");
    }
    
    private Vector3 GetBuilderPosition(int index) {
        switch (index) {
            case 0: return new Vector3(1.0f, 0, 80.0f);
            case 1: return new Vector3(0.75f, 0, 80.0f);
            case 2: return new Vector3(0.9f, 0, 80.0f);
            default: return new Vector3(0.85f, 0, 80.0f);
        }
    }
    
    private Vector3 GetCrusherPosition(int index) {
        switch (index) {
            case 0: return new Vector3(-0.45f, 0, 80.0f);
            case 1: return new Vector3(-0.95f, 0, 80.0f);
            case 2: return new Vector3(-0.87f, 0, 80.0f);
            default: return new Vector3(-0.65f, 0, 80.0f);
        }
    }

    private void PlayLoopingBGM() {
        _audioSource_BGM.clip = _audioClipLoop;
        _audioSource_BGM.loop = true;
        _audioSource_BGM.Play();
    }

    private void TransitionNextScene() {
        _isChangingScene = true;
        _audioSource_SE.PlayOneShot(_audioClip_SE);
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
