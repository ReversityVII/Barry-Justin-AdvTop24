using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Conditions {

	public class SleepingConditionTask : ConditionTask {

		public BBParameter<float> sleepThreshold;
		private float sleepTimer; 

        protected override void OnEnable()
        {
			sleepTimer = 0; //reset the sleep timer every time something interesting happens (this gets enabled again) 
        }
        protected override bool OnCheck() {

			sleepTimer += Time.deltaTime;

			if (sleepTimer > sleepThreshold.value) //if nothing has happened for a long enough time
			{
				return true;
			}

			return false;
		}
	}
}