using UnityEngine;

public class FallingBlockCollision : MonoBehaviour
{
  
    Rigidbody2D rigidBody;
    BoxCollider2D box;
    AudioSource audioSource;

    int groundLayer;


    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        box = GetComponent<BoxCollider2D>();
        audioSource = GetComponent<AudioSource>();

       
        groundLayer = LayerMask.NameToLayer("Ground");
    }

    public void Fall()
    {
        rigidBody.bodyType = RigidbodyType2D.Dynamic;
        rigidBody.gravityScale = 8f;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == groundLayer)
        {
 
            rigidBody.bodyType = RigidbodyType2D.Static;
            rigidBody.gravityScale = 0f;

            box.usedByComposite = true;
  
            this.GetComponent<BoxCollider2D>().enabled = false;

            audioSource.Play();
        }
    }

}
