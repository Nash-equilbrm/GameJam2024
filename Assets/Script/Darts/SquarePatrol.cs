using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SquarePatrol : MonoBehaviour
{
    public float speed = 5f; // Tốc độ di chuyển
    public float cornerDelay = 1f; // Độ delay ở mỗi góc
    [SerializeField]
    private float edgeLength = 1;

    private void Start()
    {
        StartCoroutine(MoveInSquare());
    }

    IEnumerator MoveInSquare()
    {
        while (true)
        {
            // Di chuyển lên
            yield return Move(Vector2.up);

            // Độ delay ở góc
            yield return new WaitForSeconds(cornerDelay);

            // Di chuyển sang phải
            yield return Move(Vector2.right);

            // Độ delay ở góc
            yield return new WaitForSeconds(cornerDelay);

            // Di chuyển xuống
            yield return Move(Vector2.down);

            // Độ delay ở góc
            yield return new WaitForSeconds(cornerDelay);

            // Di chuyển sang trái
            yield return Move(Vector2.left);

            // Độ delay ở góc
            yield return new WaitForSeconds(cornerDelay);
        }
    }

    IEnumerator Move(Vector2 direction)
    {
        float elapsedTime = 0f;
        Vector2 startingPos = transform.position;
        Vector2 targetPos = startingPos + direction * edgeLength;

        while (elapsedTime < 1f)
        {
            transform.position = Vector2.Lerp(startingPos, targetPos, elapsedTime);
            elapsedTime += Time.deltaTime * speed;
            yield return null;
        }

        transform.position = targetPos;
    }

    private void OnDrawGizmos()
    {
        //Gizmos.color = Color.red;

        //// Lấy vị trí của GameObject
        //Vector3 position = transform.position;

        //// Vẽ hình vuông
        //Gizmos.DrawLine(new Vector3(position.x - sideLength / 2, position.y - sideLength / 2, position.z),
        //                new Vector3(position.x + sideLength / 2, position.y - sideLength / 2, position.z));

        //Gizmos.DrawLine(new Vector3(position.x + sideLength / 2, position.y - sideLength / 2, position.z),
        //                new Vector3(position.x + sideLength / 2, position.y + sideLength / 2, position.z));

        //Gizmos.DrawLine(new Vector3(position.x + sideLength / 2, position.y + sideLength / 2, position.z),
        //                new Vector3(position.x - sideLength / 2, position.y + sideLength / 2, position.z));

        //Gizmos.DrawLine(new Vector3(position.x - sideLength / 2, position.y + sideLength / 2, position.z),
        //                new Vector3(position.x - sideLength / 2, position.y - sideLength / 2, position.z));
    }
}
