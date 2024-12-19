using UnityEngine;

public class Item : MonoBehaviour
{
    void Update(){
        BackToScreen();
    }

    void OnCollisionEnter(Collision col){
        if(col.gameObject.TryGetComponent<Ship>(out Ship o)){
            ObjectManager.Instance.GetItem();
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
