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
            // Debug.Log(hit2D.collider.gameObject.layer);
            // Debug.Log(hit2D.collider.gameObject.name);
            if (hit2D.collider != null && hit2D.collider.gameObject.layer == 9) {   // layer 9 は SpawnPoint です.
                var obstacle = Instantiate(_battleBuilderUIController.CurrentPrefabs, new Vector3(0, 0, 0), Quaternion.identity);
                obstacle.transform.parent = _builderController.WagonController.transform;
                obstacle.transform.localPosition = new Vector3(hit2D.collider.gameObject.transform.localPosition.x, hit2D.collider.gameObject.transform.localPosition.y, -1f);
                SpawnPoint spawnPoint = hit2D.collider.gameObject.GetComponent<SpawnPoint>();
                spawnPoint.SetOccupied(true);
                _battleBuilderUIController.SetButtonUp();
            }
        }
    }
}
