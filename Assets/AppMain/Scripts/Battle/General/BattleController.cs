using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;

public class BattleController : MonoBehaviour {
    private bool _isChangingScene = false;
    private int _nextSceneIndex = 0;

    [SerializeField] private BattleUIController _battleUIController = null;
    // シーン遷移関係
    [SerializeField] private List<string> _nextSceneNames = new List<string>();

    private void Update() {
        if (!_isChangingScene && _battleUIController.IsTimeUp) {
            // _isChangingScene = true;
            GoNextScene();
        }
    }

    /// <summary>
    /// 次のシーンへ移動する. WagonControllerでも使用.
    /// </summary>
    public void GoNextScene() {
        _isChangingScene = true;
        GameDirector.Instance.BuilderScore = _battleUIController.BuilderCurrentScore;
        GameDirector.Instance.CrusherScore = _battleUIController.CrusherCurrentScore;
        // TODO CrusherControllerで扱っているCrusherKillCountsと
        // WagonControllerで扱っているWagonCrushCountsもここで管理できるようにしたい.
        if (_battleUIController.BuilderCurrentScore > _battleUIController.CrusherCurrentScore)
            GameDirector.Instance.IsBuilderWin = true;
        else
            GameDirector.Instance.IsBuilderWin = false;
        
        GoNextSceneAsync(0.5f, _nextSceneNames[_nextSceneIndex]).Forget();
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
