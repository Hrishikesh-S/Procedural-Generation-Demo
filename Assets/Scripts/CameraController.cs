using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float sensitivity = 100;
    [SerializeField] private Transform player;
    private Transform cam;
    private float xRot;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        cam = Camera.main.transform;     
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
            CutTree();
    }

    void LateUpdate()
    {
        transform.position = player.position + Vector3.up;
        transform.Rotate(Vector3.up * sensitivity * Time.deltaTime * Input.GetAxis("Mouse X"));
        xRot -= Time.deltaTime * sensitivity * Input.GetAxis("Mouse Y");
        xRot = Mathf.Clamp(xRot, -90, 90);
        cam.localEulerAngles = new Vector3(xRot, 0, 0);
    }

    private void CutTree()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit) && hit.collider.CompareTag("Tree"))
        {
            hit.transform.gameObject.isStatic = false;
            hit.transform.gameObject.AddComponent<Rigidbody>();
            hit.transform.gameObject.GetComponent<Rigidbody>().AddForce(transform.forward / 10, ForceMode.Impulse);
        }
    }
}
