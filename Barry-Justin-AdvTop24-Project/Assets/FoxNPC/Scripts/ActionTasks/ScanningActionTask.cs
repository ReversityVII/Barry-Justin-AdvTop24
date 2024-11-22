using NodeCanvas.Framework;
using UnityEngine;


namespace NodeCanvas.Tasks.Actions {

	public class ScanningActionTask : ActionTask {

		public BBParameter<bool> foundSomething;
		public BBParameter<float> scanDistance;
		public BBParameter<GameObject> targetMouse;
		public BBParameter<bool> scanReady;
		public BBParameter<LayerMask> mouseLayer;

		private GameObject searchingIcon;
		private MeshRenderer searchingMesh;
        public MouseBehavior mouseBehavior;

        private float currentScanRadius;
		private float scanIncrement;
		

		private Collider[] col;

		protected override void OnExecute() {
			currentScanRadius = 0;
			scanIncrement = 0.05f;

			searchingIcon = agent.gameObject.transform.Find("searching").gameObject;
			searchingMesh = searchingIcon.GetComponent<MeshRenderer>();
		}

		protected override void OnUpdate() {
			for (currentScanRadius = 0; currentScanRadius < scanDistance.value; currentScanRadius += scanIncrement)
			{
				//check for a mouse
				col = Physics.OverlapSphere(agent.transform.position, currentScanRadius, mouseLayer.value);

				if (col.Length > 0)
				{
					targetMouse.value = col[0].gameObject;
                    mouseBehavior = targetMouse.value.gameObject.GetComponent<MouseBehavior>();
					
					if(mouseBehavior.hasBeenCollected == false)
					{
                        foundSomething.value = true;
						scanReady.value = false;
                        break;
                    }
				}
			}

			searchingMesh.enabled = false; 

            if (foundSomething.value == false) //has completed searching, found nothing
			{
				scanReady.value = false; 
			}
		}
	}
}