using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuDedicatedServer : MonoBehaviour
{
    private void Start()
    {
#if DEDICATED_SERVER
        Loader.Load(Loader.Scene.MainMenuScene);
#endif 
    }
}
