using System.Collections;
using UnityEngine;

public class LootWeaponController : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(nameof(Despawn));
    }

    IEnumerator Despawn()
    {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }
}