using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ComicsGenerator : MonoBehaviour {
    private int _builderIndex = 0;    
    private int _crusherIndex = 0;

    #region Serialized Fields
    [SerializeField, Header("0...Openings, 1...BuilderWin, 2...CrusherWin")] private List<ComicsDB> _comicsDBs = null;
    [SerializeField] private List<Sprite> _builderSprites = new List<Sprite>();
    [SerializeField] private List<Sprite> _crusherSprites = new List<Sprite>();
    [SerializeField] private Image _builder = null;
    [SerializeField] private Image _crusher = null;
    [SerializeField] private TextMeshProUGUI _storyTitle = null;
    #endregion

    public string CSVName { get; private set; } = "";

    private void Awake() {
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
        _builderIndex = GameDirector.Instance.BuilderIndex;
        _crusherIndex = GameDirector.Instance.CrusherIndex;

        _builder.sprite = _builderSprites[_builderIndex];
        _crusher.sprite = _crusherSprites[_crusherIndex];
    }
}
