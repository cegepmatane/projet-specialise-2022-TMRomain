using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerScript : MonoBehaviour
{
    [Header("Player Mouvement")]
    [SerializeField]
    float playerSpeed = 3f;


    [Space(5)]
    [Header("Camera Mouvement")]
    [SerializeField]
     [Range(0.5f,5f)]
    float cameraHorizontal = 1f;
    [SerializeField]
    [Range(0.5f,5f)]
    float cameraVertical = 1f;


    PlayerMouvementController charController;
    PlayerCameraController camController;
    // Start is called before the first frame update
    void Awake()
    {
        this.transform.gameObject.AddComponent<CapsuleCollider>();
        this.transform.gameObject.AddComponent<CharacterController>();
        this.transform.gameObject.AddComponent<PlayerMouvementController>();
        this.transform.gameObject.AddComponent<PlayerCameraController>();
        charController = this.transform.GetComponent<PlayerMouvementController>();
        camController = this.transform.GetComponent<PlayerCameraController>();
        charController.MovementSpeed = playerSpeed;
        camController.horizontalSpeed = cameraHorizontal;
        camController.verticalSpeed = cameraVertical;


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
