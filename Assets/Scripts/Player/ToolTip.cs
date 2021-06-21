using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ToolTip : MonoBehaviour
{
    private TextMeshProUGUI text;

    private void Start() {
        text = GetComponent<TextMeshProUGUI>();
    }

    public void SetText(string replace) 
    {
        // Set the text to something else, such as "Drop Food".
        text.text = replace.ToString();
    }

    public void ShowToolTip() 
    {
        // Show the current object.
        this.gameObject.SetActive(true);
    }

    public void HideToolTip() 
    {
        // Hide the current object.
        this.gameObject.SetActive(false);
    }
}
