using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public GameObject doorPre;
    public int X, Y;
    private List<Vector2> nullPointsList = new List<Vector2>();
    private List<Vector2> superWallPointList = new List<Vector2>();
    private GameObject door;

    //表示从对象池中取出来的所有物体集合
    private Dictionary<ObjectType, List<GameObject>> poolObjectDic =
        new Dictionary<ObjectType, List<GameObject>>();

    /// <summary>
    /// 判断当前位置是否是实体墙
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public bool IsSuperWall(Vector2 pos)
    {
        if (superWallPointList.Contains(pos))
            return true;
        return false;
    }
    public Vector2 GetPlayerPos()
    {
        return new Vector2(-(X + 1), Y - 1);
    }
    private void Recovery()
    {
        nullPointsList.Clear();
        superWallPointList.Clear();
        foreach (var item in poolObjectDic)
        {
            foreach (var obj in item.Value)
            {
                ObjectPool.Instance.Add(item.Key, obj);
            }
        }
        poolObjectDic.Clear();
    }
    public void InitMap(int x, int y, int wallCount, int enemyCount)
    {
        Recovery();

        X = x;
        Y = y;
        CreateSuperWall();
        FindNullPoints();
        CreateWall(wallCount);
        CreateDoor();
        CreateProps();
        CreateEnemy(enemyCount);
    }
    /// <summary>
    /// 生成实体墙
    /// </summary>
    private void CreateSuperWall()
    {
        for (int x = -X; x < X; x += 2)
        {
            for (int y = -Y; y < Y; y += 2)
            {
                SpawnSuperWall(new Vector2(x, y));
            }
        }

        for (int x = -(X + 2); x <= X; x++)
        {
            SpawnSuperWall(new Vector2(x, Y));
            SpawnSuperWall(new Vector2(x, -Y - 2));
        }
        for (int y = -(Y + 1); y <= Y - 1; y++)
        {
            SpawnSuperWall(new Vector2(-(X + 2), y));
            SpawnSuperWall(new Vector2(X, y));
        }
    }
    private void SpawnSuperWall(Vector2 pos)
    {
        superWallPointList.Add(pos);
        GameObject superWall = ObjectPool.Instance.Get(ObjectType.SuperWall, pos);
        if (poolObjectDic.ContainsKey(ObjectType.SuperWall) == false)
            poolObjectDic.Add(ObjectType.SuperWall, new List<GameObject>());
        poolObjectDic[ObjectType.SuperWall].Add(superWall);
    }

    /// <summary>
    /// 查找地图中所有的空点
    /// </summary>
    private void FindNullPoints()
    {
        for (int x = -(X + 1); x <= X - 1; x++)
        {
            if (-(X + 1) % 2 == x % 2)
                for (int y = -(Y + 1); y <= Y - 1; y++)
                {
                    nullPointsList.Add(new Vector2(x, y));
                }
            else
                for (int y = -(Y + 1); y <= Y - 1; y += 2)
                {
                    nullPointsList.Add(new Vector2(x, y));
                }
        }
        //8 3 
        nullPointsList.Remove(new Vector2(-(X + 1), Y - 1));
        nullPointsList.Remove(new Vector2(-X, Y - 1));
        nullPointsList.Remove(new Vector2(-(X + 1), Y - 2));
    }
    /// <summary>
    /// 创建可以销毁的墙
    /// </summary>
    private void CreateWall(int wallCount)
    {
        if (wallCount >= nullPointsList.Count)
            wallCount = (int)(nullPointsList.Count * 0.7f);

        for (int i = 0; i < wallCount; i++)
        {
            int index = Random.Range(0, nullPointsList.Count);
            GameObject wall = ObjectPool.Instance.Get(ObjectType.Wall, nullPointsList[index]);
            nullPointsList.RemoveAt(index);

            if (poolObjectDic.ContainsKey(ObjectType.Wall) == false)
                poolObjectDic.Add(ObjectType.Wall, new List<GameObject>());
            poolObjectDic[ObjectType.Wall].Add(wall);
        }
    }
    /// <summary>
    /// 创建门
    /// </summary>
    private void CreateDoor()
    {
        if (door == null)
            door = Instantiate(doorPre, transform);
        door.GetComponent<Door>().ResetDoor();
        int index = Random.Range(0, nullPointsList.Count);
        door.transform.position = nullPointsList[index];
        nullPointsList.RemoveAt(index);
    }
    private void CreateProps()
    {
        int count = Random.Range(0, 2 + (int)(nullPointsList.Count * 0.05f));
        for (int i = 0; i < count; i++)
        {
            int index = Random.Range(0, nullPointsList.Count);
            GameObject prop = ObjectPool.Instance.Get(ObjectType.Prop, nullPointsList[index]);
            nullPointsList.RemoveAt(index);

            if (poolObjectDic.ContainsKey(ObjectType.Prop) == false)
                poolObjectDic.Add(ObjectType.Prop, new List<GameObject>());
            poolObjectDic[ObjectType.Prop].Add(prop);
        }
    }
    private void CreateEnemy(int count)
    {
        for (int i = 0; i < count; i++)
        {
            int index = Random.Range(0, nullPointsList.Count);
            GameObject enemy = ObjectPool.Instance.Get(ObjectType.Enemy, nullPointsList[index]);
            enemy.GetComponent<EnemyAI>().Init();
            nullPointsList.RemoveAt(index);

            if (poolObjectDic.ContainsKey(ObjectType.Enemy) == false)
                poolObjectDic.Add(ObjectType.Enemy, new List<GameObject>());
            poolObjectDic[ObjectType.Enemy].Add(enemy);
        }
    }
}
