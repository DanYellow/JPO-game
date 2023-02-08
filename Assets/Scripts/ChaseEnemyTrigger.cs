using UnityEngine;

public class ChaseEnemyTrigger : MonoBehaviour
{
    public EnemyFlying[] enemyArray;
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player")) {
            foreach (EnemyFlying item in enemyArray)
            {
                item.isChasing = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.CompareTag("Player")) {
            foreach (EnemyFlying item in enemyArray)
            {
                item.isChasing = false;
            }
        }
    }
}
