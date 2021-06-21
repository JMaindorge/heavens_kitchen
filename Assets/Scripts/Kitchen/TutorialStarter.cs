using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TutorialStarter : MonoBehaviour
{
    public Tutorial tutorial;
    public TextMeshPro text;
    private bool hasStarted;
    
    public void ActivateTutorial()
    {
        if (hasStarted)
        {
            hasStarted = false;
            text.text = "Start Tutorial";
            tutorial.SetTutorial(false);
        }
        else
        {
            hasStarted = true;
            text.text = "End Tutorial";
            tutorial.SetTutorial(true);
        }
    }

    public bool TutorialActive()
    {
        return hasStarted;
    }
}
