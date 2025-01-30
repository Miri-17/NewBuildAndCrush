using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ComicsGenerator : MonoBehaviour {
    private int _builderIndex = 0;    
    private int _crusherIndex = 0;

    #region Serialized Fields
    [SerializeField, Header("0...Openings, 1...BuilderWin, 2...CrusherWin")] private ComicsDB[] _comicsDBs = new ComicsDB[0];
    [SerializeField] private Sprite[] _builderSprites = new Sprite[0];
    [SerializeField] private Sprite[] _crusherSprites = new Sprite[0];
    [SerializeField] private Image _builder = null;
    [SerializeField] private Image _crusher = null;
    [SerializeField] private TextMeshProUGUI _storyTitle = null;
    #endregion

    public string CSVName { get; private set; } = "";

    private void Awake() {
        _builderIndex = GameDirector.Instance.BuilderIndex;
        _crusherIndex = GameDirector.Instance.CrusherIndex;
        
        // オープニングかビルダー勝利のエンディングかビルダー負けのエンディングか判断し, CSVファイルを取得する.
        if (GameDirector.Instance.IsOpening) {
            CSVName = _comicsDBs[0].GetCSVName(_builderIndex, _crusherIndex);
            _storyTitle.text = _comicsDBs[0].GetStoryTitle(_builderIndex, _crusherIndex);
        } else {
            if (GameDirector.Instance.IsBuilderWin) {
                CSVName = _comicsDBs[1].GetCSVName(_builderIndex, _crusherIndex);
                _storyTitle.text = _comicsDBs[1].GetStoryTitle(_builderIndex, _crusherIndex);
            } else {
                CSVName = _comicsDBs[2].GetCSVName(_builderIndex, _crusherIndex);
                _storyTitle.text = _comicsDBs[2].GetStoryTitle(_builderIndex, _crusherIndex);
            }
        }
    }

    private void Start() {
        _builder.sprite = _builderSprites[_builderIndex];
        _crusher.sprite = _crusherSprites[_crusherIndex];
    }
}
