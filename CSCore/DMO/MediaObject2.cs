using System;

namespace CSCore.DMO
{
    public class MediaObject2 : IDisposable
    {
        private const string n = "IMediaObject";

        private IMediaObject mediaObject;

        public IMediaObject NativeObject
        {
            get { return mediaObject; }
        }

        public MediaObject2(IMediaObject mediaObject)
        {
            if (mediaObject == null)
                throw new ArgumentNullException("mediaObject");
            this.mediaObject = mediaObject;
        }

        public int InputStreamCount
        {
            get
            {
                int inputStreamCount, outputStreamCount;
                DmoException.Try(mediaObject.GetStreamCount(out inputStreamCount, out outputStreamCount), n, "GetStreamCount");
                return inputStreamCount;
            }
        }

        public int OutputStreamCount
        {
            get
            {
                int inputStreamCount, outputStreamCount;
                DmoException.Try(mediaObject.GetStreamCount(out inputStreamCount, out outputStreamCount), n, "GetStreamCount");
                return outputStreamCount;
            }
        }

        public InputStreamInfoFlags GetInputStreamInfo(int streamIndex)
        {
            InputStreamInfoFlags flags;
            DmoException.Try(mediaObject.GetInputStreamInfo(streamIndex, out flags), n, "GetInputStreamInfo");
            return flags;
        }

        public OutputStreamInfoFlags GetOutputStreamInfo(int streamIndex)
        {
            OutputStreamInfoFlags flags;
            DmoException.Try(mediaObject.GetOutputStreamInfo(streamIndex, out flags), n, "GetOutputStreamInfo");
            return flags;
        }

        public void SetInputType(int streamIndex, MediaType mediaType, SetTypeFlags flags, out int result)
        {
            result = mediaObject.SetInputType(streamIndex, ref mediaType, flags);
        }

        public void SetInputType(int streamIndex, MediaType mediaType, SetTypeFlags flags)
        {
            int result;
            SetInputType(streamIndex, mediaType, flags, out result);
            DmoException.Try(result, n, "SetInputType");
        }

        public void SetInputType(int streamIndex, WaveFormat waveFormat)
        {
            MediaType mediaType = MediaType.FromWaveFormat(waveFormat);
            SetInputType(streamIndex, mediaType, SetTypeFlags.None);
            mediaType.Free();
        }

        public bool SupportsInputFormat(int streamIndex, WaveFormat waveFormat)
        {
            MediaType mediaType = MediaType.FromWaveFormat(waveFormat);
            bool result = SupportsInputFormat(streamIndex, mediaType);
            mediaType.Free();
            return result;
        }

        public bool SupportsInputFormat(int streamIndex, MediaType mediaType)
        {
            int result;
            SetInputType(streamIndex, mediaType, SetTypeFlags.TestOnly, out result);
            switch ((DmoResult)result)
            {
                case DmoResult.S_OK:
                    return true;

                case DmoResult.DMO_E_INVALIDSTREAMINDEX:
                    throw new ArgumentOutOfRangeException("streamIndex");
                case DmoResult.DMO_E_INVALIDTYPE:
                case DmoResult.DMO_E_TYPE_NOT_SET:
                case DmoResult.DMO_E_NOTACCEPTING:
                case DmoResult.DMO_E_TYPE_NOT_ACCEPTED:
                case DmoResult.DMO_E_NO_MORE_ITEMS:
                default:
                    return false;
            }
        }

        //---
        public void SetOutputType(int streamIndex, MediaType mediaType, SetTypeFlags flags, out int result)
        {
            result = mediaObject.SetOutputType(streamIndex, ref mediaType, flags);
        }

        public void SetOutputType(int streamIndex, MediaType mediaType, SetTypeFlags flags)
        {
            int result;
            SetOutputType(streamIndex, mediaType, flags, out result);
            DmoException.Try(result, n, "SetOutputType");
        }

        public void SetOutputType(int streamIndex, WaveFormat waveFormat)
        {
            MediaType mediaType = MediaType.FromWaveFormat(waveFormat);
            SetOutputType(streamIndex, mediaType, SetTypeFlags.None);
            mediaType.Free();
        }

        public bool SupportsOutputFormat(int streamIndex, WaveFormat waveFormat)
        {
            MediaType mediaType = MediaType.FromWaveFormat(waveFormat);
            bool result = SupportsOutputFormat(streamIndex, mediaType);
            mediaType.Free();
            return result;
        }

        public bool SupportsOutputFormat(int streamIndex, MediaType mediaType)
        {
            int result;
            SetOutputType(streamIndex, mediaType, SetTypeFlags.TestOnly, out result);
            switch ((DmoResult)result)
            {
                case DmoResult.S_OK:
                    return true;

                case DmoResult.DMO_E_INVALIDSTREAMINDEX:
                    throw new ArgumentOutOfRangeException("streamIndex");
                case DmoResult.DMO_E_INVALIDTYPE:
                case DmoResult.DMO_E_TYPE_NOT_SET:
                case DmoResult.DMO_E_NOTACCEPTING:
                case DmoResult.DMO_E_TYPE_NOT_ACCEPTED:
                case DmoResult.DMO_E_NO_MORE_ITEMS:
                default:
                    return false;
            }
        }

        public void ProcessInput(int streamIndex, IMediaBuffer mediaBuffer)
        {
            ProcessInput(streamIndex, mediaBuffer, InputdataBufferFlags.None, 0, 0);
        }

        public void ProcessInput(int streamIndex, IMediaBuffer mediaBuffer, InputdataBufferFlags flags)
        {
            ProcessInput(streamIndex, mediaBuffer, flags, 0, 0);
        }

        public void ProcessInput(int streamIndex, IMediaBuffer mediaBuffer, InputdataBufferFlags flags, long timestamp, long timeduration)
        {
            int result = mediaObject.ProcessInput(streamIndex, mediaBuffer, flags, timestamp, timeduration);
            DmoException.Try(result, n, "ProcessInput");
        }

        //---

        public void ProcessOutput(ProcessOutputFlags flags, params DmoOutputDataBuffer[] buffers)
        {
            ProcessOutput(flags, buffers, buffers.Length);
        }

        public void ProcessOutput(ProcessOutputFlags flags, DmoOutputDataBuffer[] buffers, int bufferCount)
        {
            int tmp;
            int result = mediaObject.ProcessOutput(flags, bufferCount, buffers, out tmp);
            DmoException.Try(result, n, "ProcessOutput");
        }

        public InputStatusFlags GetInputStatus(int streamIndex)
        {
            InputStatusFlags flags;
            int result = mediaObject.GetInputStatus(streamIndex, out flags);
            DmoException.Try(result, n, "GetInputStatus");
            return flags;
        }

        public bool IsReadyForInput(int streamIndex)
        {
            return ((GetInputStatus(streamIndex) & InputStatusFlags.AcceptData) == InputStatusFlags.AcceptData);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                //dispose managed
            }
            if (mediaObject != null)
                System.Runtime.InteropServices.Marshal.ReleaseComObject(mediaObject);

            mediaObject = null;
        }

        ~MediaObject2()
        {
            Dispose(false);
        }
    }
}