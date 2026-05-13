using UnityEditor;
using UnityEngine;

public class Item : MonoBehaviour
{
    protected Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

    }

    public void PickUp(Transform parent, Vector3 pos)
    {
        rb.isKinematic = true;
        transform.SetParent(parent);
        transform.position = pos;
        transform.localRotation = Quaternion.Euler(Vector3.zero);

    }

    public virtual void Throw(float force, Vector3 direction)
    {
        rb.isKinematic = false;
        transform.SetParent(null);

        rb.AddForce(direction * force, ForceMode.Impulse);

    }
}


