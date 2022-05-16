using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectType
{
    SuperWall,
    Wall,
    Prop,
    Bomb,
    Enemy,
    BombEffect
}

[System.Serializable]
public class Type_Prefab
{
    public ObjectType type;
    public GameObject prefab;
}
public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;
    public List<Type_Prefab> type_Prefabs = new List<Type_Prefab>();
    /// <summary>
    /// 通过物体类型获取该预制体
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    private GameObject GetPreByType(ObjectType type)
    {
        foreach (var item in type_Prefabs)
        {
            if (item.type == type)
                return item.prefab;
        }
        return null;
    }

    /// <summary>
    /// 物体类型和对应的对象池关系字典
    /// </summary>
    private Dictionary<ObjectType, List<GameObject>> dic =
        new Dictionary<ObjectType, List<GameObject>>();


    private void Awake()
    {
        Instance = this;
    }
    /// <summary>
    /// 通过物体类型从相对应的对象池中取东西
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public GameObject Get(ObjectType type, Vector2 pos)
    {
        GameObject temp = null;
        //判断字典中有没有与该类型匹配的对象池，没有则创建
        if (dic.ContainsKey(type) == false)
            dic.Add(type, new List<GameObject>());
        //判断该类型对象池中有没有物体
        if (dic[type].Count > 0)
        {
            int index = dic[type].Count - 1;
            temp = dic[type][index];
            dic[type].RemoveAt(index);
        }
        else
        {
            GameObject pre = GetPreByType(type);
            if (pre != null)
            {
                temp = Instantiate(pre, transform);
            }
        }
        temp.SetActive(true);
        temp.transform.position = pos;
        temp.transform.rotation = Quaternion.identity;
        return temp;
    }
    /// <summary>
    /// 回收
    /// </summary>
    /// <param name="type"></param>
    public void Add(ObjectType type, GameObject go)
    {
        //判断该类型是否有对应的对象池以及对象池中不存在该物体
        if (dic.ContainsKey(type) && dic[type].Contains(go) == false)
        {
            //放入对象池
            dic[type].Add(go);
        }
        go.SetActive(false);
    }
}
