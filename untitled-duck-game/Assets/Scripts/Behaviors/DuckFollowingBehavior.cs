using UnityEngine;
using System.Linq;


public class DuckFollowingBehavior: MonoBehaviour
{
    public LayerMask duckLayer;
    public Transform target;

    public bool isPathBlockedByDucks() {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, target.position - transform.position, Vector2.Distance(transform.position, target.position), duckLayer);

        hits = hits.Where(hit => hit.collider.gameObject != gameObject).ToArray();

        return hits.Length > 0;
    }

    public DuckBrain getClosestBlockingDuck() {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, target.position - transform.position, Vector2.Distance(transform.position, target.position), duckLayer);

        hits = hits.Where(hit => hit.collider.gameObject != gameObject).ToArray();
        hits = hits.Where(hit => hit.collider.gameObject.GetComponent<DuckBrain>() != null).ToArray();

        return hits[0].collider.gameObject.GetComponent<DuckBrain>();
    }
}