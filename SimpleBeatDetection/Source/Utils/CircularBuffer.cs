using System.Collections.Generic;

namespace SimpleBeatDetection.Utils
{
    public class CircularBuffer<T> : List<T>
    {
        public int Length { get; set; }

        public CircularBuffer(int length)
        {
            Length = length;
        }

        public new void Add(T item)
        {
            Insert(0, item);

            if (Count > Length)
            {
                RemoveAt(Count - 1);
            }
        }
    }
}
