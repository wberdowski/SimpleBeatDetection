using CSCore.Utils;
using System;

namespace SimpleBeatDetection.Utils
{
    public abstract class PCMUtils
    {
        public static float[] PCM32ToSamples(byte[] buffer, int length, int channels)
        {
            float[] samples = new float[length / sizeof(float)];
            Buffer.BlockCopy(buffer, 0, samples, 0, length);

            float[] result = new float[length / sizeof(float) / channels];

            for (int i = 0; i < result.Length; i++)
            {
                result[i] = MergeSamples(samples, i * channels, channels);
            }

            return result;
        }

        public static Complex[] PCM32ToComplex(byte[] buffer, int length, int channels)
        {
            float[] samples = new float[length / sizeof(float)];
            Buffer.BlockCopy(buffer, 0, samples, 0, length);

            Complex[] result = new Complex[length / sizeof(float) / channels];

            for (int i = 0; i < result.Length; i++)
            {
                result[i] = new Complex(MergeSamples(samples, i * channels, channels));
            }

            return result;
        }

        public static float MergeSamples(float[] samples, int index, int channelCount)
        {
            float z = 0f;
            for (int i = 0; i < channelCount; i++)
            {
                z += samples[index + i];
            }
            return z / channelCount;
        }
    }
}
