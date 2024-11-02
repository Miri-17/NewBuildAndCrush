using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuilderSelectionUIController : MonoBehaviour {
    #region Serialized Fields
    [SerializeField] private Image _centerImage = null;
    [SerializeField] private List<Sprite> _centerSprites = new List<Sprite>();
    #endregion

    // 本当はStart関数で全てのWaitingCharactersをnot activeにするべき.

    public void SetSprites(int index) {
        _centerImage.sprite = _centerSprites[index];
    }
}
