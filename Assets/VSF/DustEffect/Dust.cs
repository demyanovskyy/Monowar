using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dust : MonoBehaviour
{
    //[SerializeField]
   // GameObject dustCloud;

    [SerializeField] private ParticleSystem particleSystem1;
    [SerializeField] private int emitAmount1;

    public LayerMask whatIsGround;
    bool isGrounded=false;
    [SerializeField]
    float groundCheckRadius = 0.2f;

    private void OnTriggerEnter2D(Collider2D col)
    {
        //Debug.Log("Dust:" + col.gameObject.name);
        isGrounded = Physics2D.OverlapCircle(transform.position, groundCheckRadius, whatIsGround);
        if (col != null && isGrounded)
        {
            //Debug.Log("Dust:" + col.gameObject.name);
            //Instantiate(dustCloud, transform.position, dustCloud.transform.rotation);
            //GameObject dust = ObjectPooler.instance.GetPooledObject(dustCloud+"(Clone)");
            //dust.transform.position = this.transform.position;
            //dust.SetActive(true);
            //AudioManager.instance.PlaySFX(12);
            EmitParticles1();
        }
    }

    public void EmitParticles1()
    {
        particleSystem1.Emit(emitAmount1);
    }
}
