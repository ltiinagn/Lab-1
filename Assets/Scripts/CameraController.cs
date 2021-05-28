using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        Transform marioTransform = GameObject.Find("UI").GetComponent<MenuController>().mainGameObject.transform.Find("Mario");
        if (marioTransform != null) {
            this.transform.position = new Vector3(marioTransform.position.x, this.transform.position.y, this.transform.position.z);
        }
    }
}
