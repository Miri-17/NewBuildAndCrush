using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementsController : MonoBehaviour {
    [SerializeField] private List<Image> _crusherAchievementImages = new List<Image>();
    [SerializeField] private List<Image> _builderAchievementImages = new List<Image>();

    private List<List<int>> _achievementLists = new List<List<int>>();
    private GameDirector _gameDirector = null;

    private void Start() {
        _gameDirector = GameDirector.Instance;

        _achievementLists = new List<List<int>>() {
            _gameDirector.GirlAchievements,
            _gameDirector.QueenOfHeartsAchievements,
            _gameDirector.TenjinAchievements,
            _gameDirector.WitchAchievements,
            _gameDirector.WolfAchievements,
            _gameDirector.QueenAliceAchievements,
            _gameDirector.MikadoAchievements,
            _gameDirector.HanzelGretelAchievements,
        };

        LoadPlayerPrefs();
        UpdateAchievementImages();
    }

    private void LoadPlayerPrefs() {
        for (int i = 0; i < _achievementLists.Count; i++)
            for (int j = 0; j < _achievementLists[i].Count; j++)
                _achievementLists[i][j] = PlayerPrefs.GetInt($"{GetAchievementKey(i)}{j}Data", 0);
    }

    private string GetAchievementKey(int index) {
        return index switch {
            0 => "girlAchievement",
            1 => "queenOfHeartsAchievement",
            2 => "tenjinAchievement",
            3 => "witchAchievement",
            4 => "wolfAchievement",
            5 => "queenAliceAchievement",
            6 => "mikadoAchievement",
            7 => "hanzelGretelAchievement",
            _ => "unknownAchievement"
        };
    }

    private void UpdateAchievementImages() {
        UpdateAchievementImageList(_crusherAchievementImages, _achievementLists.Take(4).ToList());
        UpdateAchievementImageList(_builderAchievementImages, _achievementLists.Skip(4).Take(4).ToList());
    }

    private void UpdateAchievementImageList(List<Image> imageList, List<List<int>> achievementList) {
        if (imageList.Count != achievementList.Count) {
            Debug.LogError("The number of images does not match the number of achievement lists.");
            return;
        }

        for (int i = 0; i < imageList.Count; i++)
            imageList[i].enabled = achievementList[i].All(value => value != 0);
    }
}
