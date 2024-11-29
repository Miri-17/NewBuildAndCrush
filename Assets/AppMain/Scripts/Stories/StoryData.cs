using UnityEngine;

[System.Serializable]
public class StoryData {
    // 会話キャラクター名.
    public string Name = "";
    // 会話内容.
    [Multiline(3)] public string Talk = "";
    // 場所、背景.
    public string ComicPanel = "";
    // 現在のキャラクター.
    public string CurrentCharacter = "";
}
