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
        float minX = 39.55f;
        float maxX = 100.45f;
        float minZ = -100.85f;
        float maxZ = -64.5f;

        Vector3 pos = transform.position;

        if (pos.x < minX || pos.x > maxX || pos.z < minZ || pos.z > maxZ)
        {
            if (pos.x < minX || pos.x > maxX) // saiu pela linha de fundo (escanteio ou gol)
            {
                Debug.Log("Escanteio!");
                HandleCorner();
            }
            else if (pos.z < minZ || pos.z > maxZ) // saiu pelas laterais (lateral)
            {
                Debug.Log("Lateral!");
                HandleLateral();
            }
        }
    }


    private void HandleLateral()
    {
        float campoMinX = 39.55f;
        float campoMaxX = 100.45f;
        float campoMinZ = -100.85f;
        float campoMaxZ = -64.5f;

        float xLateral = Mathf.Clamp(transform.position.x, campoMinX + 5f, campoMaxX - 5f); // evitar escanteio
        float zReposicao = transform.position.z < campoMinZ ? campoMinZ + 0.2f : campoMaxZ - 0.2f;

        transform.position = new Vector3(xLateral, 1.87f, zReposicao);
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // Libera da posse de jogador
        if (stickToPlayer)
        {
            stickToPlayer = false;
            if (scriptPlayer != null)
            {
                scriptPlayer.BallAttachedToPlayer = null;
            }
        }

        Debug.Log("Reposição de lateral realizada.");
    }

    private void HandleCorner()
    {
        float campoMinX = 39.55f;
        float campoMaxX = 100.45f;
        float campoMinZ = -100.85f;
        float campoMaxZ = -64.5f;

        // Define o canto mais próximo com base na posição atual
        float xCorner = transform.position.x < (campoMinX + campoMaxX) / 2f ? campoMinX + 0.5f : campoMaxX - 0.5f;
        float zCorner = transform.position.z < (campoMinZ + campoMaxZ) / 2f ? campoMinZ + 0.5f : campoMaxZ - 0.5f;

        transform.position = new Vector3(xCorner, 1.87f, zCorner);
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        if (stickToPlayer)
        {
            stickToPlayer = false;
            if (scriptPlayer != null)
            {
                scriptPlayer.BallAttachedToPlayer = null;
            }
        }

        Debug.Log("Reposição de escanteio realizada.");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Goal"))
        {
            Debug.Log("GOOOOL!");
            ReturnToCenter();
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

    private void ReturnToCenter()
    {
        transform.position = new Vector3(70f, 1.87f, -82.73f);
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        if (stickToPlayer)
        {
            stickToPlayer = false;
            if (scriptPlayer != null)
            {
                scriptPlayer.BallAttachedToPlayer = null;
            }
        }
    }
}