using System;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CSCore.CoreAudioAPI;
using System.Diagnostics;

namespace CSCore.Test.CoreAudioAPI
{
    [TestClass]
    public class AudioSessionTests
    {
        const string category = "CoreAudioAPI.AudioSession";

        [TestMethod]
        [TestCategory(category)]
        public void CanCreateAudioSessionManager2()
        {
            using (var enumerator = new MMDeviceEnumerator())
            {
                using (var collection = enumerator.EnumAudioEndpoints(DataFlow.Render, DeviceState.Active))
                {
                    foreach (var device in collection)
                    {
                        using (var sessionManager = AudioSessionManager2.FromMMDevice(device))
                        {
                            Assert.IsNotNull(sessionManager);
                        }
                    }
                }
            }
        }

        [TestMethod]
        [TestCategory(category)]
        public void CanRegisterSessionNotification()
        {
            using (var sessionManager = GetDefaultAudioSessionManager2(DataFlow.Render))
            {
                AudioSessionNotification notification = new AudioSessionNotification();
                
                RegisterSessionNotification(sessionManager, notification);

                sessionManager.UnregisterSessionNotification(notification);
            }
        }

        [TestMethod]
        [TestCategory(category)]
        public void CanRegisterDuckNotification()
        {
            using (var sessionManager = GetDefaultAudioSessionManager2(DataFlow.Render))
            {
                AudioVolumeDuckNotification duckNotification = new AudioVolumeDuckNotification();
                sessionManager.RegisterDuckNotification(null, duckNotification);
                sessionManager.UnregisterDuckNotification(duckNotification);
            }
        }

        [TestMethod]
        [TestCategory(category)]
        public void CanGetSessionEnumerator()
        {
            using (var sessionManager = GetDefaultAudioSessionManager2(DataFlow.Render))
            {
                using (var sessionEnumerator = sessionManager.GetSessionEnumerator())
                {
                    Assert.IsNotNull(sessionEnumerator);
                }
            }
        }

        [TestMethod]
        [TestCategory(category)]
        public void CanEnumerateSessions()
        {
            using (var sessionManager = GetDefaultAudioSessionManager2(DataFlow.Render))
            {
                using (var sessionEnumerator = sessionManager.GetSessionEnumerator())
                {
                    Assert.IsNotNull(sessionEnumerator);

                    foreach (var session in sessionEnumerator)
                    {
                        Assert.IsNotNull(session);
                    }
                }
            }
        }

        [TestMethod]
        [TestCategory(category)]
        public void CanGetSessionProperties()
        {
            using (var sessionManager = GetDefaultAudioSessionManager2(DataFlow.Render))
            {
                using (var sessionEnumerator = sessionManager.GetSessionEnumerator())
                {
                    Assert.IsNotNull(sessionEnumerator);

                    foreach (var session in sessionEnumerator)
                    {
                        Assert.IsNotNull(session);

                        Debug.WriteLine("---------------------------------");
                        Debug.WriteLine("DisplayName: {0}\nIconPath: {1}\nState: {2}\nGroupingParam: {3}",
                            session.DisplayName,
                            session.IconPath,
                            session.SessionState,
                            session.GroupingParam);
                        Debug.WriteLine("---------------------------------");
                    }
                }
            }
        }

        [TestMethod]
        [TestCategory(category)]
        public void CanGetSimpleVolumeOfAudioSession()
        {
            using (var sessionManager = GetDefaultAudioSessionManager2(DataFlow.Render))
            {
                using (var sessionEnumerator = sessionManager.GetSessionEnumerator())
                {
                    Assert.IsNotNull(sessionEnumerator);

                    foreach (var session in sessionEnumerator)
                    {
                        Assert.IsNotNull(session);

                        using (var simpleVolume = session.QueryInterface<SimpleAudioVolume>())
                        {
                            Assert.IsNotNull(simpleVolume);
                        }
                    }
                }
            }
        }

        [TestMethod]
        [TestCategory(category)]
        public void CanSetAudioSessionVolumeAndMute()
        {
            using (var sessionManager = GetDefaultAudioSessionManager2(DataFlow.Render))
            {
                using (var sessionEnumerator = sessionManager.GetSessionEnumerator())
                {
                    Assert.IsNotNull(sessionEnumerator);

                    foreach (var session in sessionEnumerator)
                    {
                        Assert.IsNotNull(session);

                        using (var simpleVolume = session.QueryInterface<SimpleAudioVolume>())
                        {
                            Assert.IsNotNull(simpleVolume);

                            float volume = simpleVolume.MasterVolume;
                            simpleVolume.MasterVolume = 1.0f;
                            simpleVolume.MasterVolume = 0.0f;
                            simpleVolume.MasterVolume = volume;

                            bool muted = simpleVolume.IsMuted;
                            simpleVolume.IsMuted = !muted;
                            simpleVolume.IsMuted = muted;
                        }
                    }
                }
            }
        }

        [TestMethod]
        [TestCategory(category)]
        public void CanGetAudioSessionControl2Properties()
        {
            using (var sessionManager = GetDefaultAudioSessionManager2(DataFlow.Render))
            {
                using (var sessionEnumerator = sessionManager.GetSessionEnumerator())
                {
                    Assert.IsNotNull(sessionEnumerator);

                    foreach (var session in sessionEnumerator)
                    {
                        Assert.IsNotNull(session);

                        using (var session2 = session.QueryInterface<AudioSessionControl2>())
                        {
                            Debug.WriteLine("------------------------------------------------------------");

                            Debug.WriteLine("SessionIdentifier: {0}\nSessionInstanceIdentifier: {1}\nProcessID: {2}\nProcessWindowTitle: {3}\nIsSingleProcessSession: {4}\nIsSystemSoundSession: {5}",
                                session2.SessionIdentifier,
                                session2.SessionInstanceIdentifier,
                                session2.ProcessID,
                                session2.Process == null ? String.Empty : session2.Process.MainWindowTitle,
                                session2.IsSingleProcessSession,
                                session2.IsSystemSoundSession);

                            Debug.WriteLine("------------------------------------------------------------");
                        }
                    }
                }
            }
        }

        private AudioSessionManager2 GetDefaultAudioSessionManager2(DataFlow dataFlow)
        {
            using (var enumerator = new MMDeviceEnumerator())
            {
                using (var device = enumerator.GetDefaultAudioEndpoint(dataFlow, Role.Multimedia))
                {
                    Debug.WriteLine("DefaultDevice: " + device.FriendlyName);
                    var sessionManager = AudioSessionManager2.FromMMDevice(device);
                    return sessionManager;
                }
            }
        }

        private void RegisterSessionNotification(AudioSessionManager2 audioSessionManager2,
            IAudioSessionNotification audioSessionNotification)
        {
            if (Thread.CurrentThread.GetApartmentState() == ApartmentState.MTA)
            {
                audioSessionManager2.RegisterSessionNotification(audioSessionNotification);
            }
            else
            {
                using (ManualResetEvent waitHandle = new ManualResetEvent(false))
                {
                    ThreadPool.QueueUserWorkItem(
                        (o) =>
                        {
                            try
                            {
                                audioSessionManager2.RegisterSessionNotification(audioSessionNotification);
                            }
                            finally
                            {
// ReSharper disable once AccessToDisposedClosure
                                waitHandle.Set();
                            }
                        });
                    waitHandle.WaitOne();
                }
            }

// ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            audioSessionManager2.GetSessionEnumerator().ToArray(); //necessary to make it work
        }
    }
}
