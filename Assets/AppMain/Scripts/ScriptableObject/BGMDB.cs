using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BGMDB", menuName = "ScriptableObjects/BGM Database")]
public class BGMDB : ScriptableObject {
    [SerializeField] private List<AudioClip> _audioClips = null;

    public List<AudioClip> AudioClips { get => _audioClips; set => _audioClips = value; }

    /// <summary>
    /// 何曲あるかを返す.
    /// </summary>
    /// <returns></returns>
    public int GetAudioClipCount() {
        return _audioClips.Count;
    }
}
