using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

// TODO WaitingPanel.csと統合したい.
public class BattleWaitingPanel : MonoBehaviour {
    private RectTransform _canvasGroupRectTransform = null;
    private Color _bgColor = Color.white;

    #region Serialized Fields
    [SerializeField] private Image _bg = null;
    [SerializeField] private float _initialBgAlpha = 0.58f;
    [SerializeField] private CanvasGroup _canvasGroup = null;
    [SerializeField] private GameObject _waitingCharacterParent = null;
    [SerializeField] private GameObject[] _waitingCharacterPrefabs = new GameObject[0];
    [SerializeField] private bool _isBuilder = true;
    #endregion

    private void Awake() {
        _canvasGroupRectTransform = _canvasGroup.GetComponent<RectTransform>();
    }

    private void Start() {
        if (_isBuilder) {
            var builderIndex = GameDirector.Instance.BuilderIndex;
            var _waitingCharacter = Instantiate(_waitingCharacterPrefabs[builderIndex], _waitingCharacterParent.transform);
            _waitingCharacter.transform.localPosition = GetBuilderPosition(builderIndex);
        } else {
            var crusherIndex = GameDirector.Instance.CrusherIndex;
            var _waitingCharacter = Instantiate(_waitingCharacterPrefabs[crusherIndex], _waitingCharacterParent.transform);
            _waitingCharacter.transform.localPosition = GetCrusherPosition(crusherIndex);
        }
    }

    private Vector3 GetBuilderPosition(int index) {
        switch (index) {
            case 0: return new Vector3(0, 112f, 0);
            case 1: return new Vector3(0, 140f, 0);
            case 2: return new Vector3(0, 112f, 0);
            default: return new Vector3(0, 112f, 0);
        }
    }

    private Vector3 GetCrusherPosition(int index) {
        switch (index) {
            case 0: return new Vector3(0, 120f, 0);
            case 1: return new Vector3(0, 110f, 0);
            case 2: return new Vector3(0, 114f, 0);
            default: return new Vector3(0, 112f, 0);
        }
    }

    private void OnEnable() {
        _bgColor = _bg.color;
        _bgColor.a = _initialBgAlpha;
        _bg.color = _bgColor;
        _bg.DOFade(0.78f, 0.5f)
            .SetEase(Ease.Linear)
            .SetLink(_bg.gameObject);
        
        _canvasGroupRectTransform.anchoredPosition = new Vector2(0, -100.0f);
        _canvasGroupRectTransform.DOAnchorPosY(0, 0.5f)
            .SetEase(Ease.Linear)
            .SetLink(_canvasGroupRectTransform.gameObject);

        _canvasGroup.alpha = 0;
        _canvasGroup.DOFade(1.0f, 0.5f)
            .SetEase(Ease.Linear)
            .SetLink(_canvasGroup.gameObject);
    }
}
