using UnityEngine;

[CreateAssetMenu(fileName = "BGMDB", menuName = "ScriptableObjects/BGM Database")]
public class BGMDB : ScriptableObject {
    [SerializeField] private AudioClip[] _audioClips = new AudioClip[0];

    public AudioClip[] AudioClips { get => _audioClips; set => _audioClips = value; }
}
