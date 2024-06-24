using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class Platforms : MonoBehaviour
{
    private const float MAX_HILL_HEIGHT = Globals.Platform.MAX_HILL_HEIGHT;
    private const float SEGMENT_SIZE = Globals.Platform.WIDTH / Globals.Platform.SEGMENT_COUNT;
    private float mWorldLeft;
    private Vector2 mNextStart;
    private float mSlope = 0.0f;
    private int mPlatformCount;
    private Queue<GameObject> mPlatformQueue;
    private LevelManager mLevelManager;

    private void GeneratePlatform()
    {
        GameObject platformNext = Instantiate(mLevelManager.PlatformPrefab, transform);
        platformNext.transform.position = mNextStart;
        SpriteShapeController spriteShapeController = platformNext.GetComponent<SpriteShapeController>();
        Spline spline = spriteShapeController.spline;
        Vector2 currentPoint = Vector2.zero;

        spline.Clear();

        for(int j = 0; j < Globals.Platform.SEGMENT_COUNT; ++j)
        {
            Vector2 slopeVectorStart = new Vector2(SEGMENT_SIZE * MathF.Cos(MathF.Atan(mSlope)), SEGMENT_SIZE * MathF.Sin(MathF.Atan(mSlope))) * 0.5f;
            spline.InsertPointAt(2 * j, currentPoint);
            spline.SetTangentMode(2 * j, ShapeTangentMode.Continuous);
            spline.SetRightTangent(2 * j, slopeVectorStart);

            float newY = -MAX_HILL_HEIGHT + 2.0f * (float)new System.Random().NextDouble() * MAX_HILL_HEIGHT;
            Vector2 newPoint = new Vector2(currentPoint.x + SEGMENT_SIZE, newY);
            mSlope = newY / SEGMENT_SIZE;
            Vector2 slopeVectorEnd = -new Vector2(SEGMENT_SIZE * MathF.Cos(MathF.Atan(mSlope)), SEGMENT_SIZE * MathF.Sin(MathF.Atan(mSlope))) * 0.5f;

            spline.InsertPointAt(2 * j + 1, newPoint);
            spline.SetTangentMode(2 * j + 1, ShapeTangentMode.Continuous);
            spline.SetLeftTangent(2 * j + 1, slopeVectorEnd);

            currentPoint = newPoint;
        }

        mNextStart += currentPoint;

        mPlatformQueue.Enqueue(platformNext);
    }

    // Start is called before the first frame update
    void Start()
    {
        mLevelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        mPlatformQueue = new Queue<GameObject>();
        int pixelWidth = Camera.main.pixelWidth;
        mWorldLeft = -Camera.main.ScreenToWorldPoint(new Vector3(pixelWidth, 0, 0)).x;
        GameObject platformStart = Instantiate(mLevelManager.PlatformPrefab, transform);
        platformStart.transform.localPosition = new Vector2(mWorldLeft, 0);

        {
            SpriteShapeController spriteShapeController = platformStart.GetComponent<SpriteShapeController>();
            Spline spline = spriteShapeController.spline;
            Vector2 startEndPoint = new Vector2(Globals.Platform.WIDTH, 0.0f);

            spline.Clear();

            spline.InsertPointAt(0, Vector2.zero);
            spline.SetTangentMode(0, ShapeTangentMode.Continuous);
            spline.InsertPointAt(1, startEndPoint);
            spline.SetTangentMode(1, ShapeTangentMode.Continuous);
            mPlatformQueue.Enqueue(platformStart);
        }

        int platformRemains = (int)(-2.0f * mWorldLeft / Globals.Platform.WIDTH) + 1;
        mPlatformCount = platformRemains + 2;
        mNextStart = platformStart.transform.localPosition + new Vector3(Globals.Platform.WIDTH, 0.0f, 0.0f);

        for(int i = 0; i < platformRemains; ++i)
        {
            GeneratePlatform();
        }

        LevelManager.SceneReady = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LateUpdate()
    {
        if(LevelManager.PlatformQueueReady)
        {
            while(mPlatformQueue.Count > 0)
            {
                float distance = Camera.main.transform.position.x + mWorldLeft - mPlatformQueue.Peek().transform.position.x;

                if(distance < Globals.Platform.WIDTH)
                {
                    break;
                }

                Destroy(mPlatformQueue.Dequeue());
            }

            while(mPlatformQueue.Count < mPlatformCount)
            {
                GeneratePlatform();
            }
        }
    }
}
