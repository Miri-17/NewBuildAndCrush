using UnityEngine;

[CreateAssetMenu(fileName = "ComicsDB", menuName = "ScriptableObjects/Comics Database")]
public class ComicsDB : ScriptableObject {
    /// <summary>
    /// ビルダーとクラッシャーの組み合わせによって発生するストーリーのCSVデータを格納する.
    /// </summary>
    [System.Serializable]
    public class CrusherParameter {
        public string Name = "";
        [Multiline(2)]
        public string StoryTitle = "";
        public string CSVName = "";
    }

    /// <summary>
    /// あるビルダーとのクラッシャーの組み合わせをデータ化する.
    /// </summary>
    [System.Serializable]
    public class BuilderParameter {
        public string Name = "";
        public CrusherParameter[] CrusherParameter = new CrusherParameter[0];
    }

    [SerializeField] private BuilderParameter[] _builderParameters = new BuilderParameter[0];

    // インデックスからビルダーと, そのビルダーに対するクラッシャーの組み合わせを取得する.
    private BuilderParameter GetBuilderParameter(int builderIndex) {
        return _builderParameters[builderIndex];
    }

    // ビルダーのパラメータとインデックスから, ストーリーのCSVデータを取得する.
    private CrusherParameter GetCrusherParameter(BuilderParameter parameter, int index) {
        return parameter.CrusherParameter[index];
    }
    
    /// <summary>
    /// ビルダーとクラッシャーのインデックスから, CSV名を取得する.
    /// </summary>
    /// <param name="builderIndex"></param>
    /// <param name="crusherIndex"></param>
    /// <returns></returns>
    public string GetCSVName(int builderIndex, int crusherIndex) {
        if (builderIndex < 0 && builderIndex > 4
            || crusherIndex < 0 && crusherIndex > 4) return "";
        
        BuilderParameter builderParameter = GetBuilderParameter(builderIndex);
        CrusherParameter crusherParameter = GetCrusherParameter(builderParameter, crusherIndex);
        return crusherParameter.CSVName;
    }
    
    /// <summary>
    /// ビルダーとクラッシャーのインデックスから, ストーリータイトルを取得する.
    /// </summary>
    /// <param name="builderIndex"></param>
    /// <param name="crusherIndex"></param>
    /// <returns></returns>
    public string GetStoryTitle(int builderIndex, int crusherIndex) {
        if (builderIndex < 0 && builderIndex > 4
            || crusherIndex < 0 && crusherIndex > 4) return "";
        
        BuilderParameter builderParameter = GetBuilderParameter(builderIndex);
        CrusherParameter crusherParameter = GetCrusherParameter(builderParameter, crusherIndex);
        return crusherParameter.StoryTitle;
    }
}
