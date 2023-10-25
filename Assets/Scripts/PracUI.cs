using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PracUI : MonoBehaviour
{
    public GameObject pracText;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DisappearRoutine());
    }

    private IEnumerator DisappearRoutine()
    {
        yield return new WaitForSeconds(3);
        pracText.SetActive(false);
    }
}
