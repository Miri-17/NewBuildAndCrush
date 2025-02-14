using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuilderSelectionController : MonoBehaviour {
    #region Private Fields
    private GameObject _builderL2D = null;
    private int _previousBuilderIndex = 0;
    private int _builderCount = 0;
    private AudioSource _audioSource_SE = null;
    private AudioClip _audioClip_SE = null;
    private Animator _builderAnimator = null;
    private bool _isBuilderSelected = false;
    #endregion

    #region Serialized Fields
    [SerializeField] private BuildersDB _buildersDB = null;
    [SerializeField] private BuilderSelectionUIController _builderSelectionUIController = null;
    [SerializeField] private CrusherSelectionController _crusherSelectionController = null;
    [SerializeField] private TextMeshProUGUI _builderNickname = null;
    [SerializeField] private TextMeshProUGUI _builderName = null;
    [SerializeField] private TextMeshProUGUI _builderDescription = null;
    [SerializeField] private GameObject _statusPanel = null;
    [SerializeField] private Button _displayStatusButton = null;
    [SerializeField] private Button _hideStatusButton = null;
    [SerializeField] private Button[] _leftButtons = new Button[0];
    [SerializeField] private Button[] _rightButtons = new Button[0];
    [SerializeField] private Button _selectionButton = null;
    // WaitingPanel
    [SerializeField] private GameObject _waitingPanel = null;
    [SerializeField] private GameObject[] _waitingCharacters = new GameObject[0];
    [SerializeField] private Button _deselectButton = null;
    #endregion

    #region
    public int BuilderIndex { get; private set; } = 0;
    public BuilderDB Builder { get; private set; }
    public bool IsBuilderSelected { get => _isBuilderSelected; set => _isBuilderSelected = value; }
    public Animator BuilderAnimator { get => _builderAnimator; set => _builderAnimator = value; }
    #endregion

    private void Start() {
        if (BuilderSE.Instance == null) {
            Debug.LogError("SE instance is not available.");
            return;
        }

        _audioSource_SE = BuilderSE.Instance.GetComponent<AudioSource>();
        
        _builderCount = _buildersDB.BuilderLength;
        UpdateBuilder(BuilderIndex);

        _displayStatusButton.onClick.AddListener(() => OnDisplayStatusButtonClicked());
        _hideStatusButton.onClick.AddListener(() => OnHideStatusButtonClicked());
        foreach(var leftButton in _leftButtons)
            leftButton.onClick.AddListener(() => OnLeftButtonClicked());
        foreach(var rightButton in _rightButtons)
            rightButton.onClick.AddListener(() => OnRightButtonClicked());
        _selectionButton.onClick.AddListener(() => OnSelectionButtonClicked());
        
        _statusPanel.SetActive(false);
        _waitingPanel.SetActive(false);
        _deselectButton.onClick.AddListener(() => OnDeselectButtonClicked());
    }

    private void UpdateBuilder(int builderIndex) {
        Builder = _buildersDB.GetBuilder(builderIndex);
        var builderInfo = _buildersDB.GetBuilderInfo(builderIndex);

        _builderName.text = Builder.Name;
         if (builderIndex == 3)
            _builderName.fontSize = 95;
        else
            _builderName.fontSize = 130;
        _builderL2D = Instantiate(Builder.Live2D, new Vector3(0, 0, 0), Quaternion.identity);
        _builderL2D.transform.parent = GameObject.Find("Builder").transform;
        _builderL2D.transform.localPosition = GetBuilderPosition(builderIndex);
        _builderAnimator = _builderL2D.GetComponent<Animator>();

        _builderNickname.text = builderInfo.Nickname;
        if (builderIndex == 0)
            _builderNickname.fontSize = 50;
        else
            _builderNickname.fontSize = 60;
        _builderDescription.text = builderInfo.Description;
        if (builderIndex == 0 || builderIndex == 1)
            _builderDescription.lineSpacing = 80;
        else
            _builderDescription.lineSpacing = 60;
        
        _builderSelectionUIController.SetSprites(builderIndex);

        _leftButtons[_previousBuilderIndex].gameObject.SetActive(false);
        _rightButtons[_previousBuilderIndex].gameObject.SetActive(false);
        _leftButtons[builderIndex].gameObject.SetActive(true);
        _rightButtons[builderIndex].gameObject.SetActive(true);

        _waitingCharacters[_previousBuilderIndex].gameObject.SetActive(false);
        _waitingCharacters[builderIndex].gameObject.SetActive(true);

        _previousBuilderIndex = builderIndex;
    }

    private Vector3 GetBuilderPosition(int index) {
        switch (index) {
            case 0: return new Vector3(4.0f, 0, 80.0f);
            case 1: return new Vector3(2.8f, 0, 80.0f);
            case 2: return new Vector3(4.3f, 0, 80.0f);
            default: return new Vector3(3.9f, 0, 80.0f);
        }
    }

    private void OnSelectionButtonClicked() {
        if (_isBuilderSelected) return;
        
        _isBuilderSelected = true;
        
        _audioClip_SE = BuilderSE.Instance.SEDB.AudioClips[0];
        _audioSource_SE.PlayOneShot(_audioClip_SE);
        
        _builderAnimator = _builderL2D.GetComponent<Animator>();
        _builderAnimator.enabled = false;
        _waitingPanel.SetActive(true);
    }

    private void OnDisplayStatusButtonClicked() {
        _statusPanel.SetActive(true);
    }
    
    private void OnHideStatusButtonClicked() {
        _statusPanel.SetActive(false);
    }

    private void OnLeftButtonClicked() {
        if (_isBuilderSelected) return;
        
        BuilderIndex--;
        if (BuilderIndex >= _builderCount)
            BuilderIndex = 0;
        else if (BuilderIndex < 0)
            BuilderIndex = _builderCount - 1;
        
        _audioClip_SE = BuilderSE.Instance.SEDB.AudioClips[1];
        _audioSource_SE.PlayOneShot(_audioClip_SE);

        if (_builderL2D != null)
            Destroy(_builderL2D);
        UpdateBuilder(BuilderIndex);
    }

    private void OnRightButtonClicked() {
        if (_isBuilderSelected) return;
        
        BuilderIndex++;
        if (BuilderIndex >= _builderCount)
            BuilderIndex = 0;
        else if (BuilderIndex < 0)
            BuilderIndex = _builderCount - 1;
        
        _audioClip_SE = BuilderSE.Instance.SEDB.AudioClips[1];
        _audioSource_SE.PlayOneShot(_audioClip_SE);

        if (_builderL2D != null)
            Destroy(_builderL2D);
        UpdateBuilder(BuilderIndex);
    }

    private void OnDeselectButtonClicked() {
        if (_crusherSelectionController.IsSetConfirmPanel)
            return;

        _isBuilderSelected = false;

        _audioClip_SE = BuilderSE.Instance.SEDB.AudioClips[2];
        _audioSource_SE.PlayOneShot(_audioClip_SE);

        _builderAnimator.enabled = true;
        _waitingPanel.SetActive(false);
    }
}
