using UnityEngine;

public class CrusherTriggerCheck : MonoBehaviour {
    [SerializeField] private string _crusherTag = "Crusher";

    public bool IsOn {get; private set; } = false;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == _crusherTag) {
            IsOn = true;
            Debug.Log("Trigger On");
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.tag == _crusherTag) {
            IsOn = false;
            Debug.Log("Trigger Off");
        }
    }

    /// <summary>
    /// ワゴン内ではないがワゴンの近くで亡くなった場合, 重力や音楽はこのIsOnがtrueにならないと変更されない.
    /// StageGenerator.csでそうなった場合に強制的に呼ぶ.
    /// </summary>
    public void CrusherIsOn() {
        Debug.Log("false Trigger On");
        IsOn = true;
    }
    
    public void CrusherIsOff() {
        Debug.Log("false Trigger Off");
        IsOn = false;
    }
}
