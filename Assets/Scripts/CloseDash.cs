using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseDash : MonoBehaviour
{
    public void CloseDashFunc(GameObject dash)
    {
        dash.SetActive(false);
    }
}
