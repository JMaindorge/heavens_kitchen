using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public MouseLook mouseReference;
    public Canvas menuReference;
    public Canvas optionsReference;
    public Canvas helpReference;
    public Canvas uiReference;
    private bool paused = false;

    // When the player updates the sensitivity, add that new value to the mouse look script.
    public void UpdateSensitivity(float newValue)
    {
        mouseReference.SetSensitivity(newValue);
    }

    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Check if the player presses the pause button.
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !paused)
        {
            PauseGame();
        }
    }

    // Resume the game. Better that this is only called from the play button as otherwise it might cause issues with the escape key / cursor lock.
    public void ResumeGame()
    {
        paused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
        menuReference.gameObject.SetActive(false);
        uiReference.gameObject.SetActive(true);
    }

    // Pause the game.
    public void PauseGame()
    {
        paused = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
        menuReference.gameObject.SetActive(true);
        uiReference.gameObject.SetActive(false);
    }

    // Load the options menu.
    public void ShowOptions()
    {
        menuReference.gameObject.SetActive(false);
        helpReference.gameObject.SetActive(false);
        optionsReference.gameObject.SetActive(true);
    }

    // Load the main menu.
    public void ShowMainMenu()
    {
        optionsReference.gameObject.SetActive(false);
        helpReference.gameObject.SetActive(false);
        menuReference.gameObject.SetActive(true);
    }

    // Show the 'how to play' menu / help menu.
    public void ShowHelp()
    {
        optionsReference.gameObject.SetActive(false);
        menuReference.gameObject.SetActive(false);
        helpReference.gameObject.SetActive(true);
    }
    
    // Quit the game.
    public void QuitGame()
    {
        Application.Quit();
    }
}
