using UnityEngine;

public class Item : MonoBehaviour
{
    void OnCollisionEnter(Collision col){
        if(col.gameObject.TryGetComponent<Ship>(out Ship o)){
            Debug.Log("Item Collide");
            ObjectManager.Instance.GetItem();
        }
    }
}
