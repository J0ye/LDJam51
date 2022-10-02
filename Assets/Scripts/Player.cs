using UnityEngine;

public class Player : MonoBehaviour
{

    protected virtual Vector3 GetInputAxis()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // calculate direction vectors based on orientation
        Vector3 forward = transform.up * v;
        Vector3 sideways = transform.right * h;
        
        Vector3 re = forward + sideways;
        return re;
    }

    protected virtual Vector3 GetInputAxisRaw()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        // calculate direction vectors based on orientation
        Vector3 forward = transform.up * v;
        Vector3 sideways = transform.right * h;

        Vector3 re = forward + sideways;
        return re;
    }

    protected virtual bool ShouldMove()
    {
        if(GetInputAxisRaw() != Vector3.zero)
        {
            return true;
        }

        return false;
    }
}
