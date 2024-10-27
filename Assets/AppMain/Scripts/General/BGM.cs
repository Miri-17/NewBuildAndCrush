using UnityEngine;
using UnityEngine.SceneManagement;

public class BGM : MonoBehaviour {
    public static BGM Instance { get; private set; }

    [SerializeField] private BGMDB _bgmDB = null;

    private AudioSource _audioSource = null;

    private void Awake() {
        // すでにロードされていたら、自分自身を破棄して終了する.
        if (Instance != null) {
            Destroy(this.gameObject);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start() {
        _audioSource = this.GetComponent<AudioSource>();

        _audioSource.clip = _bgmDB.AudioClips[0];
        #region // デバッグ用
        switch (SceneManager.GetActiveScene().name) {
            case "PlayerSelection":
                _audioSource.clip = _bgmDB.AudioClips[1];
                break;
            case "SoundTrack":
                break;
            case "Credits":
                _audioSource.clip = _bgmDB.AudioClips[13];
                break;
            default:
                break;
        }
        #endregion
        _audioSource.Play();

        // シーンが切り替わった時に呼ばれるメソッドを登録する.
        SceneManager.activeSceneChanged += ChangedActiveScene;
    }

    private void ChangedActiveScene (Scene thisScene, Scene nextScene) {
        Debug.Log("This Scene is " + thisScene.name);
        Debug.Log("Next Scene is " + nextScene.name);

        switch (nextScene.name) {
            case "ModeSelection":
                if (_audioSource.isPlaying && _audioSource.clip == _bgmDB.AudioClips[0])
                        break;
                _audioSource.Stop();
                _audioSource.volume = 1;
                _audioSource.clip = _bgmDB.AudioClips[0];
                Debug.Log("audioSource.clip: " + _audioSource.clip);
                _audioSource.Play();
                break;
            case "PlayerSelection":
                _audioSource.Stop();
                _audioSource.clip = _bgmDB.AudioClips[1];
                Debug.Log("audioSource.clip: " + _audioSource.clip);
                _audioSource.Play();
                break;
            case "SoundTrack":
                _audioSource.Stop();
                break;
            case "Credits":
                _audioSource.Stop();
                _audioSource.clip = _bgmDB.AudioClips[13];
                Debug.Log("audioSource.clip: " + _audioSource.clip);
                _audioSource.Play();
                break;
            default:
                break;
        }
    }
}
