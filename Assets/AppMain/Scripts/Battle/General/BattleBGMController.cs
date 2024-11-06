using UnityEngine;
using DG.Tweening;

public class BattleBGMController : MonoBehaviour {
    #region Private Fields
    private AudioSource _audioSourceNormal;
    private AudioSource _audioSourceWagon;
    private bool _isInWagon = false;
    #endregion

    #region Serialized Fields
    [SerializeField] private BuilderController _builderController;
    [SerializeField] private BattleBGMDB _battleBGMDB;
    [SerializeField] private float _fadeDuration = 1.0f;
    #endregion

    private void Start() {
        _audioSourceNormal = this.gameObject.AddComponent<AudioSource>();
        _audioSourceWagon = this.gameObject.AddComponent<AudioSource>();

        _audioSourceNormal.clip = _battleBGMDB.GetNormalAudioClip(GameDirector.Instance.BuilderIndex);
        _audioSourceWagon.clip = _battleBGMDB.GetWagonAudioClip(GameDirector.Instance.BuilderIndex);

        _audioSourceNormal.loop = true;
        _audioSourceWagon.loop = true;

        _audioSourceNormal.Play();
        _audioSourceWagon.Play();

        _audioSourceNormal.volume = 1;
        _audioSourceWagon.volume = 0;
    }

    private void Update() {
        if (_builderController.WagonControllerRun == null)
            return;
        
        // ワゴンに乗ったら音楽を変更.
        if (!_isInWagon && _builderController.WagonControllerRun.CrusherEnterCheck.IsOn) {
            _isInWagon = true;
            SetWagonBGM();
        }
        //ワゴンから降りたら音楽を変更.
        if (_isInWagon && _builderController.WagonControllerRun.CrusherExitCheck.IsOn) {
            _isInWagon = false;
            SetNormalBGM();
        }
    }

    private void SetWagonBGM() {
        _audioSourceNormal.DOFade(0, _fadeDuration)
            .SetLink(_audioSourceNormal.gameObject);
        
        _audioSourceWagon.DOFade(1, _fadeDuration)
            .SetLink(_audioSourceWagon.gameObject);
    }
    
    private void SetNormalBGM() {
        _audioSourceNormal.DOFade(1, _fadeDuration)
            .SetLink(_audioSourceNormal.gameObject);
        
        _audioSourceWagon.DOFade(0, _fadeDuration)
            .SetLink(_audioSourceWagon.gameObject);
    }
}
