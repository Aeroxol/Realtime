using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    public Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        OutOfScreen();
    }

    void OnCollisionEnter(Collision col){
        Destroy(gameObject);
    }

    private void OutOfScreen()
    {
        Vector3 vp = Camera.main.WorldToViewportPoint(transform.position);
        if (vp.x < 0 || vp.x > 1 || vp.y < 0 || vp.y > 1)
        {
            Destroy(gameObject);
        }
        
    }
}
