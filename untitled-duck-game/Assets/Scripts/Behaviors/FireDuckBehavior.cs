using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FireDuckBehaviour : MonoBehaviour, IInteractor
{
    public int ExplosionRadius;
    public LayerMask EnemyLayer;

    public void OnInteract(Collider2D collider) {
        if(collider.gameObject.tag == "Enemy") {
            Collider2D[] hits = Physics2D.OverlapCircleAll(this.transform.position, this.ExplosionRadius, 1 << this.EnemyLayer);

            foreach(Collider2D hit in hits) {
                StartCoroutine(DestroyEnemyOnNextFrame(hit.gameObject));
            }

            StartCoroutine(DestroyOnNextFrame());
        }
    }

    private IEnumerator DestroyOnNextFrame() {
        yield return new WaitForEndOfFrame();
        Destroy(this.gameObject);
    }
    private IEnumerator DestroyEnemyOnNextFrame(GameObject enemy) {
        yield return new WaitForEndOfFrame();
        Destroy(enemy);
    }
}
