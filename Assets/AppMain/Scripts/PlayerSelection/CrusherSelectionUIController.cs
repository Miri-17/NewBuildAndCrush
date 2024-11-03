using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrusherSelectionUIController : MonoBehaviour {
    #region Serialized Fields
    [SerializeField] private Image _centerImage = null;
    [SerializeField] private Image _leftImage = null;
    [SerializeField] private Image _rightImage = null;
    [SerializeField] private Image _statusImage = null;
    [SerializeField] private List<Sprite> _centerSprites = new List<Sprite>();
    [SerializeField] private List<Sprite> _leftSprites = new List<Sprite>();
    [SerializeField] private List<Sprite> _rightSprites = new List<Sprite>();
    [SerializeField] private List<Sprite> _statusSprites = new List<Sprite>();
    #endregion

    public void SetSprites(int index) {
        _leftImage.sprite = _leftSprites[index];
        _centerImage.sprite = _centerSprites[index];
        _rightImage.sprite = _rightSprites[index];
        _statusImage.sprite = _statusSprites[index];
    }
}
