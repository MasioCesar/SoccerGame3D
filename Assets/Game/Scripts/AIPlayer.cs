using UnityEngine;
using StarterAssets;

public class AIPlayerLinha1 : MonoBehaviour
{
    [SerializeField] private Transform ballTransform;
    [SerializeField] private Transform ownGoal;
    [SerializeField] private float moveSpeed = 2.5f;
    [SerializeField] private float minZ = -98f;
    [SerializeField] private float maxZ = -82.5f;
    [SerializeField] private float xApproachDistance = 12f;

    private StarterAssetsInputs inputs;
    private Player playerScript;
    private Animator animator;
    private int moveDirection = 1;

    private void Start()
    {
        inputs = GetComponent<StarterAssetsInputs>();
        playerScript = GetComponent<Player>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!playerScript.BallAttachedToPlayer)
        {
            MoveVertically();
            ApproachBallOnXAxis();
        }
        else
        {
            Vector3 goalPosition = new Vector3(ownGoal.position.x, transform.position.y, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, goalPosition, moveSpeed * Time.deltaTime);
            animator.SetFloat("Speed", 1f);

            if (Vector3.Distance(transform.position, goalPosition) < 1.5f)
                inputs.shoot = true;
        }
    }

    private void MoveVertically()
    {
        float newZ = transform.position.z + moveDirection * moveSpeed * Time.deltaTime;

        if (newZ > maxZ)
        {
            newZ = maxZ;
            moveDirection = -1;
        }
        else if (newZ < minZ)
        {
            newZ = minZ;
            moveDirection = 1;
        }

        transform.position = new Vector3(transform.position.x, transform.position.y, newZ);
        animator.SetFloat("Speed", 1f);
    }

    private void ApproachBallOnXAxis()
    {
        if (Mathf.Abs(ballTransform.position.z - transform.position.z) < xApproachDistance)
        {
            float newX = Mathf.Lerp(transform.position.x, ballTransform.position.x, 0.02f);
            transform.position = new Vector3(newX, transform.position.y, transform.position.z);
        }
    }
}
