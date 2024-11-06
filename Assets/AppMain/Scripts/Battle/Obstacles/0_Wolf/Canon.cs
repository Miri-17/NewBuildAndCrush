using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canon : MonoBehaviour {
    private AudioSource _audioSource = null;
    private float _nextAttackTime = 0;

    [SerializeField] private GameObject _canonBulletPrefab = null;
    [SerializeField] private Transform _attackPoint = null;
    [SerializeField] private AudioClip _audioClip = null;
    [SerializeField, Header("1秒に何回攻撃できるか")] private float _attackRate = 0.5f;

    private void Start() {
        _audioSource = this.GetComponent<AudioSource>();
    }

    private void Update() {
        if (Time.time < _nextAttackTime || _attackPoint == null)
            return;
            
        Instantiate(_canonBulletPrefab, _attackPoint.transform);
        _audioSource.PlayOneShot(_audioClip);
        _nextAttackTime = Time.time + 1.0f / _attackRate;
    }
}
