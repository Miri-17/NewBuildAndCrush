using UnityEngine;
using UnityEngine.UI;

public class CrusherSelectionUIController : MonoBehaviour {
    #region Serialized Fields
    [SerializeField] private Image _centerImage = null;
    [SerializeField] private Image _leftImage = null;
    [SerializeField] private Image _rightImage = null;
    [SerializeField] private Image _statusImage = null;
    [SerializeField] private Sprite[] _centerSprites = new Sprite[0];
    [SerializeField] private Sprite[] _leftSprites = new Sprite[0];
    [SerializeField] private Sprite[] _rightSprites = new Sprite[0];
    [SerializeField] private Sprite[] _statusSprites = new Sprite[0];
    #endregion

    /// <summary>
    /// 表示中のクラッシャーによってボタンとステータスの画像を変更する.
    /// </summary>
    /// <param name="index"></param>
    public void SetSprites(int index) {
        _leftImage.sprite = _leftSprites[index];
        _centerImage.sprite = _centerSprites[index];
        _rightImage.sprite = _rightSprites[index];
        _statusImage.sprite = _statusSprites[index];
    }
}
