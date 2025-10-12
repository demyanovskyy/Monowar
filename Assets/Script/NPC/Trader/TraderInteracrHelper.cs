using UnityEngine;

public class TraderInteractHelper : MonoBehaviour, IInteractable
{
    [SerializeField] private DialogueObject traderDialogue;

    public void Interact()
    {
        //Debug.Log("You interact:" + gameObject.name);
        ServiceLocator.Current.Get<DialogueManeger>().StartDialogue(traderDialogue);
    }
}
