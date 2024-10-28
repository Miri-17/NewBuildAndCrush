using UnityEngine;

public class AnimationCompleted : MonoBehaviour {
    public void OnCompleteAnimation() {
        Destroy(this.gameObject);
    }
}
