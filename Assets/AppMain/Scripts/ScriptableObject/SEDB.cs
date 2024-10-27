using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SEDB", menuName = "ScriptableObjects/SE Database")]
public class SEDB : ScriptableObject {
    [SerializeField] private List<AudioClip> _audioClips = null;

    public List<AudioClip> AudioClips { get => _audioClips; set => _audioClips = value; }
}
