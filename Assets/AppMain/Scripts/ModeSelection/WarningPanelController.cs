using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningPanelController : MonoBehaviour {
    private AudioSource _audioSource_SE = null;
    private int _warningIndex = 0;

    [SerializeField] private List<ButtonWithY> _buttonsWithY = new List<ButtonWithY>();
    [SerializeField] private AchievementsController _achievementsController = null;

    private void Start() {
        _audioSource_SE = CrusherSE.Instance.GetComponent<AudioSource>();

        _buttonsWithY[0].SetSelection(true);
    }

    private void Update() {
        if (Input.GetButtonDown("Horizontal")) {
            if (_warningIndex == 0) {
                _buttonsWithY[0].SetSelection(false);
                _buttonsWithY[1].SetSelection(true);
                _warningIndex = 1;
            } else {
                _buttonsWithY[1].SetSelection(false);
                _buttonsWithY[0].SetSelection(true);
                _warningIndex = 0;
            }

            _audioSource_SE.PlayOneShot(CrusherSE.Instance.SEDB.AudioClips[1]);
        } else if (Input.GetButtonDown("Select")) {
            if (_warningIndex == 0) {
                this.gameObject.SetActive(false);
            } else {
                _audioSource_SE.PlayOneShot(CrusherSE.Instance.SEDB.AudioClips[3]);
                ResetData();
                // Warning Panel表示時はデータ消去を選択してない方が選択.
                _warningIndex = 0;
                _buttonsWithY[1].SetSelection(false);
                _buttonsWithY[0].SetSelection(true);

                this.gameObject.SetActive(false);
            }
        }
    }

    private void ResetData() {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        _achievementsController.LoadPlayerPrefs();
        _achievementsController.UpdateAchievementImages();
    }
}
