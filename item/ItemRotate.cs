using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRotate : MonoBehaviour
{
    [SerializeField] SpriteRenderer sprite;

    // Update is called once per frame
    void Update()
    {
        sprite.transform.Rotate(0, -0.5f, 0);
    }
}
