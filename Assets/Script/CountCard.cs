using UnityEngine;

public sealed class CountCard : MonoBehaviour
{
    private void Update()
    {
        if (transform.GetComponentsInChildren<Transform>().LongLength is 13)
        {
            Destroy(gameObject);
        }
    }
}
