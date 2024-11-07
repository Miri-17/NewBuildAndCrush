using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Febucci.UI;

public class CrossoverController : MonoBehaviour {
    #region Private Fields
    private ComicsPanelDB _comicsPanelDB = null;
    // 次へフラグ.
    private bool _goToNextPage = false;
    // 次へ行けるフラグ.
    private bool _currentPageCompleted = false;
    // 現在のキャラクター情報.
    private string _currentCharacter = "";
    private List<StoryData> _talks = new List<StoryData>();
    private string _csvName = "";
    // シーン遷移関係.
    private AudioSource _audioSourceBGM = null;
    private AudioSource _audioSourceSE = null;
    private bool _isChangingScene = false;
    #endregion

    #region Serialized Fields
    [SerializeField] private TextMeshProUGUI _comicsText = null;
    [SerializeField] private FadeInOutLoopAnimation _fadeInOutLoopAnimation = null;
    [SerializeField] private Image _comicsPanel = null;
    [SerializeField] CSVReader _csvReader = null;
    [SerializeField] ComicsGenerator _comicsGenerator = null;
    [SerializeField] private StoriesUIController _storiesUIController = null;
    // Crossover特有.
    [SerializeField] private TextMeshProUGUI _nameText = null;
    [SerializeField] private ComicsCharacterDB _comicsCharacterDB = null;
    [SerializeField] private Image _characterImage = null;
    [SerializeField, Header("0...Opening, 1...BuilderWin, 2...CrusherWin")] private List<ComicsPanelDB> _comicsPanelDBs = new List<ComicsPanelDB>();
    [SerializeField] private TypewriterByCharacter _typewriter = null;
    #endregion
    
    private void Start() {
        if (GameDirector.Instance.IsOpening) {
            _comicsPanelDB = _comicsPanelDBs[0];
        } else {
            if (GameDirector.Instance.IsBuilderWin)
                _comicsPanelDB = _comicsPanelDBs[1];
            else
                _comicsPanelDB = _comicsPanelDBs[2];
        }
        _csvName = _comicsGenerator.CSVName;

        _audioSourceBGM = BGM.Instance.GetComponent<AudioSource>();
        _audioSourceSE = CrusherSE.Instance.GetComponent<AudioSource>();

        _typewriter.waitForNormalChars = 0.08f;

        StartTalk();
    }

    private void Update() {
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
    }

    private async void StartTalk() {
        Open();
        _talks = _csvReader.GetCSVData(_csvName);

        await TalkStart(_talks);

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

    private async UniTask TalkStart(List<StoryData> talkList) {
        _currentCharacter = "";

        Debug.Log("talkList Count: " + talkList.Count);
        int i = 0;
        foreach (var talk in talkList) {
            if (string.IsNullOrEmpty(talk.ComicPanel) == false) {
                SetComicsPanel(talk.ComicPanel);
            }

            _nameText.text = _comicsCharacterDB.GetCharacterName(int.Parse(talk.Name));
            _comicsText.text = "";
            _goToNextPage = false;
            _currentPageCompleted = false;
            _fadeInOutLoopAnimation.AnimationOnOff(false);
            await SetCharacter(talk);

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

    /// <summary>
    /// ウィンドウを開く
    /// </summary>
    /// <param name="initName"></param>
    /// <param name="initText"></param>
    /// <returns></returns>
    private void Open(string initName = "", string initText = "") {
        SetCharacter(null).Forget();
        _nameText.text = initName;
        _comicsText.text = initText;
        _fadeInOutLoopAnimation.AnimationOnOff(false);
    }

    private async UniTask SetCharacter(StoryData storyData) {
        // nullなら全て消す
        if (storyData == null) {
            _characterImage.enabled = false;
            return;
        }

        var tasks = new List<UniTask>();
        bool hideLeft = false;

        // 値がない場合は非表示にする.
        // 値が前回と違う場合は画像を変更して表示.
        // 値が同じ場合は変化なし.
        Debug.Log("currentCharacter: " + _currentCharacter);
        Debug.Log("storyData.CurrentCharacter: " + storyData.CurrentCharacter);

        // キャラクター設定.
        if (string.IsNullOrEmpty(storyData.CurrentCharacter) == true) {
            Debug.Log("キャラクター画像の非表示");

            hideLeft = true;
        } else if (_currentCharacter != storyData.CurrentCharacter) {
            Debug.Log("キャラクター画像の変更");

            Sprite characterSprite = _comicsCharacterDB.GetCharacterSprite(storyData.CurrentCharacter);
            _characterImage.sprite = characterSprite;
            _characterImage.enabled = true;

            _currentCharacter = storyData.CurrentCharacter;
        } else {
            Debug.Log("キャラクター画像の変化なし");
        }

        await UniTask.WhenAll(tasks);

        if (hideLeft == true) _characterImage.enabled = false;
    }

    private void SetComicsPanel(string comicsPanel) {
        Debug.Log("ComicsPanel: " + comicsPanel);
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

    private async UniTaskVoid GoNextSceneAsync(float duration, string nextSceneName) {
        try {
            await UniTask.Delay((int)(duration * 1000), cancellationToken: this.GetCancellationTokenOnDestroy());
            SceneManager.LoadScene(nextSceneName);
        } catch (System.Exception e) {
            Debug.LogError($"Scene transition failed: {e.Message}");
        }
    }
}
