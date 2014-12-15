using UnityEngine;
using System.Collections;

public class CharacterCtrl : MonoBehaviour {
    public Camera camera = null;
    // These values should be experimented with
    public float speed = 100.0f;
    public float sprintSpeed = 200.0f;
    public Vector3 maxVelocity = new Vector3(350.0f, 280.0f, 350.0f);
    // Should this object rotate with the camera?
    //public bool rotateWithCamera = false;
    public float fallDamageThreshold = -80;
    public bool canSprint = true;

    private Vector3 velocity = Vector3.zero;
    private Vector3 gravity = Vector3.zero;
    private bool isGrounded = false;
    private bool inWater = false;
    private bool hasJumped = false;
    private Quaternion rotation;
    private bool isSprinting = false;
    private float speedStore;
    private Vector3 maxVelocityStore;
    private Vector2 platformPrevPos = Vector2.zero;

	// Use this for initialization
	void Start () {
	     gravity = new Vector3(0.0f,-40.0f,0.0f);
         // Store the default values
         speedStore = speed;
         maxVelocityStore = maxVelocity;
         print(platformPrevPos);
	}
	
	// Update is called once per frame
	void Update () {
        /*if (rotateWithCamera)
        {
            Transform[] transforms = GetComponentsInChildren<Transform>();
            foreach ( Transform t in transforms )
            {
                if (camera.transform == t) continue;
                if (transform == t) continue;
                t.rotation = new Quaternion(0, camera.transform.rotation.y, 0, camera.transform.rotation.w);
            }
            //transform.rotation.Set(transform.rotation.x,camera.transform.rotation.y,transform.rotation.z,transform.rotation.w);
        }*/

        // Movement
        if (canSprint && Input.GetAxis("Sprint") > 0)
        {
            isSprinting = true;
            speed = sprintSpeed;
            maxVelocity.x += sprintSpeed;
            maxVelocity.z += sprintSpeed;
        }
        else if(isSprinting)
        {
            isSprinting = false;
            speed = speedStore;
            maxVelocity = maxVelocityStore;
        }

        if (isGrounded || inWater)
        {
            Vector3 movVec = Vector3.zero;
            movVec += camera.transform.forward * Input.GetAxis("Vertical") * speed;
            movVec += camera.transform.right * Input.GetAxis("Horizontal") * speed;
            if (hasJumped)
                movVec.y = 0;

            velocity += movVec;
        }
        else if (!isGrounded)
        {
        }

        if (Input.GetAxis("Jump") == 1 && isGrounded && !hasJumped)
        {
            Jump();
        }
        else
        {
            // Apply gravity
            RaycastHit hit;
            Vector3 SurfaceNormal = Vector3.zero;
            if (Physics.Raycast(transform.position, Vector3.down, out hit) && !inWater)
            {
                //print(hit.distance);
                if (hit.distance < (collider.bounds.size.y / 2 + 1.0f))
                {
                    isGrounded = true;
                    hasJumped = false;
                    SurfaceNormal = hit.normal;
                    velocity.y = 0.0f; // Stop

                    // Check for platform
                    GameObject obj = hit.collider.gameObject;
                    Elevator elev = obj.GetComponent<Elevator>();
                    if (elev != null)
                    {
                        if ( platformPrevPos != Vector2.zero )
                             velocity += new Vector3(hit.point.x - platformPrevPos.x,0,hit.point.z - platformPrevPos.y);

                        platformPrevPos = new Vector2(hit.point.x, hit.point.z);
                    }

                    if ( velocity.y < fallDamageThreshold )
                        SendMessage("OnFallDamage", velocity);
                }
                else
                    isGrounded = false;
            }
            else
                isGrounded = false;
            if (isGrounded)
            {
                Vector3 v = (SurfaceNormal * gravity.magnitude) + gravity;

                //print("y = " + v.y);
                velocity += v * Time.deltaTime;

                rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);

                // Apply friction
                // I'm not convinced that this is the best way to do it, but it works
                velocity *= 0.40f;
            }
            else if (inWater)
            {
                // Apply water friction
                velocity *= 0.40f;
            }
            else
                velocity += gravity * Time.deltaTime;
        }

        // Max speed check
        if (velocity.x > maxVelocity.x) velocity.x = maxVelocity.x;
        else if (velocity.x < maxVelocity.x * -1) velocity.x = maxVelocity.x * -1;
        if (velocity.y > maxVelocity.y) velocity.y = maxVelocity.y;
        else if (velocity.y < maxVelocity.y * -1) velocity.y = maxVelocity.y * -1;
        if (velocity.z > maxVelocity.z) velocity.z = maxVelocity.z;
        else if (velocity.z < maxVelocity.z * -1) velocity.z = maxVelocity.z * -1;

        // Apply velocity
        Debug.DrawRay(transform.position, velocity);

        if(isGrounded)
            transform.Translate((rotation * velocity) * Time.deltaTime);
        else
            transform.Translate((velocity) * Time.deltaTime);
	}

    public void Jump()
    {
        isGrounded = false;
        hasJumped = true;
        velocity += new Vector3(0, maxVelocity.y, 0);
    }

    public void SetPosition(Vector3 pos)
    {
        transform.position = pos;
    }

    public void ApplyVelocity(Vector3 addVel)
    {
        velocity += addVel;
    }

    public Vector3 GetVelocity()
    {
        return velocity;
    }

    public bool IsGrounded()
    {
        return isGrounded;
    }

    public bool HasJumped()
    {
        return hasJumped;
    }

    public bool InWater()
    {
        return inWater;
    }
}
