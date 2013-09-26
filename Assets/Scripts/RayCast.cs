using UnityEngine;
using System.Collections;

public class RayCast : MonoBehaviour
{
    // Camera Transform
    Transform mainCamTrans;

    // Camera Vectors
    Vector3 mainCamRotation;
    Vector3 mainCamRotationPrev;
    Vector3 playerRotation;
    Vector3 playerRotationPrev;

    // Player position current and previous frame
    Vector3 playerPosition, playerPositionPrev;

    // Gæt
    float rayLength = 20;

    Collider currentMovingObject;

    // Use this for initialization
    void Start()
    {
        mainCamTrans = GetComponentInChildren(typeof(Camera)).transform;
        currentMovingObject = null;
    }

    // Update is called once per frame
    void Update()
    {
        mainCamRotationPrev = mainCamRotation;
        mainCamRotation = mainCamTrans.localEulerAngles;

        playerRotationPrev = playerRotation;
        playerRotation = transform.localEulerAngles;

        playerPositionPrev = playerPosition;
        playerPosition = transform.position;

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            // DEbug
            Debug.DrawRay(mainCamTrans.position, mainCamTrans.TransformDirection(Vector3.forward * rayLength), Color.green);

            // Ray "Collider" & drawing raycast.
            RaycastHit hit;
            Ray ray = new Ray(mainCamTrans.position, mainCamTrans.TransformDirection(Vector3.forward));

            if (Physics.Raycast(ray, out hit, rayLength))
            {
                if (hit.collider.CompareTag("Movable"))
                {
                    currentMovingObject = hit.collider;

                    (hit.collider.GetComponentInChildren(typeof(Rigidbody)) as Rigidbody).useGravity = false;

                }
            }
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            if (currentMovingObject != null)
            {
                (currentMovingObject.GetComponentInChildren(typeof(Rigidbody)) as Rigidbody).useGravity = true;
                currentMovingObject = null;
            }
        }

        if (currentMovingObject != null)
        {
            currentMovingObject.rigidbody.velocity = Vector3.zero;
            currentMovingObject.rigidbody.angularVelocity = Vector3.zero;

            // Rotate collider around Main Camera.
            float deltaYrotation = playerRotation.y - playerRotationPrev.y;
            float deltaXrotation = mainCamRotation.x - mainCamRotationPrev.x;

            currentMovingObject.transform.RotateAround(mainCamTrans.position, transform.TransformDirection(Vector3.up), deltaYrotation);
            currentMovingObject.transform.RotateAround(mainCamTrans.position, mainCamTrans.TransformDirection(Vector3.right), deltaXrotation);

            if (Input.GetKey(KeyCode.Mouse1))
            {
                Vector3 deltaVector = currentMovingObject.transform.position - transform.position;
                currentMovingObject.transform.position += deltaVector.normalized * Input.GetAxis("Mouse Y");
            }

            currentMovingObject.transform.position += (playerPosition - playerPositionPrev);
        }
    }
}
