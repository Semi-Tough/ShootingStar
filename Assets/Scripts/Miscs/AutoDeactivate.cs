using System.Collections;
using UnityEngine;

public class AutoDeactivate : MonoBehaviour
{
    [SerializeField] private float lifeTime = 3f;
    [SerializeField] private bool destroyGameObject;


    private WaitForSeconds waitForDeactivate;

    private void Awake()
    {
        waitForDeactivate = new WaitForSeconds(lifeTime);
    }

    private void OnEnable()
    {
        StartCoroutine(DeactivateCoroutine());
    }

    private IEnumerator DeactivateCoroutine()
    {
        yield return waitForDeactivate;
        if (destroyGameObject)
        {
            Destroy(gameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}