using UnityEngine;

public class ResultSaver : MonoBehaviour {
    private bool isChangedCrusherAchievement = false;
    private bool isChangedBuilderAchievement = false;

    private void Start() {
        UpdateAchievement();
    }

    // Achievement情報をセーブするメソッド
    private void UpdateAchievement() {
        isChangedCrusherAchievement = false;
        isChangedBuilderAchievement = false;

        // もしビルダーが勝っていたら
        if (GameDirector.Instance.IsBuilderWin) {
            switch (GameDirector.Instance.BuilderIndex) {
                case 0:
                    // そのビルダーのachievements配列において、クラッシャーに対するindexの要素が0だったら
                    if (GameDirector.Instance.WolfAchievements[GameDirector.Instance.CrusherIndex] == 0) {
                        isChangedBuilderAchievement = true;
                        // 1にする
                        GameDirector.Instance.WolfAchievements[GameDirector.Instance.CrusherIndex] = 1;
                    }
                    break;
                case 1:
                    if (GameDirector.Instance.QueenAliceAchievements[GameDirector.Instance.CrusherIndex] == 0) {
                        isChangedBuilderAchievement = true;
                        GameDirector.Instance.QueenAliceAchievements[GameDirector.Instance.CrusherIndex] = 1;
                    }
                    break;
                case 2:
                    if (GameDirector.Instance.MikadoAchievements[GameDirector.Instance.CrusherIndex] == 0) {
                        isChangedBuilderAchievement = true;
                        GameDirector.Instance.MikadoAchievements[GameDirector.Instance.CrusherIndex] = 1;
                    }
                    break;
                case 3:
                    if (GameDirector.Instance.HanzelGretelAchievements[GameDirector.Instance.CrusherIndex] == 0) {
                        isChangedBuilderAchievement = true;
                        GameDirector.Instance.HanzelGretelAchievements[GameDirector.Instance.CrusherIndex] = 1;
                    }
                    break;
                default:
                    break;
            }
        // もしクラッシャーが勝っていたら
        } else {
            switch (GameDirector.Instance.CrusherIndex) {
                case 0:
                    // そのクラッシャーのachievements配列において、ビルダーに対するindexの要素が0だったら
                    if (GameDirector.Instance.GirlAchievements[GameDirector.Instance.BuilderIndex] == 0) {
                        isChangedCrusherAchievement = true;
                        // 1にする
                        GameDirector.Instance.GirlAchievements[GameDirector.Instance.BuilderIndex] = 1;
                    }
                    break;
                case 1:
                    if (GameDirector.Instance.QueenOfHeartsAchievements[GameDirector.Instance.BuilderIndex] == 0) {
                        isChangedCrusherAchievement = true;
                        GameDirector.Instance.QueenOfHeartsAchievements[GameDirector.Instance.BuilderIndex] = 1;
                    }
                    break;
                case 2:
                    if (GameDirector.Instance.TenjinAchievements[GameDirector.Instance.BuilderIndex] == 0) {
                        isChangedCrusherAchievement = true;
                        GameDirector.Instance.TenjinAchievements[GameDirector.Instance.BuilderIndex] = 1;
                    }
                    break;
                case 3:
                    if (GameDirector.Instance.WitchAchievements[GameDirector.Instance.BuilderIndex] == 0) {
                        isChangedCrusherAchievement = true;
                        GameDirector.Instance.WitchAchievements[GameDirector.Instance.BuilderIndex] = 1;
                    }
                    break;
                default:
                    break;
            }
        }

        // クラッシャー情報の変更があったら
        if (isChangedCrusherAchievement) {
            switch (GameDirector.Instance.CrusherIndex) {
                case 0:
                    for (int i = 0; i < GameDirector.Instance.GirlAchievements.Count; i++)
                        PlayerPrefs.SetInt("girlAchievement" + i + "Data", GameDirector.Instance.GirlAchievements[i]);
                    break;
                case 1:
                    for (int i = 0; i < GameDirector.Instance.QueenOfHeartsAchievements.Count; i++)
                        PlayerPrefs.SetInt("queenOfHeartsAchievement" + i + "Data", GameDirector.Instance.QueenOfHeartsAchievements[i]);
                    break;
                case 2:
                    for (int i = 0; i < GameDirector.Instance.TenjinAchievements.Count; i++)
                        PlayerPrefs.SetInt("tenjinAchievement" + i + "Data", GameDirector.Instance.TenjinAchievements[i]);
                    break;
                case 3:
                    for (int i = 0; i < GameDirector.Instance.WitchAchievements.Count; i++)
                        PlayerPrefs.SetInt("witchAchievement" + i + "Data", GameDirector.Instance.WitchAchievements[i]);
                    break;
                default:
                    break;
            }
            // Achievement情報をセーブする.
            PlayerPrefs.Save();
        }

        // ビルダーの情報の変更があったら
        if (isChangedBuilderAchievement) {
            switch (GameDirector.Instance.BuilderIndex) {
                case 0:
                    for (int i = 0; i < GameDirector.Instance.WolfAchievements.Count; i++)
                        PlayerPrefs.SetInt("wolfAchievement" + i + "Data", GameDirector.Instance.WolfAchievements[i]);
                    break;
                case 1:
                    for (int i = 0; i < GameDirector.Instance.QueenAliceAchievements.Count; i++)
                        PlayerPrefs.SetInt("queenAliceAchievement" + i + "Data", GameDirector.Instance.QueenAliceAchievements[i]);
                    break;
                case 2:
                    for (int i = 0; i < GameDirector.Instance.MikadoAchievements.Count; i++)
                        PlayerPrefs.SetInt("mikadoAchievement" + i + "Data", GameDirector.Instance.MikadoAchievements[i]);
                    break;
                case 3:
                    for (int i = 0; i < GameDirector.Instance.HanzelGretelAchievements.Count; i++)
                        PlayerPrefs.SetInt("hanzelGretelAchievement" + i + "Data", GameDirector.Instance.HanzelGretelAchievements[i]);
                    break;
                default:
                    break;
            }
            // Achievement情報をセーブする.
            PlayerPrefs.Save();
        }
    }
}
