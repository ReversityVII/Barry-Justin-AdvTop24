using NodeCanvas.Framework;
using UnityEngine;


namespace NodeCanvas.Tasks.Conditions {

    public class RecoverConditionTask : ConditionTask {

		public BBParameter<GameObject> mouseObject;
        public BBParameter<LayerMask> dirtMask;

        public GameObject mouth;

        protected override string OnInit() {

            mouth = GameObject.Find("MouthHoldPos");

			return null;
		}


		protected override bool OnCheck() {

            //i couldn't get OnCollisionEnter to work in nodecanvas, so i'm doing this instead
            RaycastHit hit = new RaycastHit();
            Physics.Raycast(mouth.transform.position, Vector3.down, out hit, Mathf.Infinity, dirtMask.value);


            if(hit.distance < 0.01) //close to ground
            {
                return true;
            }
            else
            return false;
		}
    }
}