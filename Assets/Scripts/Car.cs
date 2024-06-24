using UnityEngine;

public class Car : MonoBehaviour
{
    private const float MOTOR_SPEED = 1000.0f;
    private const float CAMERA_EASE = 0.5f;
    [SerializeField]
    private WheelJoint2D mRearWheelJoint;
    public bool AccDown;
    public bool RevDown;

    public void Move(float input)
    {
        JointMotor2D jointMotor = new JointMotor2D
        {
            motorSpeed = -input * MOTOR_SPEED,
            maxMotorTorque = 10000
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

        if(LevelManager.SceneReady && distance > 0.1f)
        {    
            Vector2 direction = displacement.normalized;
            Camera.main.transform.position += (Vector3)direction * distance * distance * CAMERA_EASE * Time.deltaTime;
        }
        
        if(!LevelManager.PlatformQueueReady && distance < 1.0f)
        {
            LevelManager.PlatformQueueReady = true;
        }
    }
}
