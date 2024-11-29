using UnityEngine;

[CreateAssetMenu(fileName = "BattleBGMDB", menuName = "ScriptableObjects/Battle BGM Database")]
public class BattleBGMDB : ScriptableObject {
    [SerializeField] private AudioClip[] _normalAudioClips = new AudioClip[0];
    [SerializeField] private AudioClip[] _wagonAudioClips = new AudioClip[0];

    /// <summary>
    /// 通常のバトル音楽を返す.
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public AudioClip GetNormalAudioClip(int index) {
        return _normalAudioClips[index];
    }

    /// <summary>
    /// ワゴン乗車時のバトル音楽を返す.
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public AudioClip GetWagonAudioClip(int index) {
        return _wagonAudioClips[index];
    }
}
