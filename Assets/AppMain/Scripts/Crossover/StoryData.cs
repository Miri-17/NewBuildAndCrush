using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StoryData {
    // 会話キャラ名.
    public string Name = "";
    // 会話内容.
    [Multiline(3)] public string Talk = "";
    // 場所、背景.
    public string ComicPanel = "";
    // 左キャラ.
    // public string Left = "";
    public string CurrentCharacter = "";
    // 真ん中キャラ.
    // public string Center = "";
    // public string CenterCharacter = "";
    // 右キャラ.
    // public string Right = "";
    // public string RightCharacter = "";
}
