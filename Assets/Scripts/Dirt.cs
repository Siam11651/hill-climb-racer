using UnityEngine;

public class Dirt : MonoBehaviour
{
    private const float INIT_VELOCITY = 3.0f;
    private const float LIFE_TIME = 0.5f;
    private Rigidbody2D mRb;
    private float mStartTime;

    // Start is called before the first frame update
    void Start()
    {
        mRb = GetComponent<Rigidbody2D>();
        float x = -(float)new System.Random().NextDouble() * 0.25f;

        if(transform.position.z < 0.0f)
        {
            x *= -1.0f;
            Vector3 pos = transform.position;
            transform.position = new Vector3(pos.x, pos.y, 0.0f);
        }

        float y = System.MathF.Sqrt(1.0f - x * x);
        mRb.velocity = new Vector2(x, y) * INIT_VELOCITY;
        mStartTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        float elapsed = Time.time - mStartTime;

        if(elapsed > LIFE_TIME)
        {
            Destroy(gameObject);
        }
    }
}
