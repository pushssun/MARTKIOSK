using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransUI : MonoBehaviour
{
    public GameObject previous;
    public GameObject next;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(TransRoutine());
    }

    private IEnumerator TransRoutine()
    {
        yield return new WaitForSeconds(2);
        previous.SetActive(false);
        next.SetActive(true);
    }
}
