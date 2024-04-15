using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RamboDuckBehaviour : MonoBehaviour, IInteractor
{
    public int Hp = 3;
    bool invulnerable = false;

    public void OnInteract(Collider2D collider) {
        if(collider.gameObject.tag == "Enemy" && !invulnerable) {
            this.Hp--;
            this.invulnerable = true;
            StartCoroutine(DestroyEnemyOnNextFrame(collider.gameObject));

            if(this.Hp == 0) {
                StartCoroutine(DestroyOnNextFrame());
            } else {
                StartCoroutine(EndInvulnerability());
            }
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

    private IEnumerator EndInvulnerability() {
        yield return new WaitForSeconds(0.1f);
        this.invulnerable = false;
    }
}
