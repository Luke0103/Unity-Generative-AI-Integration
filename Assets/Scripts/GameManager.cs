using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Dictionary<string, GameManager> gameObjectDict;

    private void Awake()
    {
        //WebGLInput.captureAllKeyboardInput = true;
    }
}
