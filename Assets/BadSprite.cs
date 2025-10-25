using UnityEngine;

public class BadSprite : MonoBehaviour
{
    void Start()
    {
        this.tag = "BadSprite";
        if (GetComponent<Collider2D>() == null)
        {
            gameObject.AddComponent<BoxCollider2D>();
        }
    }
}