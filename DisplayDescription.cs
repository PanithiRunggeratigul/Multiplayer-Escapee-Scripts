using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayDescription : MonoBehaviour
{
    [SerializeField] GameObject description;
    public void IsFacing()
    {
        if (description != null)
        {
            description.SetActive(true);
        }
    }

    public void IsNotFacing()
    {
        if (description != null)
        {
            description.SetActive(false);
        }
    }
}
