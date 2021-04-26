using UnityEngine;

public class BoatMovement : Movement
{
    private Vector3 border1;
    private Vector3 border2;

    private void Awake()
    {
        border1 = Vector3.zero;
        border2 = Vector3.zero;
    }

    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 movementVector = new Vector3(moveHorizontal, moveVertical, 0f);
        Vector3 newPosition = transform.position + movementVector.normalized * speed * Time.deltaTime;
        newPosition.x = Mathf.Clamp(newPosition.x, border1.x, border2.x);
        newPosition.y = Mathf.Clamp(newPosition.y, border1.y, border2.y);
        transform.position = newPosition;
    }

    public void SetBorders(Vector3 newBorder1, Vector3 newBorder2)
    {
        border1 = newBorder1;
        border2 = newBorder2;
    }

    public override void SetActive(bool value)
    {
        enabled = value;
    }
}