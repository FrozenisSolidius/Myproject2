using UnityEngine;

public class GoodSprite : MonoBehaviour
{
    void Start()
    {
        this.tag = "GoodSprite"; 
        //do NOT mix up the tags
        if (GetComponent<Collider2D>() == null)
        {
            gameObject.AddComponent<BoxCollider2D>();
        }
    }
}