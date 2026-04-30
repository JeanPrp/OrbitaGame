using UnityEngine;

public class PlanetBodyCollision : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("Player")) return;

        Debug.Log("GAME OVER - bateu no planeta");

        if (GameManager.Instance != null)
        {
            GameManager.Instance.GameOver();
        }
    }
}