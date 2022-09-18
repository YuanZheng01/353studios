using System;
using InputSamples.Controls;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InputSamples.Drawing.Test
{
    /// <summary>
    /// Input manager that interprets pen, mouse and touch input for mostly drag related controls.
    /// Passes pressure, tilt, twist and touch radius through to drawing components for processing.
    /// </summary>
    /// <remarks>
    /// Couple notes about the control setup:
    ///
    /// - Touch is split off from mouse and pen instead of just using `&lt;Pointer&gt;/position` etc.
    ///   in order to support multi-touch. If we just bind to <see cref="Touchscreen.position"/> and
    ///   such, we will correctly receive the primary touch but the primary touch only. So we put
    ///   bindings for pen and mouse separate to those from touch.
    /// - Mouse and pen are put into one composite. The expectation here is that they are not used
    ///   independently from another and thus don't need to be represented as separate pointer sources.
    ///   However, we could just as well have one <see cref="PointerInputComposite"/> for mice and
    ///   one for pens.
    /// - <see cref="InputAction.passThrough"/> is enabled on <see cref="PointerControls.PointerActions.point"/>.
    ///   The reason is that we want to source arbitrary many pointer inputs through one single actions.
    ///   Without pass-through, the default conflict resolution on actions would kick in and let only
    ///   one of the composite bindings through at a time.
    /// </remarks>
    public class TestPointerInputManager : MonoBehaviour
    {
        #region Attributes
        /// <summary>
        /// Event fired when the user presses on the screen.
        /// </summary>
        public event Action<PointerInput, double> Pressed;

        /// <summary>
        /// Event fired as the user drags along the screen.
        /// </summary>
        public event Action<PointerInput, double> Dragged;

        /// <summary>
        /// Event fired when the user releases a press.
        /// </summary>
        public event Action<PointerInput, double> Released;

        private bool m_Dragging;
        private bool m_Pressed; // added to help define release actions see OnAction() function below
        
        private PointerControls m_Controls;

        // These are useful for debugging, especially when touch simulation is on.
        [SerializeField] private bool m_UseMouse;
        [SerializeField] private bool m_UsePen;
        [SerializeField] private bool m_UseTouch;
        #endregion

        #region Unity_Functions
        protected virtual void Awake()
        {
            m_Controls = new PointerControls();

            m_Controls.pointer.point.performed += OnAction;
            // The action isn't likely to actually cancel as we've bound it to all kinds of inputs but we still
            // hook this up so in case the entire thing resets, we do get a call.
            m_Controls.pointer.point.canceled += OnAction;

            SyncBindingMask();
        }

        protected virtual void OnEnable()
        {
            m_Controls?.Enable();
        }

        protected virtual void OnDisable()
        {
            m_Controls?.Disable();
        }
        #endregion

        #region Input_Action_Functions
        protected void OnAction(InputAction.CallbackContext context) // called when an input action is detected then takes in its CallbackContext as the variable context -Joseph Roberts
        {
            var control = context.control; // creates a variable for the control of context -Joseph Roberts
            var device = control.device;   // creates a variable for the device of context -Joseph Roberts

            var isMouseInput = device is Mouse; // creates a variable for mouse input -Joseph Roberts
            var isPenInput = !isMouseInput && device is Pen; // creates a variable for pen input and blanks isMouseInput variable -Joseph Roberts

            // Read our current pointer values.
            var drag = context.ReadValue<PointerInput>(); // variable for testing for drag contact in if statements below -Joseph Roberts
            if (isMouseInput)
                drag.InputId = Helpers.LeftMouseInputId; // if mouse is used, get teh input ID from the left mouse button -Joseph Roberts
            else if (isPenInput)
                drag.InputId = Helpers.PenInputId;  // if pen is used, get the input ID from the pen -Joseph Roberts

            if (drag.Contact && !m_Dragging) // Is the screen being touched and has the "is dragging" flag/boolean been turned OFF(i.e. is FALSE)? -Joseph Roberts
            {
                // Debug.Log( "TestPointerInputManager.cs on " + this.gameObject.name + " received an input and is performing a Pressed action."); // used for debug; confirms the TestPointerInputManager.cs received an action and is performing a Pressed -Joseph Roberts
                Pressed?.Invoke(drag, context.time); // if the answer is YES (i.e. TRUE), call the Pressed action event then... -Joseph Roberts
                m_Dragging = true;                   // ... turn ON (set to TRUE) the  "is dragging" and...  -Joseph Roberts
                m_Pressed = true;                    // ... the "was pressed" flag/boolean variables -Joseph Roberts
            }
            else if (drag.Contact && m_Dragging) // Is the screen being touched has the "is dragging" flag/boolean been turned ON (i.e. is TRUE)? -Joseph Roberts
            {
                // Debug.Log( "TestPointerInputManager.cs on " + this.gameObject.name + " received an input and is performing a Dragged action."); // used for debug; confirms the TestPointerInputManager.cs received an action and is performing a Dragging -Joseph Roberts
                Dragged?.Invoke(drag, context.time); // if the answer is YES (i.e. TRUE), call the Dragged action event
            }
            else if (!drag.Contact && m_Pressed) // added to ask - Was the screen being touched/pressed before release and has the "was pressed" flag/boolean been turned ON (i.e. is TRUE)?
            {
                // Debug.Log( "TestPointerInputManager.cs on " + this.gameObject.name + " received an input and is performing a Released action."); // used for debug; confirms the TestPointerInputManager.cs received an action and is performing a Released -Joseph Roberts
                Released?.Invoke(drag, context.time); // if the answer is yes (i.e. TRUE), call the Released action event then... -Joseph Roberts
                m_Dragging = false;                   // ... reset the "is dragging"... -Joseph Roberts
                m_Pressed = false;                    // ... and "was pressed" flags/boolean variables by turning them OFF (i.e. set them to FALSE) -Joseph Roberts
            }
            else // used for debug; any action that does not start any of the "if" statements above will fall under this "else" and perform is actions -Joseph Roberts
            {
                // Debug.Log("Unused or unknown action input received on TestPointerInputManager.cs of " + this.gameObject.name + ". See the OnAction() function on " + this.name + " for more information.");
            }
        }

        private void SyncBindingMask()
        {
            if (m_Controls == null)
                return;

            if (m_UseMouse && m_UsePen && m_UseTouch)
            {
                m_Controls.bindingMask = null;
                return;
            }

            m_Controls.bindingMask = InputBinding.MaskByGroups(new[]
            {
                m_UseMouse ? "Mouse" : null,
                m_UsePen ? "Pen" : null,
                m_UseTouch ? "Touch" : null
            });
        }

        private void OnValidate()
        {
            SyncBindingMask();
        }
        #endregion
    }
}
