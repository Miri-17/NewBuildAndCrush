using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;

public class DirectionController : MonoBehaviour {
    private AudioSource _audioSourceBGM = null;
    private AudioSource _audioSourceSE = null;
    private int _isReady = 0;
    private GameObject _battleBGM = null;

    [SerializeField] private GameObject _wall = null;   // ビルダーのタッチ防ぐ用.
    [SerializeField] private Button _readyButton = null;
    [Header("0...Builder, 1...Crusher")]
    [SerializeField] private GameObject[] _startPanels = null;
    [SerializeField] private GameObject _readyGoPanel = null;
    [SerializeField] private GameObject _finishPanel = null;
    [SerializeField] private GameObject _battleBGMPrefab = null;

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
            _isReady++;
        }

        if (_isReady > 1) {
            Debug.Log("Ready Go");
            _isReady = 0;
            ReadyGo().Forget();
        }
    }

    private void OnReadyButtonClicked() {
        if (_startPanels[0] == null) return;

        _audioSourceSE.PlayOneShot(CrusherSE.Instance.SEDB.AudioClips[0]);
        Destroy(_startPanels[0]);
        _isReady++;
    }

    private async UniTaskVoid ReadyGo() {
        _audioSourceBGM.Stop();
        _audioSourceSE.PlayOneShot(CrusherSE.Instance.SEDB.AudioClips[8]);

        var prefab0 = Instantiate(_readyGoPanel, GameObject.FindWithTag("BuilderDirection").transform);
        prefab0.transform.localPosition = Vector3.zero;
        var prefab1 = Instantiate(_readyGoPanel, GameObject.FindWithTag("CrusherDirection").transform);
        prefab1.transform.localPosition = Vector3.zero;

        await UniTask.Delay(TimeSpan.FromSeconds(1.9f), cancellationToken: this.GetCancellationTokenOnDestroy());

        Destroy(prefab0);
        Destroy(prefab1);
        _wall.SetActive(false);
        _battleBGM = Instantiate(_battleBGMPrefab);
        IsDirection = false;
    }

    public void FinishDirection() {
        IsDirection = true;
        Destroy(_battleBGM);
        var prefab0 = Instantiate(_finishPanel, GameObject.FindWithTag("BuilderDirection").transform);
        prefab0.transform.localPosition = Vector3.zero;
        var prefab1 = Instantiate(_finishPanel, GameObject.FindWithTag("CrusherDirection").transform);
        prefab1.transform.localPosition = Vector3.zero;
    }
}
