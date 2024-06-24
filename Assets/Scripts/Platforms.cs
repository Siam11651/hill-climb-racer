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
    private Transform mCarTransform;
    private LevelManager mLevelManager;

    private void GeneratePlatform()
    {
        mCarTransform = GameObject.Find("Car").transform;
        GameObject platformNext = Instantiate(mLevelManager.PlatformPrefab, transform);
        GameObject platformGround = platformNext.transform.GetChild(0).gameObject;
        platformNext.transform.position = mNextStart;
        SpriteShapeController platformSpriteShapeController = platformNext.GetComponent<SpriteShapeController>();
        Spline platformSpline = platformSpriteShapeController.spline;
        SpriteShapeController groundSpriteShapeController = platformGround.GetComponent<SpriteShapeController>();
        Spline groundSpline = groundSpriteShapeController.spline;
        Vector2 currentPoint = Vector2.zero;

        platformSpline.Clear();
        groundSpline.Clear();

        for(int j = 0; j < Globals.Platform.SEGMENT_COUNT; ++j)
        {
            Vector2 slopeVectorStart = new Vector2(SEGMENT_SIZE * MathF.Cos(MathF.Atan(mSlope)), SEGMENT_SIZE * MathF.Sin(MathF.Atan(mSlope))) * 0.5f;
            platformSpline.InsertPointAt(2 * j, currentPoint);
            platformSpline.SetTangentMode(2 * j, ShapeTangentMode.Continuous);
            platformSpline.SetRightTangent(2 * j, slopeVectorStart);
            groundSpline.InsertPointAt(2 * j, currentPoint);

            if(j == 0 || j == Globals.Platform.SEGMENT_COUNT - 1)
            {
                groundSpline.SetTangentMode(2 * j, ShapeTangentMode.Broken);
            }
            else
            {
                groundSpline.SetTangentMode(2 * j, ShapeTangentMode.Continuous);
            }

            groundSpline.SetRightTangent(2 * j, slopeVectorStart);

            float newY = -MAX_HILL_HEIGHT + 2.0f * (float)new System.Random().NextDouble() * MAX_HILL_HEIGHT;
            Vector2 newPoint = new Vector2(currentPoint.x + SEGMENT_SIZE, newY);
            mSlope = newY / SEGMENT_SIZE;
            Vector2 slopeVectorEnd = -new Vector2(SEGMENT_SIZE * MathF.Cos(MathF.Atan(mSlope)), SEGMENT_SIZE * MathF.Sin(MathF.Atan(mSlope))) * 0.5f;

            platformSpline.InsertPointAt(2 * j + 1, newPoint);
            platformSpline.SetTangentMode(2 * j + 1, ShapeTangentMode.Continuous);
            platformSpline.SetLeftTangent(2 * j + 1, slopeVectorEnd);
            groundSpline.InsertPointAt(2 * j + 1, newPoint);

            if(j == 0 || j == Globals.Platform.SEGMENT_COUNT - 1)
            {
                groundSpline.SetTangentMode(2 * j + 1, ShapeTangentMode.Broken);
            }
            else
            {
                groundSpline.SetTangentMode(2 * j + 1, ShapeTangentMode.Continuous);
            }

            groundSpline.SetLeftTangent(2 * j + 1, slopeVectorEnd);

            currentPoint = newPoint;
        }

        {
            int index = 2 * Globals.Platform.SEGMENT_COUNT;

            groundSpline.InsertPointAt(index, new Vector2(Globals.Platform.WIDTH, -3.0f * MAX_HILL_HEIGHT));
            groundSpline.SetTangentMode(index, ShapeTangentMode.Linear);
            groundSpline.InsertPointAt(index + 1, new Vector2(0.0f, -3.0f * MAX_HILL_HEIGHT));
            groundSpline.SetTangentMode(index + 1, ShapeTangentMode.Linear);
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
            GameObject platformGround = platformStart.transform.GetChild(0).gameObject;
            SpriteShapeController platformSpriteShapeController = platformStart.GetComponent<SpriteShapeController>();
            Spline platformSpline = platformSpriteShapeController.spline;
            SpriteShapeController groundSpriteShapeController = platformGround.GetComponent<SpriteShapeController>();
            Spline groundSpline = groundSpriteShapeController.spline;
            Vector2 startEndPoint = new Vector2(Globals.Platform.WIDTH, 0.0f);

            platformSpline.Clear();
            groundSpline.Clear();

            platformSpline.InsertPointAt(0, Vector2.zero);
            platformSpline.SetTangentMode(0, ShapeTangentMode.Continuous);
            platformSpline.InsertPointAt(1, startEndPoint);
            platformSpline.SetTangentMode(1, ShapeTangentMode.Continuous);
            groundSpline.InsertPointAt(0, Vector2.zero);
            groundSpline.SetTangentMode(0, ShapeTangentMode.Linear);
            groundSpline.InsertPointAt(1, startEndPoint);
            groundSpline.SetTangentMode(1, ShapeTangentMode.Linear);
            groundSpline.InsertPointAt(2, new Vector2(SEGMENT_SIZE, -3.0f * MAX_HILL_HEIGHT));
            groundSpline.SetTangentMode(2, ShapeTangentMode.Linear);
            groundSpline.InsertPointAt(3, new Vector2(0.0f, -3.0f * MAX_HILL_HEIGHT));
            groundSpline.SetTangentMode(3, ShapeTangentMode.Linear);
            mPlatformQueue.Enqueue(platformStart);
        }

        int platformRemains = (int)(-2.0f * mWorldLeft / Globals.Platform.WIDTH) + 1;
        mPlatformCount = platformRemains + 2;
        mNextStart = platformStart.transform.localPosition + new Vector3(Globals.Platform.WIDTH, 0.0f, 0.0f);

        for(int i = 0; i < platformRemains; ++i)
        {
            GeneratePlatform();
        }

        mCarTransform.position = platformStart.transform.position + new Vector3(1.5f, 1.0f, 0.0f);

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
