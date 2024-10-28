using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BuilderSelectionController : MonoBehaviour {
    private GameObject _builderL2D = null;
    private int _builderIndex = 0;
    private int _builderCount = 0;
    private bool _isBuilderSelected = false;
    private AudioSource _audioSource_SE = null;
    private AudioClip _audioClip_SE = null;

    [SerializeField] private BuildersDB _buildersDB = null;
    [SerializeField] private TextMeshProUGUI _builderName = null;
    [SerializeField] private TextMeshProUGUI _builderNickname = null;
    [SerializeField] private TextMeshProUGUI _builderDescription = null;
    [SerializeField] private Button _leftArrowButton = null;
    [SerializeField] private Button _rightArrowButton = null;
    [SerializeField] private Button _confirmButton = null;

    public Animator BuilderAnimator { get; private set; } = null;
    public BuilderDB Builder { get; private set; } = null;
    public bool IsBuilderSelected { get => _isBuilderSelected; set => _isBuilderSelected = value; }

    private void Start() {
        if (BuilderSE.Instance == null) {
            Debug.LogError("SE instance is not available.");
            return;
        }

        _audioSource_SE = BuilderSE.Instance.GetComponent<AudioSource>();
        
        _builderCount = _buildersDB.BuilderCount;
        UpdateBuilder(_builderIndex);

        _confirmButton.onClick.AddListener(() => OnConfirmButtonClicked());
        _leftArrowButton.onClick.AddListener(() => OnArrowButtonClicked(-1));
        _rightArrowButton.onClick.AddListener(() => OnArrowButtonClicked(1));
    }

    private void UpdateBuilder(int builderIndex) {
        Builder = _buildersDB.GetBuilder(builderIndex);
        var builderInfo = _buildersDB.GetBuilderInfo(builderIndex);

        _builderName.text = Builder.Name;
        _builderL2D = Instantiate(Builder.Live2D, new Vector3(0, 0, 80.0f), Quaternion.identity);
        _builderL2D.transform.SetParent(GameObject.Find("Builder").transform, false);
        BuilderAnimator = _builderL2D.GetComponent<Animator>();

        _builderNickname.text = builderInfo.Nickname;
        _builderDescription.text = builderInfo.Description;
    }

    private void OnConfirmButtonClicked() {
        if (_isBuilderSelected)
            return;
        
        _isBuilderSelected = true;
        
        _audioClip_SE = BuilderSE.Instance.SEDB.AudioClips[0];
        _audioSource_SE.PlayOneShot(_audioClip_SE);
        
        var builderAnimator = _builderL2D.GetComponent<Animator>();
        builderAnimator.Play(Builder.EnglishName + "L2D_Select");
    }

    private void OnArrowButtonClicked(int number) {
        if (_isBuilderSelected)
            return;
        
        _builderIndex += number;
        if (_builderIndex >= _builderCount)
            _builderIndex = 0;
        else if (_builderIndex < 0)
            _builderIndex = _builderCount - 1;
        
        _audioClip_SE = BuilderSE.Instance.SEDB.AudioClips[1];
        _audioSource_SE.PlayOneShot(_audioClip_SE);

        if (_builderL2D != null)
            Destroy(_builderL2D);
        UpdateBuilder(_builderIndex);
    }
}
