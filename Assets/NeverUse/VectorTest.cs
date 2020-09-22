using UnityEngine;

// 1. 위치 벡터
// 2. 방향 벡터
struct MyVector
{
    public float x;
    public float y;
    public float z;

    public float magnitude { get { return Mathf.Sqrt(x * x + y * y + z * z); } }
    public MyVector normalized { get { return new MyVector(x / magnitude, y / magnitude, z / magnitude); } }

    public MyVector(float x, float y, float z) { this.x = x; this.y = y; this.z = z; }

    public static MyVector operator +(MyVector a, MyVector b)
    {
        return new MyVector(a.x + b.x, a.y + b.y, a.z + b.z);
    }

    public static MyVector operator -(MyVector a, MyVector b)
    {
        return new MyVector(a.x - b.x, a.y - b.y, a.z - b.z);
    }

    public static MyVector operator *(MyVector a, float b)
    {
        return new MyVector(a.x * b, a.y * b, a.z * b);
    }

}

public class VectorTest : MonoBehaviour
{
    private float speedTest;

    // Start is called before the first frame update
    void Start()
    {
        //MyVector to = new MyVector(10.0f, 0.0f, 0.0f);
        //MyVector from = new MyVector(5.0f, 0.0f, 0.0f);
        //MyVector dir = to - from; // 5.0f, 0.0f, 0.0f;

        //dir = dir.normalized;

        //MyVector newPos = from + dir * speedTest;

        // 방향 벡터
        // 1. 거리(크기) magnitude
        // 2. 실제 방향
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
