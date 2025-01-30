using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Febucci.UI;
using TMPro;

// TODO CrossoverControllerと統合したい.
public class StoryController : MonoBehaviour {
    #region Private Fields
    private ComicsPanelDB _comicsPanelDB = null;
    // 次へフラグ.
    private bool _goToNextPage = false;
    // 次へ行けるフラグ.
    private bool _currentPageCompleted = false;
    // 現在のキャラクター情報.
    private List<StoryData> _talks = new List<StoryData>();
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
    [SerializeField] private ComicsPanelDB[] _openingsPanelDBs = new ComicsPanelDB[0];
    [SerializeField] private ComicsPanelDB[] _builderWinPanelDBs = new ComicsPanelDB[0];
    [SerializeField] private ComicsPanelDB[] _crusherWinPanelDBs = new ComicsPanelDB[0];
    [SerializeField] private Image _endingFadeImage = null;
    [SerializeField] private TypewriterByCharacter _typewriter = null;
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
        
        _typewriter.waitForNormalChars = 0.08f;

        StartTalk();
    }

    private void Update() {
        if (!_canSkip) return;

        // Yボタンが押されたら台詞の表示中なら表示を即完了し, 表示が完了していたら次の台詞にする.
        if (_typewriter.isShowingText) {
            if (Input.GetButtonDown("Select"))
                _typewriter.SkipTypewriter();
        } else {
            if (!_currentPageCompleted) {
                _currentPageCompleted = true;
                _fadeInOutLoopAnimation.AnimationOnOff(true);
            }

            if (Input.GetButtonDown("Select"))
                _goToNextPage = true;
        }

        // Rボタンが押されたら, ストーリーのスキップを行う.
        if (!_isChangingScene && Input.GetButtonDown("Fire1")) {
            _isChangingScene = true;
            if (GameDirector.Instance.IsOpening) {
                _audioSourceSE.PlayOneShot(CrusherSE.Instance.SEDB.AudioClips[0]);
                _storiesUIController.TransitionUI(true, 0.5f);
                GoNextSceneAsync(0.5f, "Battle").Forget();
            } else {
                _audioSourceSE.PlayOneShot(CrusherSE.Instance.SEDB.AudioClips[2]);
                _audioSourceBGM.DOFade(0, 1.0f)
                    .SetEase(Ease.Linear)
                    .SetLink(_audioSourceBGM.gameObject);
                _storiesUIController.TransitionUI(true, 1.0f);
                GoNextSceneAsync(1.0f, "ModeSelection").Forget();
            }
        }
    }

    // ストーリーを始め, 終わったら次のシーンに遷移させる.
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
            _storiesUIController.TransitionUI(true, 0.5f);
            GoNextSceneAsync(0.5f, "Battle").Forget();
        } else {
            _audioSourceBGM.DOFade(0, 1.0f)
                .SetEase(Ease.Linear)
                .SetLink(_audioSourceBGM.gameObject);
            _storiesUIController.TransitionUI(true, 1.0f);
            GoNextSceneAsync(1.0f, "ModeSelection").Forget();
        }
    }

    // ストーリーを進行させる.
    private async UniTask TalkStart(List<StoryData> talkList) {
        Debug.Log("talkList Count: " + talkList.Count);
        int i = 0;
        foreach (var talk in talkList) {
            if (string.IsNullOrEmpty(talk.ComicPanel) == false) {
                SetComicsPanel(talk.ComicPanel);
            }

            _comicsText.text = "";
            _goToNextPage = false;
            _currentPageCompleted = false;
            _fadeInOutLoopAnimation.AnimationOnOff(false);

            _typewriter.ShowText(talk.Talk);

            await UniTask.WaitUntil(() => _goToNextPage == true);
            
            i++;
            if (i == talkList.Count - 1) {
                Debug.Log("Yを消し、Rの文章を変える");
                Destroy(_fadeInOutLoopAnimation.gameObject);
                _storiesUIController.ChangeText();
            }
        }
    }

    // ストーリーの初期化.
    private void Open(string initName = "", string initText = "") {
        _comicsText.text = initText;
        _fadeInOutLoopAnimation.AnimationOnOff(false);
    }

    // コミックパネルの変更を行う.
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
    
    // TODO あまり良くないコードなので直したい.
    #region Add for Alice Ending
    // AliceのエンディングのTalkStartに代わるメソッド.
    private async UniTask AliceEndingStart(List<StoryData> talkList) {
        Debug.Log("アリスのエンディングです");
        Debug.Log("talkList Count: " + talkList.Count);
        int i = 0;
        foreach (var talk in talkList) {
            if (string.IsNullOrEmpty(talk.ComicPanel) == false) {
                Debug.Log("talkCount: " + i);
                if (i == 16 && GameDirector.Instance.IsBuilderWin)
                    SetAliceEndingPanel(talk.ComicPanel);
                else if (i == 26 && !GameDirector.Instance.IsBuilderWin)
                    SetAliceEndingPanel(talk.ComicPanel);
                else
                    SetComicsPanel(talk.ComicPanel);
            }

            _goToNextPage = false;
            _currentPageCompleted = false;
            _fadeInOutLoopAnimation.AnimationOnOff(false);

            _typewriter.ShowText(talk.Talk);

            await UniTask.WaitUntil(() => _goToNextPage == true);
            
            i++;
            if (i == talkList.Count - 1) {
                Debug.Log("Yを消し、Rの文章を変える");
                Destroy(_fadeInOutLoopAnimation.gameObject);
                _storiesUIController.ChangeText();
            }
        }
    }
    
    // AliceのエンディングのSetComicsPanelに代わるメソッド.
    private async void SetAliceEndingPanel(string comicsPanel) {
        _canSkip = false;

        Sprite comicsPanelSprite = _comicsPanelDB.GetComicsPanelSprite(comicsPanel);

        string currentComicsPanel = _comicsPanel.sprite.name;
        if (currentComicsPanel == comicsPanelSprite.name) {
            Debug.Log("同じ背景なので変更をスキップします");
            return;
        }

        // フェードイン.
        await _endingFadeImage.DOFade(1.0f, 1.5f)
            .SetLink(_endingFadeImage.gameObject)
            .AsyncWaitForCompletion();
        
        _comicsPanel.gameObject.SetActive(false);
        _comicsPanel.sprite = comicsPanelSprite;

        // フェードアウト.
        await _endingFadeImage.DOFade(0, 1.5f)
            .SetLink(_endingFadeImage.gameObject)
            .AsyncWaitForCompletion();
        
        _comicsPanel.gameObject.SetActive(true);

        _canSkip = true;
    }
    #endregion

    // 次のシーンに遷移する.
    private async UniTaskVoid GoNextSceneAsync(float duration, string nextSceneName) {
        try {
            await UniTask.Delay((int)(duration * 1000), cancellationToken: this.GetCancellationTokenOnDestroy());
            SceneManager.LoadScene(nextSceneName);
        } catch (System.Exception e) {
            Debug.LogError($"Scene transition failed: {e.Message}");
        }
    }
}
