using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

[System.Serializable]
public class PlayerMovement
{
    public delegate Coroutine startCorutine(IEnumerator routine);

    private const float rotationSpeed = 10;

    #region Settings
    [SerializeField]
    private FloatStat speed;
    [SerializeField, SuffixLabel("per Second", true)]
    private float speedIncrease;
    [SerializeField]
    private bool autoWalkRight;
    #endregion

    [SerializeField, FoldoutGroup("References")]
    private Joystick jostick;

    private Vector2 lastInputDirection = new Vector2();

    public void MovementUpdate(PlayerInputActions input)
    {
        lastInputDirection = input.Player.Move.ReadValue<Vector2>();
        lastInputDirection.Normalize();

        if (autoWalkRight) return;
        lastInputDirection += jostick.GetDirection();
    }

    public void FixedMovementUpdate(Transform transform, Rigidbody rb, Animator playerAnim)
    {
        speed += speedIncrease * Time.fixedDeltaTime;

        Move(transform, rb, playerAnim);

        if (lastInputDirection == new Vector2()) return;
        LookIntoDirection(new Vector3(lastInputDirection.x, 0, lastInputDirection.y), transform);
    }

    public void LookIntoDirection(Vector3 dir, Transform transform)
    {
        if (dir == new Vector3()) return;
        var rot = Quaternion.LookRotation(dir);

        transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * rotationSpeed);
    }

    private void Move(Transform transform, Rigidbody rb, Animator playerAnim)
    {
        if (autoWalkRight)
        {
            lastInputDirection.x = 1;
        }

        rb.velocity = new Vector3(lastInputDirection.x, 0, lastInputDirection.y) * speed;
        playerAnim.SetBool("walking", lastInputDirection != Vector2.zero || autoWalkRight);
    }
}
