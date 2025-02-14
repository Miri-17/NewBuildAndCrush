using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CreditsUIController : MonoBehaviour {
    #region Serialized Fields
    [SerializeField] private CreditsController _creditsController = null;
    [SerializeField] private RectTransform _scrollingObjects = null;
    [SerializeField] private float _endAnchorPosY = 10725.0f;
    [SerializeField] private float _time = 90.0f;
    [SerializeField] private Image[] _fadeInImages = new Image[0];
    #endregion

    public static float TransitionDuration { get; private set; } = 1.0f;

    private async void Start() {
        // ゲームロゴを2秒間表示させてからスクロールを始める.
        await UniTask.Delay(2000);

        if (_scrollingObjects == null) return;

        _scrollingObjects.DOAnchorPosY(_endAnchorPosY, _time)
            .SetEase(Ease.Linear)
            .OnComplete(async () => await CreditsEnded())
            .SetLink(_scrollingObjects.gameObject);
    }

    // クレジットの終了後, 2秒待ってからモード選択に戻る.
    private async Task CreditsEnded() {
        await UniTask.Delay(2000);
        _creditsController.ChangeScene();
    }

    /// <summary>
    /// シーン遷移時に呼び, クラッシャー画面とビルダー画面に白フェードインをかける.
    /// </summary>
    public void FadeInImage() {
        var sequence = DOTween.Sequence();
        foreach (var image in _fadeInImages) {
            if (image == null) break;
            sequence.Join(
                image.DOFade(1.0f, TransitionDuration)
                    .SetEase(Ease.Linear)
                    .SetLink(image.gameObject)
            );
        }
    }
}
