using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageGenerator : MonoBehaviour {
    [SerializeField] private BuildersDB _buildersDB = null;
    [SerializeField] private SpriteRenderer _bg;
    [SerializeField] private SpriteRenderer _ground;

    private void Start() {
        UpdateStage(GameDirector.Instance.BuilderIndex);
    }

    private void UpdateStage(int builderIndex) {
        var battleBuilder = _buildersDB.GetBattleBuilder(builderIndex);

        _bg.sprite = battleBuilder.Bg;
        _ground.sprite = battleBuilder.Ground;
    }
}
