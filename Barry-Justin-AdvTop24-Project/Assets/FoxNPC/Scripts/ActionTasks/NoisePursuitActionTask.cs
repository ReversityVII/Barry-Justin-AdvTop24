using NodeCanvas.Framework;
using UnityEngine;
using UnityEngine.AI;

namespace NodeCanvas.Tasks.Actions {

	public class NoisePursuitActionTask : ActionTask {

		public BBParameter<GameObject> targetMouse;
		private Vector3 mouseDirection;

		private NavMeshAgent NavAgent;
        private GameObject notifier;
        NavMeshHit hit;

		protected override void OnExecute() {
            
            //move to mouse it heard
            NavAgent = agent.GetComponent<NavMeshAgent>();
            NavMesh.SamplePosition(targetMouse.value.transform.position, out hit, Mathf.Infinity, NavMesh.AllAreas); //finds the closet point within the navmesh
            NavAgent.destination = hit.position; //set destination

            //enable seen notifier
            notifier = GameObject.Find("notifier"); //not great practice for larger games, but works for our case
			notifier.GetComponent<Renderer>().enabled = true;
        }
	}
}