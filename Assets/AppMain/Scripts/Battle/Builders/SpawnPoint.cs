using UnityEngine;

public class SpawnPoint : MonoBehaviour {
    private SpriteRenderer _spriteRenderer;

    [SerializeField] private Color _greenColor = new Color(0, 1, 0, 0.2f);
    [SerializeField] private Color _redColor = new Color(1, 0, 0, 0.2f);

    // TODO 要確認
    #region
    public int x = 0;
    public int y = 0;
    public bool isOccupied = false;
    #endregion

    private void Start() {
        _spriteRenderer = this.GetComponent<SpriteRenderer>();
    }

    private void Update() {
        if (isOccupied == true) {
            _spriteRenderer.color = _redColor;
            this.gameObject.layer = 10;
        } else {
            _spriteRenderer.color = _greenColor;
            this.gameObject.layer = 9;
        }
    }
}
