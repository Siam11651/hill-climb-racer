using UnityEngine;

public class Wheel : MonoBehaviour
{
    private const float MAX_DRAG_SPEED = 0.25f;
    private const float DIRT_SPAWN_INTERVAL = 0.1f;
    private LevelManager mLevelManager;
    private Rigidbody2D mRb;

    // Start is called before the first frame update
    void Start()
    {
        mLevelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        mRb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            ContactPoint2D[] contacts = collision.contacts;

            foreach(ContactPoint2D contact in contacts)
            {
                if(contact.relativeVelocity.magnitude > MAX_DRAG_SPEED)
                {
                    float elapsed = Time.time - mLevelManager.LastDirtTime;

                    if(elapsed >= DIRT_SPAWN_INTERVAL)
                    {
                        Vector3 point = contact.point;

                        if(mRb.angularVelocity > 0.0f)
                        {
                            point.z = -1.0f;
                        }
                        else
                        {
                            point.z = 0.0f;
                        }

                        Instantiate(mLevelManager.DirtPrefab, point, Quaternion.identity);

                        mLevelManager.LastDirtTime = Time.time;
                    }
                }
            }
        }
    }
}
