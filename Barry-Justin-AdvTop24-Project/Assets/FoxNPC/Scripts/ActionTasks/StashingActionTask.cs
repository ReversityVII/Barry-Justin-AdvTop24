using NodeCanvas.Framework;
using UnityEngine;
using UnityEngine.AI;


namespace NodeCanvas.Tasks.Actions {

	public class StashingActionTask : ActionTask {

        public BBParameter<GameObject> mouse;
        public BBParameter<GameObject> mouth;
        public BBParameter<GameObject> stashSpot;
        public BBParameter<bool> foundSomething;

        public MouseBehavior mouseBehavior;

        public NavMeshAgent navAgent;
        NavMeshHit hit;

		protected override void OnExecute() {


            //move to stash
            navAgent = agent.GetComponent<NavMeshAgent>();
            NavMesh.SamplePosition(stashSpot.value.transform.position, out hit, Mathf.Infinity, NavMesh.AllAreas); //finds the closet point within the navmesh 
            navAgent.destination = hit.position; //set destination

            mouseBehavior = mouse.value.gameObject.GetComponent<MouseBehavior>();
		}

		protected override void OnUpdate() {

            if (navAgent.remainingDistance < 0.1 && navAgent.pathPending == false)                                                                      
            {
                mouse.value.transform.SetParent(null); //unparent the mouse
                Rigidbody mouseBody = mouse.value.GetComponent<Rigidbody>();
                mouseBody.isKinematic = false; //give it back it's physics

                mouse.value.layer = 9; //set collected mice on independent layer

                foundSomething.value = false; 
            }
		}
	}
}