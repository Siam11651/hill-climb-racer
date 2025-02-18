using UnityEngine;

public class Car : MonoBehaviour
{
    private const float MOTOR_SPEED = 1000.0f;
    private const float CAMERA_EASE = 0.75f;
    [SerializeField]
    private WheelJoint2D mRearWheelJoint;
    [SerializeField]
    private Rigidbody2D mCameraRb;
    private bool mAccDown = false;
    private bool mRevDown = false;

    public bool AccDown
    {
        get
        {
            return mAccDown;
        }

        set
        {
            mAccDown = value;
        }
    }

    public bool RevDown
    {
        get
        {
            return mRevDown;
        }

        set
        {
            mRevDown = value;
        }
    }

    public void Move(float input)
    {
        JointMotor2D jointMotor = new JointMotor2D
        {
            motorSpeed = -input * MOTOR_SPEED,
            maxMotorTorque = 100f
        };

        mRearWheelJoint.motor = jointMotor;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(AccDown && !RevDown)
        {
            Move(1.0f);
        }
        else if(!AccDown && RevDown)
        {
            Move(-1.0f);
        }
        else
        {
            Move(Input.GetAxis("Horizontal"));
        }
    }

    void LateUpdate()
    {
        Vector2 displacement = transform.position - Camera.main.transform.position;
        float distance = displacement.magnitude;

        if(LevelManager.SceneReady)
        {
            if(distance > 0.1f)
            {
                Vector2 direction = displacement.normalized;
                mCameraRb.linearVelocity = (Vector3)direction * distance * distance * CAMERA_EASE;
            }
            else
            {
                mCameraRb.linearVelocity = Vector2.zero;
            }
        }
        
        if(!LevelManager.PlatformQueueReady && distance < 1.0f)
        {
            LevelManager.PlatformQueueReady = true;
        }
    }
}
