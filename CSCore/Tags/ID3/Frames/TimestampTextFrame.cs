using System;
using System.Collections.Generic;

namespace CSCore.Tags.ID3.Frames
{
    public class TimestampTextFrame : MultiStringTextFrame
    {
        private List<DateTime> _dateTimes;

        public List<DateTime> DateTimes
        {
            get { return _dateTimes ?? (_dateTimes = new List<DateTime>()); }
        }

        public TimestampTextFrame(FrameHeader header)
            : base(header)
        {
        }

        protected override void Decode(byte[] content)
        {
            base.Decode(content);

            foreach (var str in Strings)
            {
                DateTime result;
                if (str == null)
                    throw new ID3Exception("Timestamp-String is null");
                if (String.IsNullOrEmpty(str))
                {
                    result = DateTime.MinValue;
                }
                else
                {
                    var format = GetFormatString(str.Length);
                    try
                    {
                        result = DateTime.ParseExact(str, format,
                            System.Globalization.DateTimeFormatInfo.InvariantInfo,
                            System.Globalization.DateTimeStyles.None);
                    }
                    catch (FormatException ex)
                    {
                        throw new ID3Exception(String.Format("Could not parse [{0}] with format [{1}] to Datetime. For details see Innerexception",
                            str, format), ex);
                    }
                }

                DateTimes.Add(result);
            }
        }

        /// <summary>
        /// Gets the formatstring of the timestamp
        /// </summary>
        /// <param name="length">length of the string which has to be parsed</param>
        /// <returns></returns>
        public static string GetFormatString(int length)
        {
            switch (length)
            {
                case 4:
                    return "yyyy";

                case 7:
                    return "yyyy-MM";

                case 10:
                    return "yyyy-MM-dd";

                case 13:
                    return "yyyy-MM-ddTHH";

                case 16:
                    return "yyyy-MM-ddTHH:mm";

                case 19:
                    return "yyyy-MM-ddTHH:mm:ss";

                default:
                    throw new ID3Exception("Invalid length of timestamp");
            }
        }
    }
}