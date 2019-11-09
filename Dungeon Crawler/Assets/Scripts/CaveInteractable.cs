using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CaveInteractable : Interactable
{
    public override void InteractedWith()
    {

        SceneManager.LoadScene("Dungeon");
    }
}
