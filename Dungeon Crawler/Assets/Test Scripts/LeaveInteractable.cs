﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LeaveInteractable : Interactable
{
    public override void InteractedWith()
    {

        SceneManager.LoadScene("Overworld");
    }
}
