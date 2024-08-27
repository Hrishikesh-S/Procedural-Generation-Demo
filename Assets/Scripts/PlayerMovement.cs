using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float gravity;
    [SerializeField] private float jumpHeight;
    private CharacterController controller;
    private Vector3 input;
    private Vector3 vVelocity;
    private bool grounded;
    private Transform mainCam;

    private readonly int chunkSize = WorldGenData.chunkSize;


    private void Start()
    {
        mainCam = Camera.main.transform;
        controller = GetComponent<CharacterController>();

        //transform.position = new Vector3(chunkSize / 2, transform.position.y, chunkSize / 2);
    }

    private void Update()
    {
        grounded = false;

        input = (mainCam.right * Input.GetAxisRaw("Horizontal") + mainCam.forward * Input.GetAxisRaw("Vertical"));
        input.y = 0;
        input = input.normalized;
        controller.Move(input * moveSpeed * Time.deltaTime);

        vVelocity.y -= gravity * Time.deltaTime;
        controller.Move(vVelocity * Time.deltaTime);

        if (grounded && vVelocity.y < 0)
        {
            vVelocity.y = -1;
            if (Input.GetButtonDown("Jump"))
                vVelocity.y = jumpHeight;
        }
        else if (Input.GetButton("Jump"))
             vVelocity.y = jumpHeight;
    }
}
