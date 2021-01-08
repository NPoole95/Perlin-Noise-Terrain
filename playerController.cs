using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    // movement keycodes
    KeyCode forwards = KeyCode.W;
    KeyCode backwards = KeyCode.S;
    KeyCode left = KeyCode.A;
    KeyCode right = KeyCode.D;
    KeyCode jump = KeyCode.Space;

    [SerializeField]
    GameObject player;
    [SerializeField]
    Camera camera;
    Vector3 jumpForce = new Vector3(0.0f, 300.0f, 0.0f);



    //////////////
    //Player Movement

    float playerMoveSpeed = 3; // the players base movement speed
    float playerRotateSpeed = 40; // the players base speed
    float moveSpeed;       // the final speed the player will move at (playerSpeed * frameTime)
    float rotateSpeed;     // the final speed at which the player will rotate on the y axis (playerRotateSpeed * frameTime)
    float rotateXSpeed;     // the final speed at which the player will rotate on the X axis
    bool isGrounded = false;
    public float minAngle = -45.0f; // the minimum and maximum angles the player can rotate the camera on the x axis
    public float maxAngle = 45.0f;
    float yRotate = 0.0f; //Rotation Value

    public Vector3 GetPosition()
    {
        return new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
    }

    // Start is called before the first frame update
    void Awake()
    {
        // lock and hide the mouse cursor
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        moveSpeed = playerMoveSpeed * Time.deltaTime;
        rotateSpeed = playerRotateSpeed * Time.deltaTime;
        rotateXSpeed = rotateSpeed / 3;

        if (Input.GetKey(forwards))
        {
            player.transform.position += player.transform.forward * moveSpeed;
        }
        if (Input.GetKey(backwards))
        {
            player.transform.position -= player.transform.forward * moveSpeed;
        }
        if (Input.GetKey(left))
        {
            player.transform.position -= player.transform.right * moveSpeed;
        }
        if (Input.GetKey(right))
        {
            player.transform.position += player.transform.right * moveSpeed;
        }
        // players can only jump when sstood on something
        if (Input.GetKey(jump) && isGrounded == true)
        {
            player.GetComponent<Rigidbody>().velocity = new Vector3(0.0f, 0.0f, 0.0f); // set the velocity to 0 before jumping to avoid inconsistency with forces
            player.GetComponent<Rigidbody>().AddForce(jumpForce);
            isGrounded = false;
        }

       // player.transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X"), 0) * rotateXSpeed);

        //Rotate Y view
       // yRotate += Input.GetAxis("Mouse Y") * rotateSpeed;
        //yRotate = Mathf.Clamp(yRotate, minAngle, maxAngle);
        //camera.transform.eulerAngles = new Vector3(-yRotate, camera.transform.rotation.eulerAngles.y, camera.transform.rotation.eulerAngles.z);
    }

    //void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.transform.position.y < player.transform.position.y)
    //    {
    //        player.GetComponent<Rigidbody>().useGravity = false;
    //        isGrounded = true;
    //    }
    //    player.GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 0, 0);
    //}
    //void OnCollisionExit(Collision collision)
    //{
    //    //Check for a match with the specified name on any GameObject that collides with your GameObject
    //    if (collision.gameObject.transform.position.y < player.transform.position.y)
    //    {
    //        player.GetComponent<Rigidbody>().useGravity = true;
    //        isGrounded = false;
    //    }
    //}
}
