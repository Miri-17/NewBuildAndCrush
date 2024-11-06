using UnityEngine;

public class FrogNote : MonoBehaviour {
    // [Header("n番目")] [SerializeField] private int notesNum = 1;
    // [SerializeField] private float _movementRateY = 0;

    // private Transform other;
    // private int point;
    // private CancellationToken _ct;


    // async void Start()
    // {
    //     _ct = destroyCancellationToken;
    //     if (gameObject != null)
    //     {
    //         if (notesNum == 1)
    //         {
    //             transform.DOMoveY(transform.position.y + 20, 0.5f).SetEase(Ease.InOutBack).SetLoops(-1, LoopType.Yoyo).SetLink(gameObject);
    //         }

    //         await UniTask.Delay(TimeSpan.FromSeconds(3f),cancellationToken:_ct);
    //         Destroy(gameObject);
    //     }
    // }

    // private void Update() {

    // }

    // // Update is called once per frame
    // void Update()
    // {
    //     if (notesNum == 1)
    //     {
    //         transform.Translate(-50 * Time.deltaTime, 0 * Time.deltaTime, 0);
    //     }

    //     if (notesNum == 2)
    //     {
    //         transform.Translate(-50 * Time.deltaTime, 20 * Time.deltaTime, 0);
    //     }

    //     if (notesNum == 3)
    //     {
    //         transform.Translate(-50 * Time.deltaTime, 40 * Time.deltaTime, 0);
    //     }
    // }

    // private void OnCollisionEnter2D(Collision2D other)
    // {
    //     if (!other.collider.CompareTag("Obstacle"))
    //     {
    //         Destroy(gameObject);
    //     }
    // }
}
