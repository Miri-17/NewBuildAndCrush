using UnityEngine;
using UnityEngine.Audio;
using DG.Tweening;

public class BattleBGMController : MonoBehaviour {
    #region Private Fields
    private BuilderController _builderController = null;
    private AudioSource _audioSourceNormal = null;
    private AudioSource _audioSourceWagon = null;
    #endregion

    #region Serialized Fields
    [SerializeField] private BattleBGMDB _battleBGMDB = null;
    [SerializeField] private float _fadeDuration = 0.8f;
    [SerializeField] private AudioMixerGroup _audioMixerGroup = null;
    #endregion

    private void Start() {
        _builderController = GameObject.Find("BuilderController").GetComponent<BuilderController>();
        
        _audioSourceNormal = this.gameObject.AddComponent<AudioSource>();
        _audioSourceWagon = this.gameObject.AddComponent<AudioSource>();

        _audioSourceNormal.outputAudioMixerGroup = _audioMixerGroup;
        _audioSourceWagon.outputAudioMixerGroup = _audioMixerGroup;

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
        if (_builderController.WagonControllerRun != null) {
            if (_builderController.WagonControllerRun.CrusherEnterCheck.IsOn) {
                SetWagonBGM();
            }
        }
    }

    // ワゴン乗車時のBGMにクロスフェードする.
    private void SetWagonBGM() {
        _audioSourceNormal.DOFade(0, _fadeDuration)
            .SetEase(Ease.Linear)
            .SetLink(_audioSourceNormal.gameObject);
        
        _audioSourceWagon.DOFade(1, _fadeDuration)
            .SetEase(Ease.Linear)
            .SetLink(_audioSourceWagon.gameObject);
    }
    
    // ワゴン乗車外のBGMにクロスフェードする.
    public void SetNormalBGM() {
        _audioSourceNormal.DOFade(1, _fadeDuration)
            .SetEase(Ease.Linear)
            .SetLink(_audioSourceNormal.gameObject);
        
        _audioSourceWagon.DOFade(0, _fadeDuration)
            .SetEase(Ease.Linear)
            .SetLink(_audioSourceWagon.gameObject);
    }
}
