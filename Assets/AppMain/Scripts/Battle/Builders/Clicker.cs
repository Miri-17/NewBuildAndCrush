using UnityEngine;

public class Clicker : MonoBehaviour {
    // ObstacleButtonManagerでも使用する変数.
    // public int[] isOtherButtonDown;

    // [SerializeField]
    // private Camera builderCamera;
    // [SerializeField]
    // // private ObstacleButtonManager[] obstacleButtonManager;
    // [SerializeField]
    // private BuilderController builderController;

    // private Ray _ray;
    // private RaycastHit2D _hit;
    // private bool isClick = false;

    // private void Update() {
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
    // }

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
