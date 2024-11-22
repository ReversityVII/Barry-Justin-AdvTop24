using NodeCanvas.Framework;
using UnityEngine;
using UnityEngine.AI;

namespace NodeCanvas.Tasks.Actions {

	public class DiveActionTask : ActionTask {

		public BBParameter<float> jumpHeight;
		public BBParameter<float> apexTime;
        private NavMeshAgent NavAgent;
        public BBParameter<LayerMask> dirtMask;

        //LOTS of animation logic
		private bool rotationDone;
        private float increment;
		private int wiggles;
        private int currentStretch;
        private bool wigglesDone;
        private float wiggleTimer;

        private float jumpTimer;
        private Vector3 navGround;
        private Vector3 velocity;
        private bool turnaroundDone;
        private Vector3 distanceUp;
        private Vector3 distanceDown;

		protected override void OnExecute() {

            //reset tracking values
            increment = 0;
            wiggles = 0;
            wiggleTimer = 0;
            wigglesDone = false;
            currentStretch = 1;
            jumpTimer = 0;
            turnaroundDone = false;
            velocity = Vector3.zero;

            //where the ground for the navmesh is
            navGround = agent.transform.position; //y level is what matters here

            NavAgent = agent.GetComponent<NavMeshAgent>();
        }

        protected override void OnUpdate() {


            //reset conditional tracking
            rotationDone = false;

            NavAgent.enabled = false; //disable navAgent to allow for free movement

            //rotate 45 degrees in preparation for the jump
            initialRotation();


            //perform anticipation wiggle
            anticipationWiggle();


            //perform jump
            performJump();


            //fall back down from jump
            landJump();
        }

        public void initialRotation()
        {
            if (increment > -45 && rotationDone == false)
            {
                increment -= 0.35f;

                agent.transform.rotation = Quaternion.Euler(increment, agent.transform.rotation.eulerAngles.y, 0);
            }
            else
            {
                rotationDone = true;
            }
        }

        public void anticipationWiggle()
        {
            if (rotationDone == true && wiggles < 5) //do the anticipation move 4 times
            {
                wiggleTimer += Time.deltaTime;

                if (wiggleTimer > 0.25) //by this delay
                {
                    wiggles++;
                    agent.transform.localScale = new Vector3(agent.transform.localScale.x, agent.transform.localScale.y, -(0.8f + (currentStretch * 0.2f)));
                    currentStretch *= -1;
                    wiggleTimer = 0;
                }
            }
            else if (wiggles >= 5)
            {
                wigglesDone = true;
            }
        }

        public void performJump()
        {
            if (wigglesDone)
            {
                jumpTimer += Time.deltaTime;

                agent.transform.localScale = new Vector3(-1, 1, -1); //ensure scale is in place

                distanceUp = navGround + new Vector3(0, navGround.y + jumpHeight.value, 0); //distance from ground to jump apex

                agent.transform.rotation = Quaternion.Euler(-65, agent.transform.rotation.eulerAngles.y, 0); //rotate a bit higher for the jump
                agent.transform.position = Vector3.SmoothDamp(agent.transform.position, distanceUp, ref velocity, apexTime.value);  //perform jump
            }
        }

        public void landJump()
        {
            if (jumpTimer > apexTime.value + 0.1) //jump complete + some buffer because smoothdamp's time can be innacurate
            {
                agent.transform.rotation = Quaternion.Euler(65, agent.transform.rotation.eulerAngles.y, 0); //face the ground again

                if (turnaroundDone == false)
                {
                    RaycastHit hit = new RaycastHit();
                    Physics.Raycast(agent.transform.position, Vector3.down, out hit, Mathf.Infinity, dirtMask.value);

                    distanceDown = agent.transform.position - new Vector3(0, 2 * hit.distance, 0); //distance from jump apex to dirt layer


                    turnaroundDone = true; //everything to do with the turnaround is now done, ready to start falling
                }

                agent.transform.position = Vector3.SmoothDamp(agent.transform.position, distanceDown, ref velocity, apexTime.value);
            }
        }
	}
}