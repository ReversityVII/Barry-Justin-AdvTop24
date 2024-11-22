using NodeCanvas.Framework;
using UnityEngine;
using UnityEngine.AI;

namespace NodeCanvas.Tasks.Actions {

	public class WanderingActionTask : ActionTask {

		public Vector3 randomPos;
        public NavMeshAgent navAgent;
        public BBParameter<float> maxDistance;

        private float timer;
        private float waitTime;
        private bool isMoving;

        public BBParameter<float> minWaitTime;
        public BBParameter<float> maxWaitTime;
        public BBParameter<bool> scanReady;

        //physical objects referenced for turning on/off
        private GameObject notifier;

        private GameObject searchingIcon;
        private MeshRenderer searchingMesh;

        private GameObject sleepingIcon;
        private MeshRenderer sleepingMesh;
        

        NavMeshHit hit;
		
		protected override void OnExecute() {

            //reset some values
            timer = 0;
            isMoving = false;

            notifier = GameObject.Find("notifier"); //disable notifier at runtime and ensure it isn't there when it's supposed to be
            notifier.GetComponent<Renderer>().enabled = false;

            searchingIcon = agent.gameObject.transform.Find("searching").gameObject;
            searchingMesh = searchingIcon.GetComponent<MeshRenderer>();
            searchingMesh.enabled = false;

            sleepingIcon = agent.gameObject.transform.Find("sleeping").gameObject;
            sleepingMesh = sleepingIcon.GetComponent<MeshRenderer>();
            sleepingMesh.enabled = false;
            
            //start navMesh logic
            navAgent = agent.GetComponent<NavMeshAgent>();
            navAgent.enabled = true; //ensure navAgent is enabled

            randomPos = Random.insideUnitSphere * maxDistance.value + agent.transform.position;//find random position in the bounds 
            NavMesh.SamplePosition(randomPos, out hit, maxDistance.value, NavMesh.AllAreas); //finds the closet point within the navmesh 

            waitTime = Random.Range(minWaitTime.value, maxWaitTime.value); //wait a random amount of time before moving to destination
        }

		
		protected override void OnUpdate() {
            
            timer += Time.deltaTime;

            if (timer > waitTime && isMoving == false) //after a random period of time, move
            {
                navAgent.destination = hit.position; //set destination
                isMoving = true;
                searchingMesh.enabled = false;
                
            }
            

            if (navAgent.remainingDistance < 0.5) //indicate scanning is about to happen
            {
                searchingMesh.enabled = true; 
            }

            if (navAgent.remainingDistance < 0.01 && isMoving == true) //if done moving, confirm that moving is done
            {
                //fox has reached location, transition to scan
                scanReady.value = true;
                
            }
        }
	}
}