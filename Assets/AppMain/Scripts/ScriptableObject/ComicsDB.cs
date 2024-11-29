using UnityEngine;

[CreateAssetMenu(fileName = "ComicsDB", menuName = "ScriptableObjects/Comics Database")]
public class ComicsDB : ScriptableObject {
    [System.Serializable]
    public class CrusherParameter {
        public string Name = "";
        [Multiline(2)]
        public string StoryTitle = "";
        public string CSVName = "";
    }

    [System.Serializable]
    public class BuilderParameter {
        public string Name = "";
        public CrusherParameter[] CrusherParameter = new CrusherParameter[0];
    }

    [SerializeField] private BuilderParameter[] _builderParameters = new BuilderParameter[0];

    private BuilderParameter GetBuilderParameter(int builderIndex) {
        return _builderParameters[builderIndex];
    }

    private CrusherParameter GetCrusherParameter(BuilderParameter parameter, int index) {
        return parameter.CrusherParameter[index];
    }
    
    /// <summary>
    /// ビルダーとクラッシャーのインデックスから, csv名を取得する.
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
