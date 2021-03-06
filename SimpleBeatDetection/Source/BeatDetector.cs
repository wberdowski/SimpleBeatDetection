﻿using System;

namespace SimpleBeatDetection
{
    public abstract class BeatDetector
    {
        /// <summary>
        /// Beat detection threshold.
        /// </summary>
        public float BeatThreshold { get; set; } = 0.3f;

        /// <summary>
        /// Minimal time between the detected beats in milliseconds.
        /// </summary>
        public int MinBeatGap { get; set; } = 400;

        /// <summary>
        /// Minimal width of the detected beats.
        /// </summary>
        public int MinBeatWidth { get; set; } = 2;

        /// <summary>
        /// Width of the processing window;
        /// </summary>
        public int WindowSize { get; set; } = 50;

        /// <summary>
        /// Sets whether automatic gain control is enabled.
        /// </summary>
        public bool AutoGainEnabled { get; set; } = true;

        public float Gain { get; set; } = float.MaxValue;

        /// <summary>
        /// Time of the latest beat detection.
        /// </summary>
        public DateTime LastBeatTime { get; set; }

        /// <summary>
        /// Fires when a beat is detected.
        /// </summary>
        public event EventHandler<BeatDetectedEventArgs> BeatDetected;

        /// <summary>
        /// Process incomming audio samples and detect beat.
        /// </summary>
        /// <param name="samples">PCM sample buffer.</param>
        public abstract void ProcessData(float[] samples);

        protected void OnBeatDetected()
        {
            BeatDetected?.Invoke(this, new BeatDetectedEventArgs());
        }
    }
}
