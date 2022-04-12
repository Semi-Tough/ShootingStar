using System.Collections;
using UnityEngine;

public class AutoDeactivate : MonoBehaviour
{
    [SerializeField] private float lifeTime = 3f;
    [SerializeField] private bool destroyGameObject;

   
    private WaitForSeconds waitForSeconds;

    private void Awake()
    {
        waitForSeconds = new WaitForSeconds(lifeTime);
    }

    private void OnEnable()
    {
        StartCoroutine(DeactivateCoroutine());
    }

    IEnumerator DeactivateCoroutine()
    {
        yield return waitForSeconds;
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