using UnityEngine;

public class Ladders : MonoBehaviour
{
    private LaddersAbility laddersAbility;

    [SerializeField] private Collider2D upCollider;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        laddersAbility = collision.GetComponent<LaddersAbility>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (laddersAbility != null)
        {
            if (laddersAbility.isParamited)
            {
                laddersAbility.canGoOnLadder = true;
                if (laddersAbility.GetClimbParamiter())
                    upCollider.enabled = false;
                else
                    upCollider.enabled = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (laddersAbility != null)
        {
            if (laddersAbility.isParamited)
            {
                laddersAbility.canGoOnLadder = false;
                if (laddersAbility.GetClimbParamiter())
                    upCollider.enabled = false;
                else
                    upCollider.enabled = true;
            }
        }
    }
}
