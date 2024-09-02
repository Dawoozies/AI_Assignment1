using UnityHFSM;
using UnityEngine;
public class Healer : Entity
{
    protected override void Start()
    {
        base.Start();
        entityStateMachine.AddState(State.Dead,
            onEnter: state =>
            {
                navigationSystem.Disable(true);
                meshRenderer.material = deadMaterial;
                _respawnTime = 0f;
            },
            onLogic: state =>
            {
                if (_respawnTime < respawnTime)
                {
                    _respawnTime += Time.deltaTime;
                }
                else
                {
                    entityStateMachine.RequestStateChange(State.FindCollectable);
                }
            },
            onExit: state =>
            {
                navigationSystem.Disable(false);
                meshRenderer.material = aliveMaterial;
                health = maxHealth;
            }
            );
    }
}