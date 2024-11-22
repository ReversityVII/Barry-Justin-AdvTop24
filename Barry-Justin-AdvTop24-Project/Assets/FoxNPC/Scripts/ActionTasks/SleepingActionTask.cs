using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;
using UnityEngine.AI;

namespace NodeCanvas.Tasks.Actions {

	public class SleepingActionTask : ActionTask {

		public BBParameter<GameObject> restSpot;
		
        NavMeshAgent navAgent;
        NavMeshHit hit;

        //icons it will need to change
        private GameObject sleepingIcon;
        private MeshRenderer sleepingMesh;
        private GameObject searchingIcon;
        private MeshRenderer searchingMesh;

		protected override void OnExecute() {

            //move to rest spot
            navAgent = agent.GetComponent<NavMeshAgent>();
            NavMesh.SamplePosition(restSpot.value.transform.position, out hit, Mathf.Infinity, NavMesh.AllAreas); //finds the closet point within the navmesh 
            navAgent.destination = hit.position; //set destination

        }

		protected override void OnUpdate() {
			
            if(navAgent.enabled == true && navAgent.remainingDistance < 0.1 && navAgent.pathPending == false) //if the navAgent is present, and the fox is close enough to its location                                                                                                    
            {
                //enable sleeping icon
                sleepingIcon = agent.gameObject.transform.Find("sleeping").gameObject;
                sleepingMesh = sleepingIcon.GetComponent<MeshRenderer>();
                sleepingMesh.enabled = true;

                //disable wandering icon if applicable
                searchingIcon = agent.gameObject.transform.Find("searching").gameObject;
                searchingMesh = searchingIcon.GetComponent<MeshRenderer>();
                searchingMesh.enabled = false;

                //sleepy time
                navAgent.enabled = false; 
                agent.transform.rotation = Quaternion.Euler(agent.transform.rotation.x, agent.transform.rotation.y, 90);
            }

		}
	}
}