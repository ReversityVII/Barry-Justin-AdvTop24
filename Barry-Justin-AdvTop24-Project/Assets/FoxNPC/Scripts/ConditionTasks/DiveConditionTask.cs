using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;
using UnityEngine.AI;


namespace NodeCanvas.Tasks.Conditions {

	public class DiveConditionTask : ConditionTask {

        private NavMeshAgent navAgent;
        private GameObject notifier;

		protected override void OnEnable() {

            navAgent = agent.GetComponent<NavMeshAgent>();
            notifier = GameObject.Find("notifier"); //not great practice, but works for our case
        }

		protected override bool OnCheck() {
            
            //if fox is close enough to position
            if (navAgent.remainingDistance < 0.5) //a little bit ahead so it's more of a seamless transition
            {
                //fox is in place
                navAgent.isStopped = true;
                notifier.GetComponent<Renderer>().enabled = false;

                return true;
            }
            else
            {
                return false;
            }            
		}
	}
}