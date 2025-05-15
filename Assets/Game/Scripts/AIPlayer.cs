using UnityEngine;
using StarterAssets;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(StarterAssetsInputs))]
public class AIPlayer : MonoBehaviour
{
    [SerializeField] private Transform ballTransform;
    [SerializeField] private Transform playerGoalTransform;
    [SerializeField] private float chaseSpeed = 2.5f;
    [SerializeField] private float shootDistance = 1.5f;

    private StarterAssetsInputs inputs;
    private Player playerScript;
    private Animator animator;

    private void Start()
    {
        inputs = GetComponent<StarterAssetsInputs>();
        playerScript = GetComponent<Player>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (ballTransform == null || playerScript == null)
            return;

        Vector3 targetPosition;

        if (!playerScript.BallAttachedToPlayer)
        {
            targetPosition = ballTransform.position;
            MoveTowards(targetPosition);
        }
        else
        {
            targetPosition = playerGoalTransform.position;

            float distanceToGoal = Vector3.Distance(transform.position, playerGoalTransform.position);
            MoveTowards(targetPosition);

            if (distanceToGoal < shootDistance)
            {
                inputs.shoot = true;
            }
        }
    }

    private void MoveTowards(Vector3 target)
    {
        Vector3 direction = target - transform.position;
        direction.y = 0;

        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }

        transform.position += direction.normalized * chaseSpeed * Time.deltaTime;
        animator.SetFloat("Speed", 1f);
    }
}
