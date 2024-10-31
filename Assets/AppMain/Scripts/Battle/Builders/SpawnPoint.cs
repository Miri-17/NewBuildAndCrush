using UnityEngine;

public class SpawnPoint : MonoBehaviour {
    // #region
    // public int X;
    // public int Y;
    // public bool IsOccupied;
    // #endregion

    // #region
    // [SerializeField]
    // private Color greenColor;
    // [SerializeField]
    // private Color redColor;
    // #endregion

    // private SpriteRenderer rend;

    // private void Start() {
    //     rend = GetComponent<SpriteRenderer>();

    //     greenColor = new Color(0, 1, 0, 0.2f);
    //     redColor = new Color(1, 0, 0, 0.2f);
    // }

    // private void Update() {
    //     if (IsOccupied == true)
    //     {
    //         Debug.Log("a");
    //         rend.color = redColor;
    //         this.gameObject.layer = 10;
    //     }
    //     else
    //     {
    //         rend.color = greenColor;
    //         this.gameObject.layer = 9;
    //     }
    // }
    private SpriteRenderer _spriteRenderer;

    [SerializeField] private Color _greenColor = new Color(0, 1, 0, 0.2f);
    [SerializeField] private Color _redColor = new Color(1, 0, 0, 0.2f);

    public bool IsOccupied { get; private set; } = false;

    /// <summary>
    /// SetBaseBlockで使用する.
    /// </summary>
    public int X = 0;
    public int Y = 0;
    // public bool IsOccupied = false;

    private void Awake() {
        _spriteRenderer = this.GetComponent<SpriteRenderer>();
    }

    private void Start() {
        _spriteRenderer = this.GetComponent<SpriteRenderer>();
        // SetOccupied(true);
        SetInfo();
    }

    private void OnEnable() {
        _spriteRenderer = this.GetComponent<SpriteRenderer>();
        // Debug.Log("enable");
        SetInfo();
    }

    private void SetInfo() {
        if (IsOccupied == true) {
            Debug.Log("occupy");
            _spriteRenderer.color = _redColor;
            this.gameObject.layer = 10;
        } else {
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
        Debug.Log("set occupy: " + IsOccupied);
    }
}
