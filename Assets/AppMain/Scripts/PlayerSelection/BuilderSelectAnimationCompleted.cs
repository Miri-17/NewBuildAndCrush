using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BuilderSelectAnimationCompleted : MonoBehaviour {
    private GameObject _panel = null;
    private BuilderSelectionController _builderSelectionController = null;
    
    [SerializeField] private GameObject _warningPanel = null;
    [SerializeField] private string _parentName = "";

    private void Start() {
        if (SceneManager.GetActiveScene().name != "PlayerSelection") {
            Destroy(this.GetComponent<BuilderSelectAnimationCompleted>());
            return;
        }

        _builderSelectionController = GameObject.Find("BuilderSelectionController").GetComponent<BuilderSelectionController>();
    }

    public void OnCompleteAnimation() {
        _panel = Instantiate(_warningPanel);
        _panel.transform.SetParent(GameObject.Find(_parentName).transform, false);
        _panel.SetActive(true);

        var backButton = _panel.transform.Find("BackButton").GetComponent<Button>();
        backButton.onClick.AddListener(() => OnBackButtonClicked());
    }

    private void OnBackButtonClicked() {
        _builderSelectionController.IsBuilderSelected = false;
        _builderSelectionController.BuilderAnimator.Play(_builderSelectionController.Builder.EnglishName + "L2D_Idle");

        Destroy(_panel);
    }
}
