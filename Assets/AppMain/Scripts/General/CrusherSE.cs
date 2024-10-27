using UnityEngine;

public class CrusherSE : MonoBehaviour {
    public static CrusherSE Instance { get; private set; }

    [SerializeField] private SEDB _seDB = null;

    public SEDB SEDB { get => _seDB; set => _seDB = value; }

    private void Awake() {
        // すでにロードされていたら、自分自身を破棄して終了する.
        if (Instance != null) {
            Destroy(this.gameObject);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
}
