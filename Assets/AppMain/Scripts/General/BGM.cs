using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGM : MonoBehaviour {
    public static BGM Instance { get; private set; }

    private AudioSource _audioSource = null;

    [SerializeField] private BGMDB _bgmDB = null;

    public BGMDB BGMDB { get => _bgmDB; set => _bgmDB = value; }

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
        _audioSource.Play();
        #region // デバッグ用
        switch (SceneManager.GetActiveScene().name) {
            case "PlayerSelection":
                _audioSource.clip = _bgmDB.AudioClips[1];
                _audioSource.Play();
                break;
            case "SoundList":
                _audioSource.Stop();
                break;
            case "Credits":
                _audioSource.clip = _bgmDB.AudioClips[13];
                _audioSource.Play();
                break;
            case "Story":
                if (GameDirector.Instance.IsOpening) {
                    switch (GameDirector.Instance.BuilderIndex) {
                        case 0:
                            _audioSource.clip = _bgmDB.AudioClips[2];
                            break;
                        case 1:
                            _audioSource.clip = _bgmDB.AudioClips[5];
                            break;
                        case 2:
                            _audioSource.clip = _bgmDB.AudioClips[7];
                            break;
                        default:
                            _audioSource.clip = _bgmDB.AudioClips[9];
                            break;
                    }
                } else {
                    switch (GameDirector.Instance.BuilderIndex) {
                        case 0:
                        case 3:
                            _audioSource.clip = _bgmDB.AudioClips[12];
                            break;
                        default:
                            _audioSource.clip = _bgmDB.AudioClips[13];
                            break;
                    }
                }
                _audioSource.Play();
                break;
            case "Crossover":
                if (GameDirector.Instance.IsOpening)
                    _audioSource.clip = _bgmDB.AudioClips[2];
                else
                    _audioSource.clip = _bgmDB.AudioClips[12];
                _audioSource.Play();
                break;
            case "Battle":
                _audioSource.clip = _bgmDB.AudioClips[14];
                _audioSource.Play();
                break;
            case "Result":
                _audioSource.loop = false;
                _audioSource.clip = _bgmDB.AudioClips[11];
                _audioSource.Play();
                break;
            default:
                break;
        }
        #endregion
        // _audioSource.Play(); // TODO デバッグを終えたらコメントアウト外す！！

        // シーンが切り替わった時に呼ばれるメソッドを登録する.
        SceneManager.activeSceneChanged += ChangedActiveScene;
    }

    private void ChangedActiveScene (Scene thisScene, Scene nextScene) {
        Debug.Log("This Scene is " + thisScene.name);
        Debug.Log("Next Scene is " + nextScene.name);

        switch (nextScene.name) {
            case "ModeSelection":
                // if (_audioSource.isPlaying && _audioSource.clip == _bgmDB.AudioClips[0])
                if (GameDirector.Instance.PreviousSceneName == "Title" && _audioSource.clip == _bgmDB.AudioClips[0])
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
            case "SoundList":
                _audioSource.Stop();
                break;
            case "Credits":
                _audioSource.Stop();
                _audioSource.clip = _bgmDB.AudioClips[13];
                Debug.Log("audioSource.clip: " + _audioSource.clip);
                _audioSource.Play();
                break;
            case "Story":
                _audioSource.Stop();
                if (GameDirector.Instance.IsOpening) {
                    switch (GameDirector.Instance.BuilderIndex) {
                        case 0:
                            _audioSource.clip = _bgmDB.AudioClips[2];
                            break;
                        case 1:
                            _audioSource.clip = _bgmDB.AudioClips[5];
                            break;
                        case 2:
                            _audioSource.clip = _bgmDB.AudioClips[7];
                            break;
                        default:
                            _audioSource.clip = _bgmDB.AudioClips[9];
                            break;
                    }
                } else {
                    switch (GameDirector.Instance.BuilderIndex) {
                        case 0:
                        case 3:
                            _audioSource.clip = _bgmDB.AudioClips[12];
                            break;
                        default:
                            _audioSource.clip = _bgmDB.AudioClips[13];
                            break;
                    }
                }
                _audioSource.Play();
                break;
            case "Crossover":
                _audioSource.Stop();
                if (GameDirector.Instance.IsOpening)
                    _audioSource.clip = _bgmDB.AudioClips[2];
                else
                    _audioSource.clip = _bgmDB.AudioClips[12];
                Debug.Log("audioSource.clip: " + _audioSource.clip);
                _audioSource.Play();
                break;
            case "Battle":
                _audioSource.Stop();
                _audioSource.clip = _bgmDB.AudioClips[14];
                Debug.Log("audioSource.clip: " + _audioSource.clip);
                _audioSource.Play();
                break;
            case "Result":
                _audioSource.Stop();
                _audioSource.loop = false;
                _audioSource.clip = _bgmDB.AudioClips[11];
                Debug.Log("audioSource.clip: " + _audioSource.clip);
                _audioSource.Play();
                break;
            default:
                break;
        }
    }
}
