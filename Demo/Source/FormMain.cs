using CSCore.SoundIn;
using SimpleBeatDetection.Utils;
using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimpleBeatDetection.Demo
{
    public partial class FormMain : Form
    {
        private SoundEnergyDetector detector;
        private WasapiLoopbackCapture wasapi;

        public FormMain()
        {
            Application.ApplicationExit += Application_ApplicationExit;
            InitializeComponent();

            detector = new SoundEnergyDetector();
            detector.BeatDetected += Detector_BeatDetected;

            wasapi = new WasapiLoopbackCapture(10);
            wasapi.DataAvailable += Wasapi_DataAvailable;
            wasapi.Initialize();
            wasapi.Start();

            chart1.ChartAreas[0].AxisY.Maximum = 1;
            chart1.ChartAreas[0].AxisX.Minimum = 0;
            chart1.ChartAreas[0].AxisX.Maximum = detector.WindowSize;

            chart1.Series[2].Points.AddXY(0, detector.BeatThreshold);
            chart1.Series[2].Points.AddXY(detector.WindowSize, detector.BeatThreshold);
        }

        // Beat detected
        private void Detector_BeatDetected(object sender, BeatDetectedEventArgs e)
        {
            Flash();
        }

        private async void Flash()
        {
            for (int i = 0; i < 255; i += 20)
            {
                beatIndicator.BackColor = Color.FromArgb(i, i, i);
                beatIndicator.Invalidate();
                await Task.Delay(10);
            }
        }

        // Process audio data
        private void Wasapi_DataAvailable(object sender, DataAvailableEventArgs e)
        {
            if (IsHandleCreated && !Disposing && !IsDisposed)
            {
                try
                {
                    Invoke(new Action(() =>
                    {
                        var samples = PCMUtils.PCM32ToSamples(e.Data, e.ByteCount, 2);
                        detector.ProcessData(samples);

                        chart1.Series[0].Points.Clear();
                        chart1.Series[1].Points.Clear();

                        for (int i = 0; i < detector.energyBuffer.Count; i++)
                        {
                            chart1.Series[0].Points.Add(detector.energyBuffer[i]);
                            chart1.Series[1].Points.Add(detector.beatBuffer[i]);
                        }
                    }));
                }
                catch (ObjectDisposedException)
                {

                }
            }
        }

        // Clear resources
        private void Application_ApplicationExit(object sender, EventArgs e)
        {
            wasapi.Stop();
            wasapi.DataAvailable -= Wasapi_DataAvailable;
            wasapi.Dispose();
        }
    }
}
