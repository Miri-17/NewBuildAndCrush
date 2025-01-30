using UnityEngine;
using UnityEngine.UI;

public class BuilderSelectionUIController : MonoBehaviour {
    #region Serialized Fields
    [SerializeField] private Image _centerImage = null;
    [SerializeField] private Image _statusImage = null;
    [SerializeField] private Sprite[] _centerSprites = new Sprite[0];
    [SerializeField] private Sprite[] _statusSprites = new Sprite[0];
    #endregion

    private void Start() {
        _statusImage.alphaHitTestMinimumThreshold = 1;
    }

    /// <summary>
    /// 表示中のビルダーによってボタンに挟まれた部分とステータスの画像を変更する.
    /// ボタンの画像は, 押した時に表示を変える都合上 BuilderSelectionController.cs で変更しているので注意してください.
    /// </summary>
    /// <param name="index"></param>
    public void SetSprites(int index) {
        _centerImage.sprite = _centerSprites[index];
        _statusImage.sprite = _statusSprites[index];
    }
}
