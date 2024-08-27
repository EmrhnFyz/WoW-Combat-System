using UnityEngine;
using UnityEngine.UI;

public class ScrollRectVelocityModifier : MonoBehaviour
{
    [SerializeField] private ScrollRect targetScrollRect;
    [SerializeField] private Vector2 velocity;
    [SerializeField] private bool repeatWithDelay;
    [SerializeField] private float repeatVelocityDelay = 0.1f;

    public void Forward()
    {
        ModifyVelocityForward();

        if (repeatWithDelay)
        {
            CancelInvoke();
            Invoke(nameof(ModifyVelocityForward), repeatVelocityDelay);
        }
    }

    public void Backward()
    {
        ModifyVelocityBackward();

        if (repeatWithDelay)
        {
            CancelInvoke();
            Invoke(nameof(ModifyVelocityBackward), repeatVelocityDelay);
        }
    }

    private void ModifyVelocityForward() => targetScrollRect.velocity = velocity;

    private void ModifyVelocityBackward() => targetScrollRect.velocity = -velocity;
}