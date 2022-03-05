using UnityEngine;
 
public class PlayerMouvementController : MonoBehaviour
{
    CharacterController characterController;
    [HideInInspector]
    public float MovementSpeed =1;
    private float Gravity = 9.8f;
    private bool asGravity = false;
    private float velocity = 0;
    private Camera cam;
    
    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        cam = Camera.main;
    }
 
    void Update()
    {
        // player movement - forward, backward, left, right
        float horizontal = Input.GetAxis("Horizontal") * MovementSpeed;
        float vertical = Input.GetAxis("Vertical") * MovementSpeed;
        characterController.Move((cam.transform.right * horizontal + cam.transform.forward * vertical) * Time.deltaTime);
 
        // Gravity
        // if(characterController.isGrounded && asGravity)
        // {
        //     velocity = 0;
        // }
        // else
        // {
        //     velocity -= Gravity * Time.deltaTime;
        //     characterController.Move(new Vector3(0, velocity, 0));
        // }
    }
}
 