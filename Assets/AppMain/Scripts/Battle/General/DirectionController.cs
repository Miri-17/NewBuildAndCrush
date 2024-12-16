using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;

public class DirectionController : MonoBehaviour {
    private AudioSource _audioSourceBGM = null;
    private AudioSource _audioSourceSE = null;
    private int _isReady = 0;
    private GameObject _makeWagonPanelPrefab = null;
    private GameObject _waitForBuilderPanelPrefab = null;
    private GameObject _battleBGM = null;
    private bool _isBuilderReady = false;
    private bool _isSetWaitForBuilderPanel = false;

    [SerializeField] private GameObject _wall = null;   // ビルダーのタッチ防ぐ用.
    [SerializeField] private Button _readyButton = null;
    [Header("0...Builder, 1...Crusher")]
    [SerializeField] private GameObject[] _startPanels = new GameObject[0];
    [SerializeField] private GameObject _makeWagonPanel = null;
    [SerializeField] private GameObject _waitForBuilderPanel = null;
    [SerializeField] private GameObject _readyGoPanel = null;
    [SerializeField] private GameObject _finishPanel = null;
    [SerializeField] private GameObject _battleBGMPrefab = null;
    [SerializeField] private BattleBuilderUIController _battleBuilderUIController = null;

    public bool IsDirection { get; private set; } = false;

    private void Start() {
        IsDirection = true;
        _readyButton.onClick.AddListener(() => OnReadyButtonClicked());
        _audioSourceBGM = BGM.Instance.GetComponent<AudioSource>();
        _audioSourceSE = CrusherSE.Instance.GetComponent<AudioSource>();
    }

    private void Update() {
        if (_startPanels[1] != null && Input.GetButtonDown("Select")) {
            _audioSourceSE.PlayOneShot(CrusherSE.Instance.SEDB.AudioClips[0]);
            Destroy(_startPanels[1]);
            if (!_isBuilderReady) {
                _isSetWaitForBuilderPanel = true;
                _waitForBuilderPanelPrefab = Instantiate(_waitForBuilderPanel, GameObject.FindWithTag("CrusherDirection").transform);
                _waitForBuilderPanelPrefab.transform.localPosition = Vector3.zero;
            }
            _isReady++;
        }

        if (_isReady > 1) {
            _isReady = 0;
            ReadyGo().Forget();
        }
    }

    private void OnReadyButtonClicked() {
        if (_startPanels[0] == null) return;

        _audioSourceSE.PlayOneShot(CrusherSE.Instance.SEDB.AudioClips[0]);
        Destroy(_startPanels[0]);
        _makeWagonPanelPrefab = Instantiate(_makeWagonPanel, GameObject.FindWithTag("BuilderDirection").transform);
        _makeWagonPanelPrefab.transform.localPosition = Vector3.zero;
        _wall.SetActive(false);
    }

    public void BuilderReady() {
        _isBuilderReady = true;
        Destroy(_makeWagonPanelPrefab);
        if (_isSetWaitForBuilderPanel && _waitForBuilderPanelPrefab != null)
            Destroy(_waitForBuilderPanelPrefab);
        _isReady++;
        _wall.SetActive(true);
    }

    private async UniTaskVoid ReadyGo() {
        _audioSourceBGM.Stop();
        _audioSourceSE.PlayOneShot(CrusherSE.Instance.SEDB.AudioClips[8]);

        var builderReadyGoPanelPrefab = Instantiate(_readyGoPanel, GameObject.FindWithTag("BuilderDirection").transform);
        builderReadyGoPanelPrefab.transform.localPosition = Vector3.zero;
        var crusherReadyGoPanelPrefab = Instantiate(_readyGoPanel, GameObject.FindWithTag("CrusherDirection").transform);
        crusherReadyGoPanelPrefab.transform.localPosition = Vector3.zero;

        await UniTask.Delay(TimeSpan.FromSeconds(1.9f), cancellationToken: this.GetCancellationTokenOnDestroy());

        Destroy(builderReadyGoPanelPrefab);
        Destroy(crusherReadyGoPanelPrefab);
        _wall.SetActive(false);
        _battleBGM = Instantiate(_battleBGMPrefab);
        IsDirection = false;
        _battleBuilderUIController.OnGoButtonClicked();
    }

    public void FinishDirection() {
        IsDirection = true;
        Destroy(_battleBGM);
        var builderFinishPanelPrefab = Instantiate(_finishPanel, GameObject.FindWithTag("BuilderDirection").transform);
        builderFinishPanelPrefab.transform.localPosition = Vector3.zero;
        var crusherFinishPanelPrefab = Instantiate(_finishPanel, GameObject.FindWithTag("CrusherDirection").transform);
        crusherFinishPanelPrefab.transform.localPosition = Vector3.zero;
    }
}
