using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowTarget : MonoBehaviour
{
    public Transform player1;
    public Transform player2;

    private Vector3 offset;
    private Camera gameCamera;
    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - (player1.position + player2.position) / 2;
        gameCamera = this.GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPosition = (player1.position + player2.position) / 2 + offset;
        // y不发生改变
        newPosition.y = offset.y;
        transform.position = newPosition;
        float distance = Vector3.Distance(player1.position,player2.position);
        float size = distance*0.55f;
        if(size <= 10)
            size = 10;
        gameCamera.orthographicSize = size;
    }
}
