using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuilderSelectionUIController : MonoBehaviour {
    #region Serialized Fields
    [SerializeField] private Image _centerImage = null;
    [SerializeField] private Image _statusImage = null;
    [SerializeField] private List<Sprite> _centerSprites = new List<Sprite>();
    [SerializeField] private List<Sprite> _statusSprites = new List<Sprite>();
    #endregion

    private void Start() {
        _statusImage.alphaHitTestMinimumThreshold = 1;
    }

    public void SetSprites(int index) {
        _centerImage.sprite = _centerSprites[index];
        _statusImage.sprite = _statusSprites[index];
    }
}
