using UnityEngine;

public class SpawnPoint : MonoBehaviour {
    private SpriteRenderer _spriteRenderer;

    [SerializeField] private Color _greenColor = new Color(0, 1, 0, 0.2f);
    [SerializeField] private Color _redColor = new Color(1, 0, 0, 0.2f);

    public bool IsOccupied { get; private set; } = false;

    /// <summary>
    /// SetBaseBlockで使用する.
    /// </summary>
    public int X = 0;
    public int Y = 0;

    private void Awake() {
        _spriteRenderer = this.GetComponent<SpriteRenderer>();
    }

    private void Start() {
        _spriteRenderer = this.GetComponent<SpriteRenderer>();
        SetInfo();
    }

    private void OnEnable() {
        _spriteRenderer = this.GetComponent<SpriteRenderer>();
        SetInfo();
    }

    private void SetInfo() {
        if (IsOccupied == true) {
            // Debug.Log("occupied: (X " + X + ", Y " + Y + ")");
            _spriteRenderer.color = _redColor;
            this.gameObject.layer = 10;
        } else {
            // Debug.Log("not occupied: (X " + X + ", Y " + Y + ")");
            _spriteRenderer.color = _greenColor;
            this.gameObject.layer = 9;
        }
    }

    /// <summary>
    /// Spawnできない場所にする.
    /// </summary>
    /// <param name="isOccupied"></param>
    public void SetOccupied(bool isOccupied) {
        IsOccupied = isOccupied;
    }
}
