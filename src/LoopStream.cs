using NAudio.Wave;

namespace SlotMachineGame
{
    public class LoopStream : WaveStream
    {
        private readonly WaveStream sourceStream;
        private long loopCount;
        private long loopStartPosition;

        public LoopStream(WaveStream sourceStream, int loopCount)
        {
            this.sourceStream = sourceStream ?? throw new ArgumentNullException(nameof(sourceStream));
            this.loopCount = loopCount;
            this.loopStartPosition = 0;
        }

        public override WaveFormat WaveFormat => sourceStream.WaveFormat;

        public override long Length => sourceStream.Length * loopCount;

        public override long Position
        {
            get => sourceStream.Position - loopStartPosition;
            set => sourceStream.Position = value + loopStartPosition;
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int totalBytesRead = 0;
            while (count > 0)
            {
                int bytesRead = sourceStream.Read(buffer, offset, count);
                if (bytesRead == 0)
                {
                    if (loopCount == 0)
                        break;

                    sourceStream.Position = 0;
                    loopStartPosition = sourceStream.Position;
                    loopCount--;
                }
                else
                {
                    count -= bytesRead;
                    offset += bytesRead;
                    totalBytesRead += bytesRead;
                }
            }
            return totalBytesRead;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            sourceStream.Dispose();
        }
    }
}
