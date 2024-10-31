using UnityEngine;

public class Clicker : MonoBehaviour {
    // ObstacleButtonManagerでも使用する変数.
    // public int[] isOtherButtonDown;

    [SerializeField] private Camera _builderCamera;
    // [SerializeField]
    // // private ObstacleButtonManager[] obstacleButtonManager;
    [SerializeField] private BattleBuilderUIController _battleBuilderUIController = null;
    [SerializeField] private BuilderController _builderController;

    // private Ray _ray;
    // private RaycastHit2D _hit;
    // private bool isClick = false;

    private void Update() {
        if (!_battleBuilderUIController.IsButtonDown)
            return;

        if (Input.GetMouseButtonDown(0)) {
        Debug.Log("Clicker");      
            Ray ray = _builderCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit2D = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction, LayerMask.GetMask("SpawnPoint"));
            Debug.Log(hit2D.collider.gameObject.layer);
            Debug.Log(hit2D.collider.gameObject.name);
            // if (hit2D.collider != null && hit2D.collider.gameObject.layer == 12) {   // layer 12 は SpawnPoint です
            if (hit2D.collider != null && hit2D.collider.gameObject.layer == 9) {   // layer 12 は SpawnPoint です
                Instantiate(_battleBuilderUIController.CurrentPrefabs, hit2D.collider.gameObject.transform.position, Quaternion.identity, _builderController.WagonController.transform);
                SpawnPoint spawnPoint = hit2D.collider.gameObject.GetComponent<SpawnPoint>();
                // spawnPoint.IsOccupied = true;
                spawnPoint.SetOccupied(true);
                _battleBuilderUIController.SetButtonUp();
            }
        }
    //     // foreach (var manager in obstacleButtonManager) {
    //         // ある1つの障害物のボタンが押されたら
    //         if (manager.isButtonDown) {
    //             // 他のボタンをinteractableでなくする.
    //             Method1(manager.index);

    //             if (Input.GetMouseButtonDown(0))
    //             {
    //                 isClick = true;
    //                 Ray ray = builderCamera.ScreenPointToRay(Input.mousePosition);
    //                 RaycastHit2D hit2d = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction, LayerMask.GetMask("SpawnPoint"));
    //                 if (isClick && hit2d.collider != null && hit2d.collider.gameObject.layer == 9)
    //                 {
    //                     manager.isButtonDown = false;
    //                     // Debug.Log(hit2d.collider.gameObject.name);
    //                     // Debug.Log(hit2d.collider.gameObject.transform.position);
    //                     isClick = false;
    //                     // GameObject obstacle = Instantiate(manager.obstaclePrefabs[GameDirector.Instance.builderIndex], hit2d.collider.gameObject.transform.position, Quaternion.identity);
    //                     // obstacle.transform.parent = builderController.wagon.transform;
    //                     // builderController.wagon.transform.Find("Grid").transform.gameObject.SetActive(false);
    //                     // Debug.Log(hit2d.collider.gameObject.layer);
    //                     hit2d.collider.gameObject.layer = 10;
    //                     SpawnPoint spawnPoint = hit2d.collider.gameObject.GetComponent<SpawnPoint>();
    //                     spawnPoint.isOccupied = true;
    //                     manager.SetPartsGenerationBar();
    //                     manager.SetWeighingBar();
    //                     // 他のボタンをinteractableにする.
    //                     Method2(manager.index);
    //                 }
    //             }
    //         }
    //     }
    }

    private bool Hello() {
        Debug.Log("hello");
        return true;
    }

    // private void Method1(int index)
    // {
    //     for (int i = 0; i < 6; i++)
    //     {
    //         if (i == index)
    //         {
    //             continue;
    //         }
    //         isOtherButtonDown[i] = 1;
    //     }
    // }

    // private void Method2(int index)
    // {
    //     for (int i = 0; i < 6; i++)
    //     {
    //         if (i == index)
    //         {
    //             continue;
    //         }
    //         isOtherButtonDown[i] = 2;
    //     }
    // }
}
