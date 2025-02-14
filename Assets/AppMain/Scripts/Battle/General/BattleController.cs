using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;

public class BattleController : MonoBehaviour {
    #region Private Fields
    private int _nextSceneIndex = 0;
    private DestroyableBuilder _destroyableBuilder = null;
    private AudioSource _audioSourceSE = null;
    private bool _isChangingScene = false;
    #endregion

    #region Serialized Fields
    [SerializeField] private DirectionController _directionController = null;
    [SerializeField] private BattleUIController _battleUIController = null;
    // シーン遷移関係
    [SerializeField] private string[] _nextSceneNames = new string[0];
    #endregion

    private void Start() {
        var builder = GameObject.FindGameObjectWithTag("Builder");
        _destroyableBuilder = builder.GetComponent<DestroyableBuilder>();

        _audioSourceSE = CrusherSE.Instance.GetComponent<AudioSource>();
    }

    private void Update() {
        // タイムアップによる次のシーン遷移処理.
        if (!_isChangingScene && _battleUIController.IsTimeUp && !_destroyableBuilder.IsCrushed) {
            _isChangingScene = true;

            if (_battleUIController.BuilderCurrentScore > _battleUIController.CrusherCurrentScore)
                GameDirector.Instance.IsBuilderWin = true;
            else
                GameDirector.Instance.IsBuilderWin = false;
            _audioSourceSE.PlayOneShot(CrusherSE.Instance.SEDB.AudioClips[6]);
            GoNextScene();
        } else if (!_isChangingScene && _destroyableBuilder.IsCrushed) {
            _isChangingScene = true;
            GameDirector.Instance.IsBuilderWin = false;
            _audioSourceSE.PlayOneShot(CrusherSE.Instance.SEDB.AudioClips[7]);
            GoNextScene();
        }
    }

    /// <summary>
    /// 次のシーンへ移動する. WagonControllerでも使用.
    /// </summary>
    public void GoNextScene() {
        _isChangingScene = true;
        if (!_battleUIController.IsTimeUp)
            _battleUIController.SetFinishText();

        _directionController.FinishDirection();
        
        // TODO CrusherControllerで扱っているCrusherKillCountsと
        // WagonControllerで扱っているWagonCrushCountsもここで管理できるようにしたい.
        GameDirector.Instance.BuilderScore = _battleUIController.BuilderCurrentScore;
        GameDirector.Instance.CrusherScore = _battleUIController.CrusherCurrentScore;
        
        GoNextSceneAsync(5.0f, _nextSceneNames[_nextSceneIndex]).Forget();
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
