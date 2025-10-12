using UnityEngine;

public class BaseAbilityPlayer : BaseAbility
{
    protected Player player;
    protected PlayerPhysicsControl linkedPhysics;
    protected GetherInput linkedInput;

    [SerializeField] protected AudioSource audioSours;
    [SerializeField] protected AudioClip audioClip;

    public PlayerStates.State abilityID;

    private void Awake()
    {
        thisAbilityState = (int)abilityID;
    }

    protected override void Initialization()
    {
        base.Initialization();

        player = GetComponent<Player>();

        linkedPhysics = player.physicsControl;

        if (player != null)
            linkedInput = player.gatherInput;
     }
}
