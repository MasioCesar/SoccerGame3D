using UnityEngine;

public class StadiumCamera : MonoBehaviour
{
    [SerializeField] private Transform ball;

    void Update()
    {
        float z = ball.position.z + 25;
        if (z > 29)
        {
            z = 29;
        }

        // Mantém a câmera a 10 unidades acima do solo
        transform.position = new Vector3(ball.position.x, ball.position.y, z);
    }
}
