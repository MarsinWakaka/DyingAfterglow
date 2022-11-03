using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DeveloperScript : MonoBehaviour
{
    public GameObject player;
    public List<StorageTiles> container;
    public List<GameObject> GUI;
    // Start is called before the first frame update
    void Start()
    {
        #region Ϊ�洢�ռ��GUI�ӿ��ṩ��ʱ���Ž�
        container[0] = (StorageTiles)Instantiate(Resources.Load("BackpackL3"));
        container[1] = (StorageTiles)Instantiate(Resources.Load("Bureau"));
        GUI[0].GetComponent<ContainerMap>().container = container[0];
        GUI[1].GetComponent<ContainerMap>().container = container[1];
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
