using UnityEngine;

namespace KartGame.KartSystems
{
    public class KeyboardInput : BaseInput
    {
        public string TurnInputName = "Horizontal";
        public string AccelerateButtonName = "Accelerate";
        public string BrakeButtonName = "Brake";

        public VariableJoystick _VerticalJoystick;
        public VariableJoystick _HorizontalJoystick;

        public override InputData GenerateInput()
        {
            return new InputData
            {

#if !UNITY_EDITOR
                Accelerate = _VerticalJoystick.Vertical > 0 ? true : false,
                Brake = _VerticalJoystick.Vertical < 0 ? true : false,
                TurnInput = _HorizontalJoystick.Horizontal
#else
                Accelerate = Input.GetButton(AccelerateButtonName),
                Brake = Input.GetButton(BrakeButtonName),
                TurnInput = Input.GetAxis("Horizontal")
#endif
            };
        }
    }
}
