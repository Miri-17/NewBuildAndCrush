using UnityEngine;
using DG.Tweening;

public class BattleBGMController : MonoBehaviour {
    private AudioSource _audioSource_Normal;
    private AudioSource _audioSource_Wagon;
    private bool _isInWagon = false;
    private bool _isNormalMusic = true;

    #region Serialized Fields
    [SerializeField] private BuilderController _builderController;
    [SerializeField] private BattleBGMDB _battleBGMDB;
    [SerializeField] private float _fadeDuration = 1.0f;
    #endregion

    private void Start() {
        _audioSource_Normal = this.gameObject.AddComponent<AudioSource>();
        _audioSource_Wagon = this.gameObject.AddComponent<AudioSource>();

        _audioSource_Normal.clip = _battleBGMDB.GetNormalAudioClip(GameDirector.Instance.BuilderIndex);
        _audioSource_Wagon.clip = _battleBGMDB.GetWagonAudioClip(GameDirector.Instance.BuilderIndex);

        _audioSource_Normal.loop = true;
        _audioSource_Wagon.loop = true;

        _audioSource_Normal.Play();
        _audioSource_Wagon.Play();

        _audioSource_Normal.volume = 1;
        _audioSource_Wagon.volume = 0;
    }

    private void Update() {
        if (_builderController.wagonControllerRun == null)
            return;

        // ワゴンに乗ったら音楽を変更.
        if (!_isInWagon && _builderController.wagonControllerRun.CrusherEnterCheck.IsOn) {
            _isInWagon = true;
            SetWagonBGM();
            Debug.Log("曲をワゴン用に変更");
        }
        //ワゴンから降りたら音楽を変更.
        if (_isInWagon && _builderController.wagonControllerRun.CrusherExitCheck.IsOn)
            _isInWagon = false;
            SetNormalBGM();
            Debug.Log("曲を普通用に変更");
    }

    private void SetWagonBGM() {
        _audioSource_Normal.DOFade(0, _fadeDuration)
            .SetLink(_audioSource_Normal.gameObject);
        
        _audioSource_Wagon.DOFade(1, _fadeDuration)
            .SetLink(_audioSource_Normal.gameObject);
            // .OnComplete(() => _isNormalMusic = false);
    }
    
    private void SetNormalBGM() {
        _audioSource_Normal.DOFade(0, _fadeDuration)
            .SetLink(_audioSource_Normal.gameObject);
        
        _audioSource_Wagon.DOFade(1, _fadeDuration)
            .SetLink(_audioSource_Normal.gameObject);
            // .OnComplete(() => _isNormalMusic = true);
    }
}
