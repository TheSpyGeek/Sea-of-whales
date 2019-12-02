using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{

    public LayerMask whatcanBeClickedOn;

    private NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)) {
			Debug.Log("Test click");
            if(!PauseMenu.gamePaused) {
				Debug.Log("action");
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if(Physics.Raycast(ray, out hit, 100, whatcanBeClickedOn)) {
                    agent.SetDestination(hit.point);
                }
            }
        }
        
    }
}
