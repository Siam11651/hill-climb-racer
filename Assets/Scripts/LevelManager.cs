using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject PlatformPrefab;
    public GameObject DirtPrefab;
    private float mLastDirtTime = -1.0f;
    private static bool sSceneReady = false;
    private static bool sPlatformQueueReady = false;

    public float LastDirtTime
    {
        get
        {
            return mLastDirtTime;
        }

        set
        {
            mLastDirtTime = value;
        }
    }

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
