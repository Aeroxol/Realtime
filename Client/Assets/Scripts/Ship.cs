using System.Collections;
using System.Collections.Generic;
using Best.HTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509.Qualified;
using Unity.Mathematics;
using UnityEngine;

public class Ship : MonoBehaviour
{
    public Bullet bullet;
    public Rigidbody rb;
    public Transform body;
    public Transform gunPoint;

    public const float shootIntervel = 0.6f;
    // private float _shootInterval = 0;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        BackToScreen();
        // if (_shootInterval >= 0)
        //     _shootInterval -= Time.deltaTime;
        // if (Input.GetKey(KeyCode.Space))
        // {
        //     // Shoot();
        // }
        UpdateDirection();
    }

    private void UpdateDirection()
    {
        if(rb.velocity.magnitude == 0) return;
        body.forward = rb.velocity;
    }

    // private void Shoot()
    // {
    //     if (_shootInterval <= 0)
    //     {
    //         var go = Instantiate<Bullet>(bullet, gunPoint.position, body.rotation);
    //         go.rb.velocity = this.rb.velocity + 4 * this.rb.velocity.normalized;
    //         _shootInterval = shootIntervel;
    //     }
    // }

    void OnCollisionEnter(Collision col){
        if(col.gameObject.TryGetComponent<Asteroid>(out Asteroid a)){
            GameManager.Instance.BtnGameEnd();
        }
    }

    private void BackToScreen()
    {
        Vector3 vp = Camera.main.WorldToViewportPoint(transform.position);
        if (vp.x < 0)
        {
            transform.position = Camera.main.ViewportToWorldPoint(new Vector3(1, vp.y, vp.z));
        }
        else if (vp.x > 1)
        {
            transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0, vp.y, vp.z));
        }
        if (vp.y < 0)
        {
            transform.position = Camera.main.ViewportToWorldPoint(new Vector3(vp.x, 1, vp.z));
        }
        else if (vp.y > 1)
        {
            transform.position = Camera.main.ViewportToWorldPoint(new Vector3(vp.x, 0, vp.z));
        }
    }
}
