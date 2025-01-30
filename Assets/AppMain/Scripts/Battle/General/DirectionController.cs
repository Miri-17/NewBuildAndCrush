using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;

public class DirectionController : MonoBehaviour {
    private AudioSource _audioSourceBGM = null;
    private AudioSource _audioSourceSE = null;
    private GameObject _makeWagonPanel = null;
    private GameObject _builderWaitingPanel = null;
    private GameObject _crusherWaitingPanel = null;
    private GameObject _battleBGM = null;
    private bool _isBuilderReady = false;
    private bool _isCrusherReady = false;
    private bool _isReadyGoState = false;

    [SerializeField] private Button _readyButton = null;
    [Header("0...Builder, 1...Crusher")]
    [SerializeField] private GameObject[] _startPanels = new GameObject[0];
    [SerializeField] private GameObject _makeWagonPanelPrefab = null;
    [SerializeField] private GameObject[] _waitingPanelPrefabs = new GameObject[0];
    [SerializeField] private GameObject _readyGoPanelPrefab = null;
    [SerializeField] private GameObject _finishPanelPrefab = null;
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
            _isCrusherReady = true;
            _audioSourceSE.PlayOneShot(CrusherSE.Instance.SEDB.AudioClips[0]);
            Destroy(_startPanels[1]);
            if (!_isBuilderReady) {
                _crusherWaitingPanel = Instantiate(_waitingPanelPrefabs[1], GameObject.FindWithTag("CrusherDirection").transform);
                _crusherWaitingPanel.transform.localPosition = Vector3.zero;
            }
        }

        if (!_isReadyGoState && _isBuilderReady && _isCrusherReady) {
            _isReadyGoState = true;
            ReadyGo().Forget();
        }
    }

    private void OnReadyButtonClicked() {
        if (_startPanels[0] == null) return;

        _audioSourceSE.PlayOneShot(CrusherSE.Instance.SEDB.AudioClips[0]);
        Destroy(_startPanels[0]);
        _makeWagonPanel = Instantiate(_makeWagonPanelPrefab, GameObject.FindWithTag("BuilderDirection").transform);
        _makeWagonPanel.transform.localPosition = Vector3.zero;
    }

    private async UniTaskVoid ReadyGo() {
        if (_crusherWaitingPanel != null)
            Destroy(_crusherWaitingPanel);
        if (_builderWaitingPanel != null)
            Destroy(_builderWaitingPanel);
        
        _audioSourceBGM.Stop();
        _audioSourceSE.PlayOneShot(CrusherSE.Instance.SEDB.AudioClips[8]);

        var builderReadyGoPanel = Instantiate(_readyGoPanelPrefab, GameObject.FindWithTag("BuilderDirection").transform);
        builderReadyGoPanel.transform.localPosition = Vector3.zero;
        var crusherReadyGoPanel = Instantiate(_readyGoPanelPrefab, GameObject.FindWithTag("CrusherDirection").transform);
        crusherReadyGoPanel.transform.localPosition = Vector3.zero;

        await UniTask.Delay(TimeSpan.FromSeconds(1.9f), cancellationToken: this.GetCancellationTokenOnDestroy());

        Destroy(builderReadyGoPanel);
        Destroy(crusherReadyGoPanel);
        _battleBGM = Instantiate(_battleBGMPrefab);
        IsDirection = false;
        _battleBuilderUIController.OnGoButtonClicked();
    }

    /// <summary>
    /// ビルダーが最初のワゴンに障害物まで設置し終わった時に呼ぶ.
    /// </summary>
    public void BuilderReady() {
        _isBuilderReady = true;
        Destroy(_makeWagonPanel);
        if (!_isCrusherReady) {
            _builderWaitingPanel = Instantiate(_waitingPanelPrefabs[0], GameObject.FindWithTag("BuilderDirection").transform);
            _builderWaitingPanel.transform.localPosition = Vector3.zero;
        }
    }

    /// <summary>
    /// 戦闘終了時演出を流す.
    /// </summary>
    public void FinishDirection() {
        IsDirection = true;
        Destroy(_battleBGM);
        var builderFinishPanel = Instantiate(_finishPanelPrefab, GameObject.FindWithTag("BuilderDirection").transform);
        builderFinishPanel.transform.localPosition = Vector3.zero;
        var crusherFinishPanel = Instantiate(_finishPanelPrefab, GameObject.FindWithTag("CrusherDirection").transform);
        crusherFinishPanel.transform.localPosition = Vector3.zero;
    }
}
