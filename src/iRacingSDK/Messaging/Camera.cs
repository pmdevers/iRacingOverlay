using System;
using System.Collections.Generic;
using System.Text;

namespace iRacingSDK.Messaging
{
    public class Camera : iRacingMessaging
    {

        internal Camera()
        {
        }

        /// <summary>
        /// Switch to the 'focus on crashes' dynamic camera.
        /// </summary>
        public void FocusOnCrashes()
        {
            SwitchToPosition(-3);
        }

        /// <summary>
        /// Switch to the 'focus on leader' dynamic camera.
        /// </summary>
        public void FocusOnLeader()
        {
            SwitchToPosition(-2);
        }

        /// <summary>
        /// Switch to the 'focus on most exciting' dynamic camera.
        /// </summary>
        public void FocusMostExciting()
        {
            SwitchToPosition(-1);
        }


        /// <summary>
        /// Switch the camera to the specified position and set the camera group.
        /// </summary>
        /// <param name="position">The position of the car to switch to.</param>
        /// <param name="group">The camera group to use.</param>
        public void SwitchToPosition(int position, int group)
        {
            SendMessage(BroadcastMessage.CameraSwitchPos, position, group, 0);
        }

        /// <summary>
        /// Switch the camera to the specified position.
        /// </summary>
        /// <param name="position">The position of the car to switch to.</param>
        public void SwitchToPosition(int position)
        {
            SwitchToPosition(position, 0);
        }

        /// <summary>
        /// Switch the camera to the specified (raw) car number and set the camera group.
        /// </summary>
        /// <param name="carNumberRaw">The car number of the car to switch to. This must be the RAW car number.</param>
        /// <param name="group">The camera group to use.</param>
        public void SwitchToCar(int carNumberRaw, int group)
        {
            SendMessage(BroadcastMessage.CameraSwitchNum, carNumberRaw, group, 0);
        }

        /// <summary>
        /// Switch the camera to the specified (raw) car number and set the camera group.
        /// </summary>
        /// <param name="carNumberRaw">The car number of the car to switch to. This must be the RAW car number.</param>
        public void SwitchToCar(int carNumberRaw)
        {
            SwitchToCar(carNumberRaw, 0);
        }


        /// <summary>
        /// Switch the camera group to use.
        /// </summary>
        /// <param name="group">The camera group to use.</param>
        public void SwitchGroup(int group)
        {
            SendMessage(BroadcastMessage.CameraSwitchPos, 0, group, 0);
        }

        /// <summary>
        /// Set the state of the camera to a (combination of) specified states.
        /// </summary>
        /// <param name="state">A combination of specified states as a bitfield.</param>
        public void SetCameraState(CameraStates state)
        {
            SendMessage(BroadcastMessage.CameraSetState, (int)state, 0);
        }
    }
}
