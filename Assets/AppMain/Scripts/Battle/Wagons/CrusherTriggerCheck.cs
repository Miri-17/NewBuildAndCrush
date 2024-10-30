using UnityEngine;

public class CrusherTriggerCheck : MonoBehaviour {
    [SerializeField] private string _crusherTag = "Crusher";

    // TODO 外部参照もある場合は変更すること
    public bool IsOn {get; private set; } = false;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == _crusherTag)
            IsOn = true;
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.tag == _crusherTag)
            IsOn = false;
    }
}
