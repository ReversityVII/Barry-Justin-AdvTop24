using NodeCanvas.Framework;
using UnityEngine;
using UnityEngine.AI;


namespace NodeCanvas.Tasks.Conditions {

	public class StashingConditionTask : ConditionTask {

		public BBParameter<GameObject> mouse;
		public BBParameter<GameObject> mouth;

        public NavMeshAgent NavAgent;

        protected override string OnInit(){

            NavAgent = agent.GetComponent<NavMeshAgent>();

            return null;
		}

		protected override bool OnCheck() {

            //if the mouth has a child and the navagent is active (all major changes done)
            if (mouth.value.transform.childCount > 0 && NavAgent.enabled == true) 
			{
				return true;
			}
			else
				return false;

		
		}
	}
}