using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class StoryController : MonoBehaviour {
    #region Private Fields
    private ComicsPanelDB _comicsPanelDB = null;
    // 次へフラグ.
    private bool _goToNextPage = false;
    // 次へ行けるフラグ.
    private bool _currentPageCompleted = false;
    // スキップフラグ.
    private bool _isSkipped = false;
    // 現在のキャラクター情報.
    private List<StoryData> _talks = new List<StoryData>();
    // タグ判定用.
    private bool _isInTag = false;
    private string _tagStrings = "";
    // シーン遷移関係.
    private AudioSource _audioSourceBGM = null;
    private AudioSource _audioSourceSE = null;
    private bool _isChangingScene = false;
    private string _csvName = "";
    private bool _canSkip = true;
    #endregion

    #region Serialized Fields
    [SerializeField] private TextMeshProUGUI _comicsText = null;
    [SerializeField] private FadeInOutLoopAnimation _fadeInOutLoopAnimation = null;
    [SerializeField] CSVReader _csvReader = null;
    [SerializeField] ComicsGenerator _comicsGenerator = null;
    [SerializeField] private Image _comicsPanel = null;
    [SerializeField] private StoriesUIController _storiesUIController = null;
    // Story特有.
    [Header("ビルダーのインデックス順")]
    [SerializeField] private List<ComicsPanelDB> _openingsPanelDBs = new List<ComicsPanelDB>();
    [SerializeField] private List<ComicsPanelDB> _builderWinPanelDBs = new List<ComicsPanelDB>();
    [SerializeField] private List<ComicsPanelDB> _crusherWinPanelDBs = new List<ComicsPanelDB>();

    [SerializeField] private Image _endingFadeImage = null;
    #endregion
    
    private void Start() {
        if (GameDirector.Instance.IsOpening) {
            _comicsPanelDB = _openingsPanelDBs[GameDirector.Instance.BuilderIndex];
        } else {
            if (GameDirector.Instance.IsBuilderWin)
                _comicsPanelDB = _builderWinPanelDBs[GameDirector.Instance.BuilderIndex];
            else
                _comicsPanelDB = _crusherWinPanelDBs[GameDirector.Instance.BuilderIndex];
        }
        _csvName = _comicsGenerator.CSVName;
        
        _audioSourceBGM = BGM.Instance.GetComponent<AudioSource>();
        _audioSourceSE = CrusherSE.Instance.GetComponent<AudioSource>();
        
        StartTalk();
    }

    private void Update() {
        if (!_canSkip) return;

        if (!_isChangingScene && Input.GetButtonDown("Fire1")) {
            _isChangingScene = true;
            if (GameDirector.Instance.IsOpening) {
                _audioSourceSE.PlayOneShot(CrusherSE.Instance.SEDB.AudioClips[0]);
                _storiesUIController.TransitionUI(0.5f);
                GoNextSceneAsync(0.5f, "Battle").Forget();
            } else {
                _audioSourceSE.PlayOneShot(CrusherSE.Instance.SEDB.AudioClips[2]);
                _audioSourceBGM.DOFade(0, 1.0f)
                    .SetEase(Ease.Linear)
                    .SetLink(_audioSourceBGM.gameObject);
                _storiesUIController.TransitionUI(1.0f);
                GoNextSceneAsync(1.0f, "ModeSelection").Forget();
            }
        }
        if (Input.GetButtonDown("Select")) {
            if (_currentPageCompleted)
                _goToNextPage = true;
            else
                _isSkipped = true;
        }
    }

    private async void StartTalk() {
        Open();
        _talks = _csvReader.GetCSVData(_csvName);

        await (GameDirector.Instance.IsOpening || GameDirector.Instance.BuilderIndex != 1
            ? TalkStart(_talks)
            : AliceEndingStart(_talks));

        if (_isChangingScene) return;

        _isChangingScene = true;
        if (GameDirector.Instance.IsOpening) {
            _audioSourceSE.PlayOneShot(CrusherSE.Instance.SEDB.AudioClips[0]);
            _storiesUIController.TransitionUI(0.5f);
            GoNextSceneAsync(0.5f, "Battle").Forget();
        } else {
            _audioSourceBGM.DOFade(0, 1.0f)
                .SetEase(Ease.Linear)
                .SetLink(_audioSourceBGM.gameObject);
            _storiesUIController.TransitionUI(1.0f);
            GoNextSceneAsync(1.0f, "ModeSelection").Forget();
        }
    }

    // 会話の開始.
    private async UniTask TalkStart(List<StoryData> talkList, float wordInterval = 0.08f) {
        Debug.Log("talkList Count: " + talkList.Count);
        int i = 0;
        foreach (var talk in talkList) {
            if (string.IsNullOrEmpty(talk.ComicPanel) == false) {
                SetComicsPanel(talk.ComicPanel);
            }

            _comicsText.text = "";
            _goToNextPage = false;
            _currentPageCompleted = false;
            _isSkipped = false;
            _fadeInOutLoopAnimation.AnimationOnOff(false);

            await UniTask.Delay((int)(0.5f * 1000f));

            foreach (char word in talk.Talk) {
                // タグ判定用
                bool isCloseTag = false;
                if (word.ToString() == "<") {
                    Debug.Log("<です。");
                    _isInTag = true;
                } else if (word.ToString() == ">") {
                    Debug.Log(">です。");
                    _isInTag = false;
                    isCloseTag = true;
                }

                if (_isInTag == false && isCloseTag == false && string.IsNullOrEmpty(_tagStrings) == false) {
                    var _word = _tagStrings + word;
                    _comicsText.text += _word;
                    _tagStrings = "";
                } else if (_isInTag == true || isCloseTag == true) {
                    _tagStrings += word;
                    Debug.Log("Tag内です。");
                    continue;
                } else {
                    _comicsText.text += word;
                }
                await UniTask.Delay((int)(wordInterval * 1000f));

                if (_isSkipped == true) {
                    _comicsText.text = talk.Talk;
                    break;
                }
            }

            _currentPageCompleted = true;
            _fadeInOutLoopAnimation.AnimationOnOff(true);

            await UniTask.WaitUntil(() => _goToNextPage == true);
            
            i++;
            if (i == talkList.Count - 1) {
                Debug.Log("Yを消し、Rの文章を変える");
                Destroy(_fadeInOutLoopAnimation.gameObject);
                _storiesUIController.ChangeText();
            }
        }
    }

    /// <summary>
    /// ウィンドウを開く
    /// </summary>
    /// <param name="initName"></param>
    /// <param name="initText"></param>
    /// <returns></returns>
    private void Open(string initName = "", string initText = "") {
        _comicsText.text = initText;
        _fadeInOutLoopAnimation.AnimationOnOff(false);
    }

    private void SetComicsPanel(string comicsPanel) {
        Sprite comicsPanelSprite = _comicsPanelDB.GetComicsPanelSprite(comicsPanel);

        string currentComicsPanel = _comicsPanel.sprite.name;
        if (currentComicsPanel == comicsPanelSprite.name) {
            Debug.Log("同じ背景なので変更をスキップします");
            return;
        }
        
        _comicsPanel.gameObject.SetActive(false);
        _comicsPanel.sprite = comicsPanelSprite;

        _comicsPanel.gameObject.SetActive(true);
    }
    
#region Add for Alice Ending
// TODO あまり良くないコードなので直したい.
    private async UniTask AliceEndingStart(List<StoryData> talkList, float wordInterval = 0.08f) {
        Debug.Log("アリスのエンディングです");
        Debug.Log("talkList Count: " + talkList.Count);
        int i = 0;
        foreach (var talk in talkList) {
            if (string.IsNullOrEmpty(talk.ComicPanel) == false) {
                Debug.Log("talkCount: " + i);
                if (i == 16 && GameDirector.Instance.IsBuilderWin)
                    SetAliceEndingPanel(talk.ComicPanel);
                    // await SetAliceEndingPanel(talk.ComicPanel);
                else if (i == 26 && !GameDirector.Instance.IsBuilderWin)
                    SetAliceEndingPanel(talk.ComicPanel);
                    // await SetAliceEndingPanel(talk.ComicPanel);
                else
                    SetComicsPanel(talk.ComicPanel);
            }

            _comicsText.text = "";
            _goToNextPage = false;
            _currentPageCompleted = false;
            _isSkipped = false;
            _fadeInOutLoopAnimation.AnimationOnOff(false);

            await UniTask.Delay((int)(0.5f * 1000f));

            foreach (char word in talk.Talk) {
                // タグ判定用
                bool isCloseTag = false;
                if (word.ToString() == "<") {
                    Debug.Log("<です。");
                    _isInTag = true;
                } else if (word.ToString() == ">") {
                    Debug.Log(">です。");
                    _isInTag = false;
                    isCloseTag = true;
                }

                if (_isInTag == false && isCloseTag == false && string.IsNullOrEmpty(_tagStrings) == false) {
                    var _word = _tagStrings + word;
                    _comicsText.text += _word;
                    _tagStrings = "";
                } else if (_isInTag == true || isCloseTag == true) {
                    _tagStrings += word;
                    Debug.Log("Tag内です。");
                    continue;
                } else {
                    _comicsText.text += word;
                }
                await UniTask.Delay((int)(wordInterval * 1000f));

                if (_isSkipped == true) {
                    _comicsText.text = talk.Talk;
                    break;
                }
            }

            _currentPageCompleted = true;
            _fadeInOutLoopAnimation.AnimationOnOff(true);

            await UniTask.WaitUntil(() => _goToNextPage == true);
            
            i++;
            if (i == talkList.Count - 1) {
                Debug.Log("Yを消し、Rの文章を変える");
                Destroy(_fadeInOutLoopAnimation.gameObject);
                _storiesUIController.ChangeText();
            }
        }
    }
    
    // private async UniTask SetAliceEndingPanel(string comicsPanel) {
    private async void SetAliceEndingPanel(string comicsPanel) {
        _canSkip = false;

        Sprite comicsPanelSprite = _comicsPanelDB.GetComicsPanelSprite(comicsPanel);

        string currentComicsPanel = _comicsPanel.sprite.name;
        if (currentComicsPanel == comicsPanelSprite.name) {
            Debug.Log("同じ背景なので変更をスキップします");
            return;
        }

        // フェードイン
        await _endingFadeImage.DOFade(1.0f, 1.5f)
            .SetLink(_endingFadeImage.gameObject)
            .AsyncWaitForCompletion();
        
        _comicsPanel.gameObject.SetActive(false);
        _comicsPanel.sprite = comicsPanelSprite;

        // フェードアウト
        await _endingFadeImage.DOFade(0, 1.5f)
            .SetLink(_endingFadeImage.gameObject)
            .AsyncWaitForCompletion();
        
        _comicsPanel.gameObject.SetActive(true);

        _canSkip = true;
    }
#endregion

    private async UniTaskVoid GoNextSceneAsync(float duration, string nextSceneName) {
        try {
            await UniTask.Delay((int)(duration * 1000), cancellationToken: this.GetCancellationTokenOnDestroy());
            SceneManager.LoadScene(nextSceneName);
        } catch (System.Exception e) {
            Debug.LogError($"Scene transition failed: {e.Message}");
        }
    }
}
