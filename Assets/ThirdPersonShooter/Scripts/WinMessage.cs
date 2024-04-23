using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinMessage : MonoBehaviour
{
    public GameObject winArea;
    // Start is called before the first frame update
    private void ActivateSelf(bool winCondition)
    {
        if (winCondition)
        {
            winArea.SetActive(true);

        }

    }
}
