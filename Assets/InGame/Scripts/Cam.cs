using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class Cam : Item 
{

    // Update is called once per frame
    public override void Throw(float force, Vector3 direction)
    {
        rb.isKinematic = false;
        transform.SetParent(null);

        rb.AddForce(direction * force, ForceMode.Impulse);

    }

    public void Stick(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            rb.isKinematic = false;
        }
        

    }
}
