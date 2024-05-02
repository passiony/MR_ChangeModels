using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCloseCpu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject obj = GameObject.Find("Diagnostics");
        obj.SetActive(false);
    }

}
