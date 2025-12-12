using System.Collections.Generic;
using UnityEngine;


public class ObjectPooler : MonoBehaviour
{
    [SerializeField] private GameObject Prefab;
    [SerializeField] private int poolSize = 5;
    private List<GameObject> pool;


    void Start()
    {
        pool = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
            CreateObject();
    }


    GameObject CreateObject()
    {
        GameObject obj = Instantiate(Prefab, transform);
        obj.SetActive(false);
        pool.Add(obj);
        return obj;
    }


    public GameObject GetPooledObject()
    {
        foreach (var obj in pool)
            if (!obj.activeSelf)
                return obj;


        return CreateObject();
    }
}