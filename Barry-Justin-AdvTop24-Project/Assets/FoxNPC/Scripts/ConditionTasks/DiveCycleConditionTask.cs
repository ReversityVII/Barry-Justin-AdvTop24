using NodeCanvas.Framework;
using NodeCanvas.StateMachines;
using ParadoxNotion.Design;
using UnityEngine;


namespace NodeCanvas.Tasks.Conditions {

    public class DiveCycleConditionTask : ConditionTask {

        public FSMOwner foxFSM;
        public BBParameter<bool> foundSomething;

        protected override string OnInit(){

            foxFSM = agent.GetComponent<FSMOwner>(); //directly access the FSM to see where it's at

            return null;
		}


		protected override bool OnCheck() {
            string currentState;
            currentState = foxFSM.GetCurrentState(true).ToString();
            

            //change criteria depending on current condition
            if (currentState == "Scanning" || currentState == "Wandering") //within the sub-fsm
            {
                if (foundSomething.value == true) //scan ready?
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (currentState == "Stashing")
            {
                if (foundSomething.value == false) //scan done?
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else { return false; }
            
        }
	}
}