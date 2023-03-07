using UnityEngine;

public class BulletController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody2D>().velocity = transform.right * 25f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GetComponent<Renderer>().isVisible)
            Destroy(gameObject);
    }




    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
