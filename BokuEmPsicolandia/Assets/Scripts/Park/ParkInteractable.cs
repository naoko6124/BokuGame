using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ParkInteractable : MonoBehaviour
{
    public string levelName;
    public bool isPlayerOn = false;

    private ParkActions actions;

    private void Start()
    {
        actions = new ParkActions();
        actions.Enable();

        actions.Boku.Interact.performed += OnInteraction;
    }

    private void OnDestroy()
    {
        actions.Disable();
    }

    private void OnInteraction(InputAction.CallbackContext context)
    {
        if (isPlayerOn)
            SceneManager.LoadScene(levelName);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerOn = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerOn = false;
        }
    }
}
