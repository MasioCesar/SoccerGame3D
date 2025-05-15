using UnityEngine;

public class Ball : MonoBehaviour
{
    private Transform playerBallPosition;
    private bool stickToPlayer;
    private Player scriptPlayer;
    private Vector3 previousLocation;
    private float speed;
    private Rigidbody rb;

    public bool StickToPlayer { get => stickToPlayer; set => stickToPlayer = value; }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (!stickToPlayer)
        {
            Player[] allPlayers = FindObjectsOfType<Player>();
            foreach (Player player in allPlayers)
            {
                float distance = Vector3.Distance(player.transform.position, transform.position);
                if (distance < 0.5f)
                {
                    AttachToPlayer(player);
                    break;
                }
            }
        }
        else
        {
            UpdateBallPosition();
        }

        CheckOutOfBounds();
    }

    private void AttachToPlayer(Player player)
    {
        stickToPlayer = true;
        scriptPlayer = player;
        scriptPlayer.BallAttachedToPlayer = this;
        playerBallPosition = player.transform.Find("Geometry/BallLocation");
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    private void UpdateBallPosition()
    {
        Vector2 currentLocation = new Vector2(transform.position.x, transform.position.z);
        speed = Vector2.Distance(currentLocation, previousLocation) / Time.deltaTime;
        transform.position = playerBallPosition.position;
        transform.Rotate(new Vector3(scriptPlayer.transform.right.x, 0, scriptPlayer.transform.right.z), speed, Space.World);
        previousLocation = currentLocation;
    }

    private void CheckOutOfBounds()
    {
        if (transform.position.y < 1.6)
        {
            ResetBallPosition();
        }
    }

    private void ResetBallPosition()
    {
        transform.position = new Vector3(Random.value * 60.9f + 39.55f, 1.87f, Random.value * 36.35f - 100.85f);
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        
        if (stickToPlayer)
        {
            stickToPlayer = false;
            scriptPlayer.BallAttachedToPlayer = null;
        }
    }
}