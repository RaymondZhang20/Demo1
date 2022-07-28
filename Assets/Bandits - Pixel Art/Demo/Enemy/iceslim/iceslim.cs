using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class iceslim : Enemy
{
    // Start is called before the first frame update
    public void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public void Update()
    {
        Flip();
        base.Update();
    }

    void Flip()
    {
        if (transform.position.x > movePos.position.x) {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        } else if (transform.position.x < movePos.position.x) {
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
    }
}
