using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Media.Devices;

namespace SDX.Toolkit.Helpers
{
    // ---------------------------------------------------------------------------------------------
    // TODO:
    // ---------------------------------------------------------------------------------------------
    // !!! We are also going to need to add the code to detect that Joplin is plugged in, to choose it as the output, 
    // !!! and to upgrade its firmware.  - PSS
    // ---------------------------------------------------------------------------------------------

    // ---------------------------------------------------------------------------------------------
    // Patrick Strader - The code below is taken from the source denoted in the comments. It has 
    // been modified to match our needs more closely, and to match our code style.
    // ---------------------------------------------------------------------------------------------
    // Control the system volume from UWP, using the IAudioEndpointVolume interface
    //
    // Wim Bokkers
    //
    // Credits:
    // * Reddit user sunius (https://www.reddit.com/user/sunius)
    //    See this thread: https://www.reddit.com/r/WPDev/comments/4kqzkb/launch_exe_with_parameter_in_uwp/d3jepi7/
    //    And this code: https://pastebin.com/cPhVCyWj
    //
    // The code provided by sunius has two major drawbacks:
    //   - It uses unsafe code
    //   - It does not work in Release mode (the app crashes)
    //
    // Marshalling the Guid pointers as out parameters will fix this.
    //
    // This code is also available from: https://gist.github.com/wbokkers/74e05ccc1ee2371ec55c4a7daf551a26
    //---

    internal enum HResult : uint
    {
        S_OK = 0
    }

    public static class AudioHelper
    {
        public static bool SetVolumeTo(double volume)
        {
            bool succeeded = false;

            // level can only be between 0 and 1
            volume = Math.Min(1, volume);
            volume = Math.Max(0, volume);

            try
            {
                // get the master volume interface
                var masterVol = GetAudioEndpointVolumeInterface();
                if (null == masterVol) { return succeeded; }

                // Make sure that the audio is not muted (unless we mean to mute it
                if (volume > 0)
                {
                    masterVol.SetMute(false, Guid.Empty);
                }
                else
                {
                    masterVol.SetMute(true, Guid.Empty);
                }

                // ** We're not looking here to set it to "at least" a certain value, 
                // ** we're looking to set it to a specific value
                float newAudioValue = Convert.ToSingle(volume);
                //// Only adapt volume if the current level is below the specified minimum level
                //var currentAudioValue = masterVol.GetMasterVolumeLevelScalar();
                //float newAudioValue = Convert.ToSingle(volume);
                //if (currentAudioValue > newAudioValue)
                //    return;

                // set the volume
                masterVol.SetMasterVolumeLevelScalar(newAudioValue, Guid.Empty);

                // success
                succeeded = true;
            }
            catch
            {
                succeeded = false;
            }

            return succeeded;
        }

        private static IAudioEndpointVolume GetAudioEndpointVolumeInterface()
        {
            // get the non-communications device
            var speakerId = MediaDevice.GetDefaultAudioRenderId(AudioDeviceRole.Default);

            var completionHandler = new ActivateAudioInterfaceCompletionHandler<IAudioEndpointVolume>();

            var hr = ActivateAudioInterfaceAsync(
                speakerId,
                typeof(IAudioEndpointVolume).GetTypeInfo().GUID,
                IntPtr.Zero,
                completionHandler,
                out var activateOperation);

            Debug.Assert(hr == (uint)HResult.S_OK);

            return completionHandler.WaitForCompletion();
        }

        [DllImport("Mmdevapi.dll", ExactSpelling = true, PreserveSig = false)]
        [return: MarshalAs(UnmanagedType.Error)]
        private static extern uint ActivateAudioInterfaceAsync(
                [In, MarshalAs(UnmanagedType.LPWStr)]string deviceInterfacePath,
                [In, MarshalAs(UnmanagedType.LPStruct)]Guid riid,
                [In] IntPtr activationParams,
                [In] IActivateAudioInterfaceCompletionHandler completionHandler,
                out IActivateAudioInterfaceAsyncOperation activationOperation);

        internal class ActivateAudioInterfaceCompletionHandler<T> : IActivateAudioInterfaceCompletionHandler
        {
            private AutoResetEvent _completionEvent;
            private T _result;

            public ActivateAudioInterfaceCompletionHandler()
            {
                _completionEvent = new AutoResetEvent(false);
            }

            public void ActivateCompleted(IActivateAudioInterfaceAsyncOperation operation)
            {
                operation.GetActivateResult(out var hr, out var activatedInterface);

                Debug.Assert(hr == (uint)HResult.S_OK);

                _result = (T)activatedInterface;

                var setResult = _completionEvent.Set();
                Debug.Assert(setResult != false);
            }

            public T WaitForCompletion()
            {
                var waitResult = _completionEvent.WaitOne();
                Debug.Assert(waitResult != false);

                return _result;
            }
        }
    }

    [ComImport]
    [Guid("5CDF2C82-841E-4546-9722-0CF74078229A"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IAudioEndpointVolume
    {
        void NotImpl1();

        void NotImpl2();

        [return: MarshalAs(UnmanagedType.U4)]
        uint GetChannelCount();

        void SetMasterVolumeLevel(
            [In] [MarshalAs(UnmanagedType.R4)] float level,
            [In] [MarshalAs(UnmanagedType.LPStruct)] Guid eventContext);

        void SetMasterVolumeLevelScalar(
            [In] [MarshalAs(UnmanagedType.R4)] float level,
            [In] [MarshalAs(UnmanagedType.LPStruct)] Guid eventContext);

        [return: MarshalAs(UnmanagedType.R4)]
        float GetMasterVolumeLevel();

        [return: MarshalAs(UnmanagedType.R4)]
        float GetMasterVolumeLevelScalar();

        void SetChannelVolumeLevel(
            [In] [MarshalAs(UnmanagedType.U4)] uint channelNumber,
            [In] [MarshalAs(UnmanagedType.R4)] float level,
            [In] [MarshalAs(UnmanagedType.LPStruct)] Guid eventContext);

        void SetChannelVolumeLevelScalar(
            [In] [MarshalAs(UnmanagedType.U4)] uint channelNumber,
            [In] [MarshalAs(UnmanagedType.R4)] float level,
            [In] [MarshalAs(UnmanagedType.LPStruct)] Guid eventContext);

        void GetChannelVolumeLevel(
            [In] [MarshalAs(UnmanagedType.U4)] uint channelNumber,
            [Out] [MarshalAs(UnmanagedType.R4)] out float level);

        [return: MarshalAs(UnmanagedType.R4)]
        float GetChannelVolumeLevelScalar([In] [MarshalAs(UnmanagedType.U4)] uint channelNumber);

        void SetMute(
            [In] [MarshalAs(UnmanagedType.Bool)] bool isMuted,
            [In] [MarshalAs(UnmanagedType.LPStruct)] Guid eventContext);

        [return: MarshalAs(UnmanagedType.Bool)]
        bool GetMute();

        void GetVolumeStepInfo(
            [Out] [MarshalAs(UnmanagedType.U4)] out uint step,
            [Out] [MarshalAs(UnmanagedType.U4)] out uint stepCount);

        void VolumeStepUp([In] [MarshalAs(UnmanagedType.LPStruct)] Guid eventContext);

        void VolumeStepDown([In] [MarshalAs(UnmanagedType.LPStruct)] Guid eventContext);

        [return: MarshalAs(UnmanagedType.U4)] // bit mask
        uint QueryHardwareSupport();

        void GetVolumeRange(
            [Out] [MarshalAs(UnmanagedType.R4)] out float volumeMin,
            [Out] [MarshalAs(UnmanagedType.R4)] out float volumeMax,
            [Out] [MarshalAs(UnmanagedType.R4)] out float volumeStep);
    }

    [ComImport]
    [Guid("72A22D78-CDE4-431D-B8CC-843A71199B6D")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IActivateAudioInterfaceAsyncOperation
    {
        void GetActivateResult(
            [MarshalAs(UnmanagedType.Error)]out uint activateResult,
            [MarshalAs(UnmanagedType.IUnknown)]out object activatedInterface);
    }

    [ComImport]
    [Guid("41D949AB-9862-444A-80F6-C261334DA5EB")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IActivateAudioInterfaceCompletionHandler
    {
        void ActivateCompleted(IActivateAudioInterfaceAsyncOperation activateOperation);
    }
}
