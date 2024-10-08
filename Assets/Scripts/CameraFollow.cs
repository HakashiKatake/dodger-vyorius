using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    private Rigidbody playerRB;
    public Vector3 Offset;
    public float speed;

    public float zoomSpeed = 10f;
    public float minFOV = 60f;
    public float maxFOV = 90f;

    private Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        playerRB = player.GetComponent<Rigidbody>();
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        float playerSpeed = playerRB.velocity.magnitude;
        cam.fieldOfView = Mathf.Lerp(minFOV, maxFOV, playerSpeed / zoomSpeed);


        Vector3 playerForward = (playerRB.velocity + player.transform.forward).normalized;
        transform.position = Vector3.Lerp(transform.position,
            player.position + player.transform.TransformVector(Offset)
            + playerForward * (-5f),
            speed * Time.deltaTime);
        transform.LookAt(player);
    }
}