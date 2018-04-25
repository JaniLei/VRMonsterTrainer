using UnityEngine;

public class BreakableStone : MonoBehaviour
{
    public float explosionForce = 1;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
            Break(transform.position);
    }

    public void Break(Vector3 hitPos)
    {
        Rigidbody[] rbs = GetComponentsInChildren<Rigidbody>();
        for (int i = 0; i < rbs.Length; i++)
        {
            rbs[i].useGravity = true;
            rbs[i].isKinematic = false;
            //Vector3 randOffset = new Vector3(Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f));
            rbs[i].AddForceAtPosition((rbs[i].transform.position - hitPos).normalized * explosionForce /*+ randOffset*/, hitPos, ForceMode.Impulse);
        }
    }
}
