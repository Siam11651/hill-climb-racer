using UnityEngine;

public class Car : MonoBehaviour
{
    private const float MOTOR_SPEED = 1000.0f;
    private const float CAMERA_EASE = 0.5f;
    [SerializeField]
    private WheelJoint2D mRearWheelJoint;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        JointMotor2D jointMotor = new JointMotor2D
        {
            motorSpeed = -Input.GetAxis("Horizontal") * MOTOR_SPEED,
            maxMotorTorque = 10000
        };

        mRearWheelJoint.motor = jointMotor;
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
