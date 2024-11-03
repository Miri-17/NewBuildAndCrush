using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// キャラクター名、画像関連付データ.
/// </summary>
[CreateAssetMenu(fileName = "ComicsCharacterDB", menuName = "ScriptableObjects/Comics Character Database")]
public class ComicsCharacterDB : ScriptableObject {
    /// <summary>
    /// キャラクター定義.
    /// </summary>
    public enum Type {
        None = 0,
        Wolf = 1,
        QueenAlice = 2,
        Mikado = 3,
        HanzelGretel = 4,
        Girl = 5,
        QueenOfHearts = 6,
        Tenjin = 7,
        Witch = 8,
        Hanzel = 9,
        Gretel = 10,
        Kaguya = 11,
        Jumin = 12,
    }

    /// <summary>
    /// 表情の定義.
    /// </summary>
    public enum EmotionType {
        None,
        Normal,
        Angry,
        Happy,
        Panic,
        Sad,
        Smile,
        Spoony,
        Surprise,
    }

    /// <summary>
    /// 画像と表情の関連付けパラメータ.
    /// </summary>
    [System.Serializable]
    public class ImageParam {
        // 表情タイプ.
        public EmotionType Emotion = EmotionType.None;
        // 画像.
        public Sprite Sprite = null;
    }

    /// <summary>
    /// キャラクターのパラメータ.
    /// </summary>
    [System.Serializable]
    public class Parameter {
        public string DisplayName = "";
        public Type Character = Type.None;
        public List<ImageParam> ImageParams = new List<ImageParam>();

        public Sprite GetEmotionSprite(EmotionType emotion) {
            foreach (var imageParam in ImageParams) {
                if (imageParam.Emotion == emotion) return imageParam.Sprite;
            }
            return null;
        }
    }

    [SerializeField] private List<Parameter> Parameters = new List<Parameter>();

    /// <summary>
    /// キャラクター番号からキャラクター表示名を取得する.
    /// </summary>
    /// <param name="characterNumber"></param>
    /// <returns>キャラクター表示名</returns>
    public string GetCharacterName(int characterNumber) {
        // キャラクター番号が0やif文指定範囲以外の時は何もなしで返す.
        if (characterNumber < 0 && characterNumber > 12) return "";
        
        Parameter param = GetParameterFromNumber(characterNumber);
        return param.DisplayName;
    }

    // キャラクター番号からパラメータを取得する.
    private Parameter GetParameterFromNumber(int characterNumber) {
        foreach (Parameter param in Parameters) {
            int typeInt = (int)param.Character;
            if (typeInt == characterNumber) return param;
        }
        return null;
    }

    /// <summary>
    /// 文字列のデータからキャラクター画像を取得する.
    /// </summary>
    /// <param name="dataString"></param>
    /// <returns>キャラクター画像</returns>
    public Sprite GetCharacterSprite(string dataString) {
        // 先頭の2文字はキャラクター定義.
        string characterNumberString = dataString.Substring(0, 2);
        // 3文字目から先は表情定義.
        string emotionString = dataString.Substring(2);

        int characterNumber = int.Parse(characterNumberString);
        if (characterNumber < 0 && characterNumber > 11) {
            Debug.Log("入力データが正しくありません: " + dataString);
            return null;
        }
        
        Parameter param = GetParameterFromNumber(characterNumber);

        EmotionType emotionType = GetEmotionType(emotionString);
        Sprite emotionSprite = param.GetEmotionSprite(emotionType);

        return emotionSprite;
    }

    // 表情部分の文字列からEmotionTypeを取得する.
    private EmotionType GetEmotionType(string emotionString) {
        switch(emotionString) {
            case "Normal": return EmotionType.Normal;
            case "Angry": return EmotionType.Angry;
            case "Happy": return EmotionType.Happy;
            case "Panic": return EmotionType.Panic;
            case "Sad": return EmotionType.Sad;
            case "Smile": return EmotionType.Smile;
            case "Spoony": return EmotionType.Spoony;
            case "Surprise": return EmotionType.Surprise;
            default: return EmotionType.None;
        }
    }
}
