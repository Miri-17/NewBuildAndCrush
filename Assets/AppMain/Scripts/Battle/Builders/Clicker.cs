using UnityEngine;

public class Clicker : MonoBehaviour {
    #region Serialized Fields
    [SerializeField] private Camera _builderCamera;
    [SerializeField] private BattleBuilderUIController _battleBuilderUIController = null;
    [SerializeField] private BuilderController _builderController;
    #endregion

    private void Update() {
        if (!_battleBuilderUIController.IsButtonDown)
            return;

        if (Input.GetMouseButtonDown(0)) {
        Debug.Log("Clicker");      
            Ray ray = _builderCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit2D = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction, LayerMask.GetMask("SpawnPoint"));
            Debug.Log(hit2D.collider.gameObject.layer);
            Debug.Log(hit2D.collider.gameObject.name);
            if (hit2D.collider != null && hit2D.collider.gameObject.layer == 9) {   // layer 9 は SpawnPoint です.
                Instantiate(_battleBuilderUIController.CurrentPrefabs, hit2D.collider.gameObject.transform.position, Quaternion.identity, _builderController.WagonController.transform);
                SpawnPoint spawnPoint = hit2D.collider.gameObject.GetComponent<SpawnPoint>();
                spawnPoint.SetOccupied(true);
                _battleBuilderUIController.SetButtonUp();
            }
        }
    }
}
