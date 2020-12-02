using System;
using System.Collections.Generic;
using System.Text;

namespace iRacingSDK.Messaging
{
    public enum CameraStates
    {
        IsSessionScreen = 1, // the camera tool can only be activated if viewing the session screen (out of car)
        IsScenicActive = 2, // the scenic camera is active (no focus car)
        CamToolActive = 4,
        UIHidden = 8,
        UseAutoShotSelection = 10,
        UseTemporaryEdits = 20,
        UseKeyAcceleration = 40,
        UseKey10xAcceleration = 80,
        UseMouseAimMode = 100
    }
}
