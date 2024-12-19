using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public Rigidbody rb;
    public Transform body;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        BackToScreen();
    }

    void OnCollisionEnter(Collision col)
    {
        // Debug.Log("col");
    }

    private void BackToScreen()
    {
        Vector3 vp = Camera.main.WorldToViewportPoint(transform.position);
        if (vp.x < -0.5f)
        {
            transform.position = Camera.main.ViewportToWorldPoint(new Vector3(1, vp.y, vp.z));
        }
        else if (vp.x > 1.5f)
        {
            transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0, vp.y, vp.z));
        }
        if (vp.y < -0.5f)
        {
            transform.position = Camera.main.ViewportToWorldPoint(new Vector3(vp.x, 1, vp.z));
        }
        else if (vp.y > 1.5f)
        {
            transform.position = Camera.main.ViewportToWorldPoint(new Vector3(vp.x, 0, vp.z));
        }
    }

    public void Set(int level){
        var _x = -1 + 2 * Random.value;
        var _y = -1 + 2 * Random.value;
        rb.velocity = (2 + 0.5f * level) * new Vector3(_x, _y, 0);
        rb.angularVelocity = new Vector3(_x, _y, 0);
    }
}
