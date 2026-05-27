using UnityEngine;

public class MovingObjects : MonoBehaviour
{

    public float speed = 2f;       
    public float topBound = 140f;    
    public float bottomBound = -140f; 

    void Update()
    {
        transform.Translate(Vector2.down * speed * Time.deltaTime);

        if (transform.position.y < bottomBound)
        {
            Vector3 newPos = transform.position;
            newPos.y = topBound;
            transform.position = newPos;
        }
    }
}
