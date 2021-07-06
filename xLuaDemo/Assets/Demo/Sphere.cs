using UnityEngine;

public class Sphere : MonoBehaviour
{
    [SerializeField] private VirtualInputMoveDirection virtualInputMoveDirection;

    float speed = 0.2f;
    bool canCollision;

    private void Start()
    {
        canCollision = true;
    }

    private void Update()
    {

    }

    private void FixedUpdate()
    {
        Vector2 direction = virtualInputMoveDirection.GetDirection();
        float lenPercent = virtualInputMoveDirection.GetLenPercent();
        //Debug.Log($"MoveDir:{direction}, MoveLenPercent:{lenPercent}");
        gameObject.transform.position += new Vector3(direction.x * speed, 0, direction.y * speed);
    }

    public void JumpClick ()
    {
        canCollision = true;

        Debug.Log("JumpClick");
        gameObject.transform.position += new Vector3(0, 10, 0);
    }

    // 碰撞开始
    void OnCollisionEnter(Collision collision)
    {
        if (!canCollision)
        {
            return;
        }

        Debug.Log("碰撞开始");
        canCollision = false;

        FloorCube sp = collision.gameObject.GetComponent<FloorCube>();
        Debug.Log("row: " + sp.row + "   column : " + sp.column);

        GameManager.Instance.ChangeCube(sp);
    }
 
     // 碰撞结束
    void OnCollisionExit(Collision collision)
    {
        if (!canCollision)
        {
            return;
        }

        
        Debug.Log("碰撞结束");
    }
}
