using NodeCanvas.Framework;
using NodeCanvas.StateMachines;
using ParadoxNotion.Design;
using Unity.VisualScripting;
using UnityEngine;


namespace NodeCanvas.Tasks.Conditions {

	public class ScanCycleConditionTask : ConditionTask {

		public FSMOwner foxFSM;
		public BBParameter<bool> scanReady;

		protected override string OnInit(){

			foxFSM = agent.GetComponent<FSMOwner>(); //directly access the FSM to see where it's at

            return null;
		}

		protected override bool OnCheck() {

            string currentState;
            currentState = foxFSM.GetCurrentState(true).ToString();

            //change criteria depending on current condition
            if (currentState == "Wandering")  
			{
				if(scanReady.value == true) //scan ready?
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			else if (currentState == "Scanning")
			{
                if (scanReady.value == false) //scan done?
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
			return true;
		}
	}
}