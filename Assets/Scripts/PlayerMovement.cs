using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 4f;

    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        Vector2 dir = new Vector2(x, y).normalized;
        transform.Translate(dir * speed * Time.deltaTime);
    }
}
