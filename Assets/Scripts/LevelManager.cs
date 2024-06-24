using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject PlatformPrefab;
    private static bool sSceneReady = false;
    private static bool sPlatformQueueReady = false;

    public static bool SceneReady
    {
        get
        {
            return sSceneReady;
        }

        set
        {
            sSceneReady = value;
        }
    }

    public static bool PlatformQueueReady
    {
        get
        {
            return sPlatformQueueReady;
        }

        set
        {
            sPlatformQueueReady = value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
