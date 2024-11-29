using UnityEngine;

[CreateAssetMenu(fileName = "ComicsPanelDB", menuName = "ScriptableObjects/Comics Panel Database")]
public class ComicsPanelDB : ScriptableObject {
    /// <summary>
    /// コマ絵のパラメータ.
    /// </summary>
    [System.Serializable]
    public class Parameter {
        public string Name = "";
        public Sprite Sprite = null;
    }

    [SerializeField] private Parameter[] Parameters = new Parameter[0];

    /// <summary>
    /// 画像名から画像を取得する.
    /// </summary>
    /// <param name="imageName"></param>
    /// <returns></returns>
    public Sprite GetComicsPanelSprite(string imageName) {
        foreach (var param in Parameters) {
            if (param.Name == imageName) return param.Sprite;
        }
        return null;
    }
}
