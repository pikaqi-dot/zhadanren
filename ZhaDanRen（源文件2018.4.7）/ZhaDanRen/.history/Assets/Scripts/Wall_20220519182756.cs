
using UnityEngine;

public class Wall : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Tags.BombEffect))
        {
            ObjectPool.Instance.Add(ObjectType.Wall, gameObject);
        }
    }
}
