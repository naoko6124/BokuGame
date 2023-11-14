using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpookyMansionMovement : MonoBehaviour
{
    public float sensitivity;
    public float cameraEffectSpeed;
    public float cameraEffectScale;
    public float moveSpeed;
    public Transform playerCamera;
    private Rigidbody rb;
    private SpookyMansionActions actions;
    private Vector2 camInput = Vector2.zero;
    private float cameraEffect = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        actions = new SpookyMansionActions();
        actions.Enable();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        Vector2 camMove = actions.Boku.Camera.ReadValue<Vector2>();
        camInput.y += camMove.x * sensitivity;
        camInput.x -= camMove.y * sensitivity;
        camInput.x = Mathf.Clamp(camInput.x, -90f, 90f);
        playerCamera.rotation = Quaternion.Euler(camInput.x, camInput.y, 0);

        Vector2 moveInput = actions.Boku.Movement.ReadValue<Vector2>();
        Vector3 moveDirection = playerCamera.forward * moveInput.y + playerCamera.right * moveInput.x;
        cameraEffect += moveInput.y * cameraEffectSpeed;
        playerCamera.localPosition = new Vector3(0f, 0.5f + Mathf.Sin(cameraEffect) * cameraEffectScale, 0f);

        rb.AddForce(moveDirection.normalized * moveSpeed, ForceMode.Force);
    }
}
