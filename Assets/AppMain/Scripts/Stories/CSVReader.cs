using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class CSVReader : MonoBehaviour {
    // CSVファイル.
    private TextAsset csvFile = null;

    public List<StoryData> GetCSVData(string name) {
        // CSVファイルの中身を入れるリスト.
        List<string[]> cells = new List<string[]>();
        // ResourcesにあるCSVファイルを格納する.
        csvFile = Resources.Load("CSV/" + name) as TextAsset;
        // TextAssetをStringReaderに変換.
        StringReader reader = new StringReader(csvFile.text);
        // 1行目はラベルなので外す.
        reader.ReadLine();

        while (reader.Peek() != -1) {
            // 1行ずつ読み込む.
            string line = reader.ReadLine();
            string[] elements = line.Split(',');
            cells.Add(elements);
        }

        #region デバッグ用.
        string log = "";
        List<StoryData> stories = new List<StoryData>();
        foreach (var line in cells) {
            var data = new StoryData();

            var replace2 = line[2].Replace("<br>", "\n");

            // データの値を入れる.
            data.ComicPanel = line[0];
            data.Name = line[1];
            data.Talk = replace2;
            data.CurrentCharacter = line[3];

            log += $"<コマ絵> {data.ComicPanel}, <キャラNo> {data.Name}, <内容> {data.Talk}, "
                + $"<キャラ> {data.CurrentCharacter}\n";
            
            stories.Add(data);
        }
        Debug.Log(log);
        #endregion

        return stories;
    }
}
