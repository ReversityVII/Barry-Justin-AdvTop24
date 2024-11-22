using NodeCanvas.Framework;
using UnityEngine;
using UnityEngine.AI;

namespace NodeCanvas.Tasks.Actions {

	public class RecoverActionTask : ActionTask {

		public BBParameter<GameObject> mouse;
		public BBParameter<GameObject> MouthHoldPos;

        public NavMeshAgent NavAgent;

        private int wiggles;
        private int currentStretch;
        private bool wigglesDone;
        private float wiggleTimer;

		protected override void OnExecute() {

            //make fox grab the mouse
			mouse.value.transform.SetParent(MouthHoldPos.value.transform); //fox grabs the mouse
            Rigidbody mouseBody = mouse.value.GetComponent<Rigidbody>(); 
            mouseBody.isKinematic = true; //do not override movement from the rigidbody from the fox's mouth 

            //reset some tracking values
            wiggles = 0;
            wigglesDone = false;
            currentStretch = 1;
            NavAgent = agent.GetComponent<NavMeshAgent>();
        }

		protected override void OnUpdate() {

            //wiggle back out of the ground
            if (wiggles < 5) //do the anticipation move 4 times
            {
                wiggleTimer += Time.deltaTime;

                if (wiggleTimer > 0.25) //by this delay
                {
                    wiggles++;
                    agent.transform.localScale = new Vector3(agent.transform.localScale.x, agent.transform.localScale.y, -(0.8f + (currentStretch * 0.1f)));
                    currentStretch *= -1;
                    wiggleTimer = 0;
                }
            }
            else if (wiggles >= 5)
            {
                wigglesDone = true;
            }

            //re-enstate the nav agent when the fox has recovered
            if (wigglesDone == true)
            {
                NavAgent.enabled = true;
            }

        }
    }
}
