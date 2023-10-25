using UnityEngine;

public sealed class CountCard : MonoBehaviour
{
    private void Update()
    {
        if (transform.GetComponentInChildren<Transform>().childCount is 13)
        {
            Destroy(gameObject);
        }
    }
}
