using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private float speed = 0.04f;
    private Rigidbody2D rig;
    private SpriteRenderer spriteRenderer;
    private Color color;
    /// <summary>
    /// 方向：0上 1下 2左 3右
    /// </summary>
    private int dirId = 0;
    private Vector2 dirVector;
    private float rayDistance = 0.7f;
    /// <summary>
    /// 是否可以移动
    /// </summary>
    private bool canMove = true;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        color = spriteRenderer.color;
        rig = GetComponent<Rigidbody2D>();
    }
    /// <summary>
    /// 初始化方法
    /// </summary>
    public void Init()
    {
        color.a = 1;
        spriteRenderer.color = color;
        canMove = true;
        InitDir(Random.Range(0, 4));
    }
    private void InitDir(int dir)
    {
        dirId = dir;
        switch (dirId)
        {
            case 0:
                dirVector = Vector2.up;
                break;
            case 1:
                dirVector = Vector2.down;
                break;
            case 2:
                dirVector = Vector2.left;
                break;
            case 3:
                dirVector = Vector2.right;
                break;
            default:
                break;
        }
    }
    private void Update()
    {
        if (canMove)
            rig.MovePosition((Vector2)transform.position + dirVector * speed);
        else
        {
            ChangeDir();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //消灭敌人
        if (collision.CompareTag(Tags.BombEffect) && gameObject.activeSelf)
        {
            GameController.Instance.enemyCount--;
            ObjectPool.Instance.Add(ObjectType.Enemy, gameObject);
        }
        if (collision.CompareTag(Tags.Enemy))
        {
            color.a = 0.3f;
            spriteRenderer.color = color;
        }
        if (canMove)
            if (collision.CompareTag(Tags.SuperWall) || collision.CompareTag(Tags.Wall))
            {
                //复位
                transform.position = new Vector2(Mathf.RoundToInt(transform.position.x),
                    Mathf.RoundToInt(transform.position.y));
                ChangeDir();
            }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag(Tags.Enemy))
        {
            color.a = 0.3f;
            spriteRenderer.color = color;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(Tags.Enemy))
        {
            color.a = 1;
            spriteRenderer.color = color;
        }
    }
    private void ChangeDir()
    {
        List<int> dirList = new List<int>();
        if (Physics2D.Raycast(transform.position, Vector2.up, rayDistance, 1 << 8) == false)
        {
            dirList.Add(0);
        }
        if (Physics2D.Raycast(transform.position, Vector2.down, rayDistance, 1 << 8) == false)
        {
            dirList.Add(1);
        }
        if (Physics2D.Raycast(transform.position, Vector2.left, rayDistance, 1 << 8) == false)
        {
            dirList.Add(2);
        }
        if (Physics2D.Raycast(transform.position, Vector2.right, rayDistance, 1 << 8) == false)
        {
            dirList.Add(3);
        }
        if (dirList.Count > 0)
        {
            canMove = true;
            int index = Random.Range(0, dirList.Count);
            InitDir(dirList[index]);
        }
        else canMove = false;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(0, rayDistance, 0));
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(0, -rayDistance, 0));
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(-rayDistance, 0, 0));
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(rayDistance, 0, 0));
    }
}
