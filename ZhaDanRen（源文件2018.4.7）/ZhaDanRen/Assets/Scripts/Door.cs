using UnityEngine;

public class Door : MonoBehaviour
{
    public Sprite doorSprite;

    private Sprite defultSp;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        defultSp = spriteRenderer.sprite;
    }
    public void ResetDoor()
    {
        tag = "Wall";
        gameObject.layer = 8;
        spriteRenderer.sprite = defultSp;
        GetComponent<Collider2D>().isTrigger = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Tags.BombEffect))
        {
            tag = "Untagged";
            gameObject.layer = 0;
            spriteRenderer.sprite = doorSprite;
            GetComponent<Collider2D>().isTrigger = true;
        }
        if (collision.CompareTag(Tags.Player))
        {
            //判断当前场景中的敌人是否都消灭了
            GameController.Instance.LoadNextLevel();
        }
    }
}
