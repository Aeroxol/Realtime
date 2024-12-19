using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    private static ObjectManager _instance;
    public static ObjectManager Instance
    {
        get { return _instance; }
    }

    public Asteroid asteroidPrefab;
    public Item itemPrefab;

    private List<Asteroid> asteroids = new List<Asteroid>();
    private Item currentItem;

    void Awake()
    {
        if (null == _instance)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SpawnAsteroid(int num, int level)
    {
        for (int i = 0; i < num; i++)
        {
            float _x = -0.5f + 2 * Random.value;
            Vector3 pos = new Vector3(_x, -1.5f, 0);
            pos = Camera.main.ViewportToWorldPoint(pos);
            pos.z = 0;
            var go = Instantiate<Asteroid>(asteroidPrefab, pos, Quaternion.identity);
            go.Set(level);
            asteroids.Add(go);
        }
    }

    public void SpawnItem(){
        float _x = Random.value;
        float _y = Random.value;
        Vector3 pos = new Vector3(_x, _y, 0);
        pos = Camera.main.ViewportToWorldPoint(pos);
        pos.z = 0;
        var go = Instantiate<Item>(itemPrefab, pos, Quaternion.identity);
        currentItem = go;
    }

    public void GetItem(){
        Destroy(currentItem.gameObject);
        NetworkManager.Instance.SendPacket(22, GameManager.Instance.assetManager.CurrentItemScore);
        SpawnItem();
    }

    public void Clear()
    {
        foreach (var obj in asteroids)
        {
            if (obj != null) Destroy(obj.gameObject);
        }
        asteroids.Clear();
        if(currentItem != null)
            Destroy(currentItem.gameObject);
        currentItem = null;
    }
}
