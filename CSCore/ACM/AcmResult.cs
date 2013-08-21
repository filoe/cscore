namespace CSCore.ACM
{
    public sealed class AcmResult
    {
        public int SourceUsed { get; private set; }

        public int DestinationUsed { get; private set; }

        public bool HasError { get; private set; }

        public byte[] DestinationBuffer { get; private set; }

        public AcmResult(int sourceUsed, int destinationUsed, bool hasError, byte[] destinationBuffer)
        {
            SourceUsed = sourceUsed;
            DestinationUsed = destinationUsed;
            HasError = hasError;

            DestinationBuffer = destinationBuffer;
        }
    }
}