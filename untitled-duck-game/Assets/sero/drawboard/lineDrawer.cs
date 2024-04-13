using UnityEngine;

public class lineDrawer : MonoBehaviour
{
    public float height;
    public float width;

    private LineRenderer lineRenderer;

    private bool isButtonCurrentlyPressed = false;

    // Start is called before the first frame update
    void Start(){
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update(){
        if (!isButtonCurrentlyPressed && Input.GetAxis("Fire1") == 1){
            isButtonCurrentlyPressed = true;

            Vector3 castPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (!contains(new Vector2(castPoint.x, castPoint.y))) return;

            lineRenderer.positionCount++;
            lineRenderer.SetPosition(
                lineRenderer.positionCount-1,
                new Vector3(
                    castPoint.x, 
                    castPoint.y, 
                    transform.position.z
                )
            );
        } else if (Input.GetAxis("Fire1") == 0)  {
            isButtonCurrentlyPressed = false;
        }
    }
    void OnDrawGizmosSelected() {
        Gizmos.color = new Color(1, 1, 1);
        Vector3 a = new Vector3(
            transform.position.x - width / 2,
            transform.position.y - height / 2,
            transform.position.z
        );
        Vector3 b = new Vector3(
            transform.position.x + width / 2,
            transform.position.y - height / 2,
            transform.position.z
        );
        Vector3 c = new Vector3(
            transform.position.x + width / 2,
            transform.position.y + height / 2,
            transform.position.z
        );
        Vector3 d = new Vector3(
            transform.position.x - width / 2,
            transform.position.y + height / 2,
            transform.position.z
        );

        Gizmos.DrawLineList(new Vector3[8]{a, b, b, c, c, d, d, a});
    }

    private bool contains(Vector2 point){
        float xMin = transform.position.x - width / 2;
        float xMax = transform.position.x + width / 2;
        float yMin = transform.position.y - height / 2;
        float yMax = transform.position.y + height / 2;

        return point.x >= xMin && point.x <= xMax && point.y >= yMin && point.y <= yMax;
    }
}


