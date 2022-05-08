using System.Collections;
using UnityEngine;

public class Hexagon : MonoBehaviour
{
    public Rigidbody2D rb;

    public float shrinkSpeed = 3f;

    private void Start()
    {
        rb.rotation = Random.Range(0f, 360f);
        transform.localScale = Vector3.one * 10f;
    }

    private void Update()
    {
        transform.localScale -= Vector3.one * (shrinkSpeed * Time.deltaTime);
        
        if(transform.localScale.x <= .05f)
            Destroy(gameObject);
    }
}