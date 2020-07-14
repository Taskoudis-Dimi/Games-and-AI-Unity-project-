using System.Collections;
using UnityEngine;

public class Move : MonoBehaviour
{
    float timeCounter = 0;
    float speed;
    float width;
    float height;

    // Start is called before the first frame update
    void Start()
    {
        speed = 5;
        width = 4;
        height = 7;

    }

    // Update is called once per frame
    void Update()
    {
        timeCounter += Time.deltaTime * speed;
        float x = Mathf.Cos(timeCounter) * width;
        float y = Mathf.Sin(timeCounter) * height;
        float z = 10;
        transform.position = new Vector3(x, y, z);
        
    }
}
