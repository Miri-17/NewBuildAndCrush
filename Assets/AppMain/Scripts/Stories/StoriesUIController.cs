using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class StoriesUIController : MonoBehaviour {
    [SerializeField] private List<Image> _fadeInOutImages = new List<Image>();

    private void Start() {
        foreach (var image in _fadeInOutImages) {
            image.DOFade(0, 0.2f)
                .SetEase(Ease.Linear)
                .SetLink(image.gameObject);
        }
    }

    public void TransitionUI(float duration) {
        foreach (var image in _fadeInOutImages) {
            image.DOFade(1.0f, duration)
                .SetEase(Ease.Linear)
                .SetLink(image.gameObject);
        }
    }
}
