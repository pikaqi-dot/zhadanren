using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PropType
{
    HP,
    Speed,
    Bomb,
    Range,
    Time
}
[System.Serializable]
public class PropType_Sprite
{
    public PropType type;
    public Sprite sp;
}
public class Prop : MonoBehaviour
{
    public PropType_Sprite[] propType_Sprites;
    private Sprite defultSp;
    private SpriteRenderer spriteRenderer;
    private PropType propType;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        defultSp = spriteRenderer.sprite;
    }
    /// <summary>
    /// 重置
    /// </summary>
    private void ResetProp()
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
            GetComponent<Collider2D>().isTrigger = true;
            int index = Random.Range(0, propType_Sprites.Length);
            spriteRenderer.sprite = propType_Sprites[index].sp;
            propType = propType_Sprites[index].type;

            StartCoroutine(PropAni());
        }
        //碰到人物，根据道具类型，增加效果
        if (collision.CompareTag(Tags.Player))
        {
            PlayerCtrl playerCtrl = collision.gameObject.GetComponent<PlayerCtrl>();

            switch (propType)
            {
                case PropType.HP:
                    playerCtrl.HP++;
                    break;
                case PropType.Speed:
                    playerCtrl.AddSpeed();
                    break;
                case PropType.Bomb:
                    playerCtrl.bombCount++;
                    break;
                case PropType.Range:
                    playerCtrl.range++;
                    break;
                case PropType.Time:
                    GameController.Instance.time += 50;
                    break;
                default:
                    break;
            }
            ResetProp();
            ObjectPool.Instance.Add(ObjectType.Prop, gameObject);
        }
    }
    IEnumerator PropAni()
    {
        for (int i = 0; i < 2; i++)
        {
            spriteRenderer.color = Color.yellow;
            yield return new WaitForSeconds(0.25f);
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(0.25f);
        }
    }
}
