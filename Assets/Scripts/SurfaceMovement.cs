using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class SurfaceMovement : Movement
{
    private Rigidbody rb;
    private RigidbodyConstraints baseConstraints;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        baseConstraints = rb.constraints;
    }

    private void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 movementVector = new Vector3(moveHorizontal, 0f, moveVertical);
        transform.position += movementVector.normalized * speed * Time.deltaTime;
    }

    public override void SetActive(bool value)
    {
        enabled = value;
        if (value)
        {
            rb.constraints = baseConstraints;
        }
        else
        {
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }
    }
}