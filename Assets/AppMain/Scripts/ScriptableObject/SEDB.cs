using UnityEngine;

[CreateAssetMenu(fileName = "SEDB", menuName = "ScriptableObjects/SE Database")]
public class SEDB : ScriptableObject {
    [SerializeField] private AudioClip[] _audioClips = new AudioClip[0];

    public AudioClip[] AudioClips { get => _audioClips; set => _audioClips = value; }
}
