using UnityEngine;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;

public class DestroyableBuilder : MonoBehaviour {
    #region Private Fields
    private AudioSource _audioSource = null;
    private Animator _animator = null;
    private int _defense = 5;   // どのビルダーも必ず5回の攻撃で倒せる.
    #endregion

    #region Serialized Fields
    [SerializeField] private AudioClip[] _audioClips = new AudioClip[4];
    [SerializeField] private GameObject _defeatFilterPrefab = null;
    #endregion

    public bool IsCrushed { get; private set; } = false;

    private void Start() {
        _audioSource = this.GetComponent<AudioSource>();
        _animator = this.GetComponent<Animator>();
    }

    public void TakeDamage(int damage) {
        _defense -= damage;
        switch (_defense) {
            case 0:
                IsCrushed = true;
                Crush().Forget();
                break;
            case 1:
                _audioSource.PlayOneShot(_audioClips[3]);
                break;
            case 2:
                _audioSource.PlayOneShot(_audioClips[2]);
                break;
            case 3:
                _audioSource.PlayOneShot(_audioClips[1]);
                break;
            case 4:
                _audioSource.PlayOneShot(_audioClips[0]);
                _animator.SetBool("Damage", true);
                break;
            default:
                break;
        }
    }

    private async UniTaskVoid Crush() {
        _animator.SetTrigger("Defeat");

        var prefab0 = Instantiate(_defeatFilterPrefab, GameObject.Find("CrusherDirectionPanel").transform);
        prefab0.transform.localPosition = Vector3.zero;
        var prefab1 = Instantiate(_defeatFilterPrefab, GameObject.Find("BuilderDirectionPanel").transform);
        prefab1.transform.localPosition = Vector3.zero;

        this.transform.DORotate(new Vector3(0, 0, -75), 2);
        this.transform.DOLocalPath(
            new[] {
                new Vector3(100f, 30f, 0),
                new Vector3(200f, -10f, 0),
            },
            2f, PathType.CatmullRom)
            .SetEase(Ease.Linear)
            .SetRelative();
        
        await UniTask.Delay(TimeSpan.FromSeconds(1.9f), cancellationToken: this.GetCancellationTokenOnDestroy());

        this.transform.DOLocalPath(
            new[] {
                new Vector3(30f, 10f, 0),
                new Vector3(60f, -10f, 0),
            },
            0.8f, PathType.CatmullRom)
            .SetEase(Ease.Linear)
            .SetRelative();

        await UniTask.Delay(TimeSpan.FromSeconds(0.8f), cancellationToken: this.GetCancellationTokenOnDestroy());
    }
}
