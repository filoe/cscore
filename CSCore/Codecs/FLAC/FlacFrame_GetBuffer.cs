using System;

namespace CSCore.Codecs.FLAC
{
    public partial class FlacFrame
    {
		private unsafe int GetBufferInternal(ref byte[] buffer)
		{
			short vals;
			int   vali;

			int desiredsize = Header.BlockSize * Header.Channels * ((Header.BitsPerSample + 7) / 2);
            if (buffer == null || buffer.Length < desiredsize)
                buffer = new byte[desiredsize];

            fixed (byte* ptrBuffer = buffer)
            {
                byte* ptr = ptrBuffer;
				switch (Header.BitsPerSample)
				{
#region 8
					case 8:
						switch (Header.Channels)
						{
							case 1:
								for (int i = 0; i < Header.BlockSize; i++)
								{
									*(ptr++) = (byte)(_subFrameData[0].DestinationBuffer[i] + 0x80);
								}
								break;
							case 2:
								for (int i = 0; i < Header.BlockSize; i++)
								{
									*(ptr++) = (byte)(_subFrameData[0].DestinationBuffer[i] + 0x80);
									*(ptr++) = (byte)(_subFrameData[1].DestinationBuffer[i] + 0x80);
								}
								break;
							case 3:
								for (int i = 0; i < Header.BlockSize; i++)
								{
									*(ptr++) = (byte)(_subFrameData[0].DestinationBuffer[i] + 0x80);
									*(ptr++) = (byte)(_subFrameData[1].DestinationBuffer[i] + 0x80);
									*(ptr++) = (byte)(_subFrameData[2].DestinationBuffer[i] + 0x80);
								}
								break;
							case 4:
								for (int i = 0; i < Header.BlockSize; i++)
								{
									*(ptr++) = (byte)(_subFrameData[0].DestinationBuffer[i] + 0x80);
									*(ptr++) = (byte)(_subFrameData[1].DestinationBuffer[i] + 0x80);
									*(ptr++) = (byte)(_subFrameData[2].DestinationBuffer[i] + 0x80);
									*(ptr++) = (byte)(_subFrameData[3].DestinationBuffer[i] + 0x80);
								}
								break;
							case 5:
								for (int i = 0; i < Header.BlockSize; i++)
								{
									*(ptr++) = (byte)(_subFrameData[0].DestinationBuffer[i] + 0x80);
									*(ptr++) = (byte)(_subFrameData[1].DestinationBuffer[i] + 0x80);
									*(ptr++) = (byte)(_subFrameData[2].DestinationBuffer[i] + 0x80);
									*(ptr++) = (byte)(_subFrameData[3].DestinationBuffer[i] + 0x80);
									*(ptr++) = (byte)(_subFrameData[4].DestinationBuffer[i] + 0x80);
								}
								break;
							case 6:
								for (int i = 0; i < Header.BlockSize; i++)
								{
									*(ptr++) = (byte)(_subFrameData[0].DestinationBuffer[i] + 0x80);
									*(ptr++) = (byte)(_subFrameData[1].DestinationBuffer[i] + 0x80);
									*(ptr++) = (byte)(_subFrameData[2].DestinationBuffer[i] + 0x80);
									*(ptr++) = (byte)(_subFrameData[3].DestinationBuffer[i] + 0x80);
									*(ptr++) = (byte)(_subFrameData[4].DestinationBuffer[i] + 0x80);
									*(ptr++) = (byte)(_subFrameData[5].DestinationBuffer[i] + 0x80);
								}
								break;
							case 7:
								for (int i = 0; i < Header.BlockSize; i++)
								{
									*(ptr++) = (byte)(_subFrameData[0].DestinationBuffer[i] + 0x80);
									*(ptr++) = (byte)(_subFrameData[1].DestinationBuffer[i] + 0x80);
									*(ptr++) = (byte)(_subFrameData[2].DestinationBuffer[i] + 0x80);
									*(ptr++) = (byte)(_subFrameData[3].DestinationBuffer[i] + 0x80);
									*(ptr++) = (byte)(_subFrameData[4].DestinationBuffer[i] + 0x80);
									*(ptr++) = (byte)(_subFrameData[5].DestinationBuffer[i] + 0x80);
									*(ptr++) = (byte)(_subFrameData[6].DestinationBuffer[i] + 0x80);
								}
								break;
							case 8:
								for (int i = 0; i < Header.BlockSize; i++)
								{
									*(ptr++) = (byte)(_subFrameData[0].DestinationBuffer[i] + 0x80);
									*(ptr++) = (byte)(_subFrameData[1].DestinationBuffer[i] + 0x80);
									*(ptr++) = (byte)(_subFrameData[2].DestinationBuffer[i] + 0x80);
									*(ptr++) = (byte)(_subFrameData[3].DestinationBuffer[i] + 0x80);
									*(ptr++) = (byte)(_subFrameData[4].DestinationBuffer[i] + 0x80);
									*(ptr++) = (byte)(_subFrameData[5].DestinationBuffer[i] + 0x80);
									*(ptr++) = (byte)(_subFrameData[6].DestinationBuffer[i] + 0x80);
									*(ptr++) = (byte)(_subFrameData[7].DestinationBuffer[i] + 0x80);
								}
								break;
							default:
								for (int i = 0; i < Header.BlockSize; i++)
								{
									for (int c = 0; c < Header.Channels; c++)
									{
										*(ptr++) = (byte)(_subFrameData[c].DestinationBuffer[i] + 0x80);
									}
								}
								break;
						}
						break;
#endregion
#region 16
					case 16:
						switch (Header.Channels)
						{
							case 1:
								for (int i = 0; i < Header.BlockSize; i++)
								{
									vals = (short)(_subFrameData[0].DestinationBuffer[i]);
									*(ptr++) = (byte)(vals & 0xFF);
									*(ptr++) = (byte)((vals >> 8) & 0xFF);

								}
								break;
							case 2:
								for (int i = 0; i < Header.BlockSize; i++)
								{
									vals = (short)(_subFrameData[0].DestinationBuffer[i]);
									*(ptr++) = (byte)(vals & 0xFF);
									*(ptr++) = (byte)((vals >> 8) & 0xFF);

									vals = (short)(_subFrameData[1].DestinationBuffer[i]);
									*(ptr++) = (byte)(vals & 0xFF);
									*(ptr++) = (byte)((vals >> 8) & 0xFF);

								}
								break;
							case 3:
								for (int i = 0; i < Header.BlockSize; i++)
								{
									vals = (short)(_subFrameData[0].DestinationBuffer[i]);
									*(ptr++) = (byte)(vals & 0xFF);
									*(ptr++) = (byte)((vals >> 8) & 0xFF);

									vals = (short)(_subFrameData[1].DestinationBuffer[i]);
									*(ptr++) = (byte)(vals & 0xFF);
									*(ptr++) = (byte)((vals >> 8) & 0xFF);

									vals = (short)(_subFrameData[2].DestinationBuffer[i]);
									*(ptr++) = (byte)(vals & 0xFF);
									*(ptr++) = (byte)((vals >> 8) & 0xFF);

								}
								break;
							case 4:
								for (int i = 0; i < Header.BlockSize; i++)
								{
									vals = (short)(_subFrameData[0].DestinationBuffer[i]);
									*(ptr++) = (byte)(vals & 0xFF);
									*(ptr++) = (byte)((vals >> 8) & 0xFF);

									vals = (short)(_subFrameData[1].DestinationBuffer[i]);
									*(ptr++) = (byte)(vals & 0xFF);
									*(ptr++) = (byte)((vals >> 8) & 0xFF);

									vals = (short)(_subFrameData[2].DestinationBuffer[i]);
									*(ptr++) = (byte)(vals & 0xFF);
									*(ptr++) = (byte)((vals >> 8) & 0xFF);

									vals = (short)(_subFrameData[3].DestinationBuffer[i]);
									*(ptr++) = (byte)(vals & 0xFF);
									*(ptr++) = (byte)((vals >> 8) & 0xFF);

								}
								break;
							case 5:
								for (int i = 0; i < Header.BlockSize; i++)
								{
									vals = (short)(_subFrameData[0].DestinationBuffer[i]);
									*(ptr++) = (byte)(vals & 0xFF);
									*(ptr++) = (byte)((vals >> 8) & 0xFF);

									vals = (short)(_subFrameData[1].DestinationBuffer[i]);
									*(ptr++) = (byte)(vals & 0xFF);
									*(ptr++) = (byte)((vals >> 8) & 0xFF);

									vals = (short)(_subFrameData[2].DestinationBuffer[i]);
									*(ptr++) = (byte)(vals & 0xFF);
									*(ptr++) = (byte)((vals >> 8) & 0xFF);

									vals = (short)(_subFrameData[3].DestinationBuffer[i]);
									*(ptr++) = (byte)(vals & 0xFF);
									*(ptr++) = (byte)((vals >> 8) & 0xFF);

									vals = (short)(_subFrameData[4].DestinationBuffer[i]);
									*(ptr++) = (byte)(vals & 0xFF);
									*(ptr++) = (byte)((vals >> 8) & 0xFF);

								}
								break;
							case 6:
								for (int i = 0; i < Header.BlockSize; i++)
								{
									vals = (short)(_subFrameData[0].DestinationBuffer[i]);
									*(ptr++) = (byte)(vals & 0xFF);
									*(ptr++) = (byte)((vals >> 8) & 0xFF);

									vals = (short)(_subFrameData[1].DestinationBuffer[i]);
									*(ptr++) = (byte)(vals & 0xFF);
									*(ptr++) = (byte)((vals >> 8) & 0xFF);

									vals = (short)(_subFrameData[2].DestinationBuffer[i]);
									*(ptr++) = (byte)(vals & 0xFF);
									*(ptr++) = (byte)((vals >> 8) & 0xFF);

									vals = (short)(_subFrameData[3].DestinationBuffer[i]);
									*(ptr++) = (byte)(vals & 0xFF);
									*(ptr++) = (byte)((vals >> 8) & 0xFF);

									vals = (short)(_subFrameData[4].DestinationBuffer[i]);
									*(ptr++) = (byte)(vals & 0xFF);
									*(ptr++) = (byte)((vals >> 8) & 0xFF);

									vals = (short)(_subFrameData[5].DestinationBuffer[i]);
									*(ptr++) = (byte)(vals & 0xFF);
									*(ptr++) = (byte)((vals >> 8) & 0xFF);

								}
								break;
							case 7:
								for (int i = 0; i < Header.BlockSize; i++)
								{
									vals = (short)(_subFrameData[0].DestinationBuffer[i]);
									*(ptr++) = (byte)(vals & 0xFF);
									*(ptr++) = (byte)((vals >> 8) & 0xFF);

									vals = (short)(_subFrameData[1].DestinationBuffer[i]);
									*(ptr++) = (byte)(vals & 0xFF);
									*(ptr++) = (byte)((vals >> 8) & 0xFF);

									vals = (short)(_subFrameData[2].DestinationBuffer[i]);
									*(ptr++) = (byte)(vals & 0xFF);
									*(ptr++) = (byte)((vals >> 8) & 0xFF);

									vals = (short)(_subFrameData[3].DestinationBuffer[i]);
									*(ptr++) = (byte)(vals & 0xFF);
									*(ptr++) = (byte)((vals >> 8) & 0xFF);

									vals = (short)(_subFrameData[4].DestinationBuffer[i]);
									*(ptr++) = (byte)(vals & 0xFF);
									*(ptr++) = (byte)((vals >> 8) & 0xFF);

									vals = (short)(_subFrameData[5].DestinationBuffer[i]);
									*(ptr++) = (byte)(vals & 0xFF);
									*(ptr++) = (byte)((vals >> 8) & 0xFF);

									vals = (short)(_subFrameData[6].DestinationBuffer[i]);
									*(ptr++) = (byte)(vals & 0xFF);
									*(ptr++) = (byte)((vals >> 8) & 0xFF);

								}
								break;
							case 8:
								for (int i = 0; i < Header.BlockSize; i++)
								{
									vals = (short)(_subFrameData[0].DestinationBuffer[i]);
									*(ptr++) = (byte)(vals & 0xFF);
									*(ptr++) = (byte)((vals >> 8) & 0xFF);

									vals = (short)(_subFrameData[1].DestinationBuffer[i]);
									*(ptr++) = (byte)(vals & 0xFF);
									*(ptr++) = (byte)((vals >> 8) & 0xFF);

									vals = (short)(_subFrameData[2].DestinationBuffer[i]);
									*(ptr++) = (byte)(vals & 0xFF);
									*(ptr++) = (byte)((vals >> 8) & 0xFF);

									vals = (short)(_subFrameData[3].DestinationBuffer[i]);
									*(ptr++) = (byte)(vals & 0xFF);
									*(ptr++) = (byte)((vals >> 8) & 0xFF);

									vals = (short)(_subFrameData[4].DestinationBuffer[i]);
									*(ptr++) = (byte)(vals & 0xFF);
									*(ptr++) = (byte)((vals >> 8) & 0xFF);

									vals = (short)(_subFrameData[5].DestinationBuffer[i]);
									*(ptr++) = (byte)(vals & 0xFF);
									*(ptr++) = (byte)((vals >> 8) & 0xFF);

									vals = (short)(_subFrameData[6].DestinationBuffer[i]);
									*(ptr++) = (byte)(vals & 0xFF);
									*(ptr++) = (byte)((vals >> 8) & 0xFF);

									vals = (short)(_subFrameData[7].DestinationBuffer[i]);
									*(ptr++) = (byte)(vals & 0xFF);
									*(ptr++) = (byte)((vals >> 8) & 0xFF);

								}
								break;
							default:
								for (int i = 0; i < Header.BlockSize; i++)
								{
									for (int c = 0; c < Header.Channels; c++)
									{
										short val = (short)(_subFrameData[c].DestinationBuffer[i]);
										*(ptr++) = (byte)(val & 0xFF);
										*(ptr++) = (byte)((val >> 8) & 0xFF);
									}
								}
								break;
						}
						break;
#endregion
#region 24
					case 24:
						switch (Header.Channels)
						{
							case 1:
								for (int i = 0; i < Header.BlockSize; i++)
								{
									vali = (_subFrameData[0].DestinationBuffer[i]);
									*(ptr++) = (byte)(vali & 0xFF);
									*(ptr++) = (byte)((vali >> 8) & 0xFF);
									*(ptr++) = (byte)((vali >> 16) & 0xFF);

								}
								break;
							case 2:
								for (int i = 0; i < Header.BlockSize; i++)
								{
									vali = (_subFrameData[0].DestinationBuffer[i]);
									*(ptr++) = (byte)(vali & 0xFF);
									*(ptr++) = (byte)((vali >> 8) & 0xFF);
									*(ptr++) = (byte)((vali >> 16) & 0xFF);

									vali = (_subFrameData[1].DestinationBuffer[i]);
									*(ptr++) = (byte)(vali & 0xFF);
									*(ptr++) = (byte)((vali >> 8) & 0xFF);
									*(ptr++) = (byte)((vali >> 16) & 0xFF);

								}
								break;
							case 3:
								for (int i = 0; i < Header.BlockSize; i++)
								{
									vali = (_subFrameData[0].DestinationBuffer[i]);
									*(ptr++) = (byte)(vali & 0xFF);
									*(ptr++) = (byte)((vali >> 8) & 0xFF);
									*(ptr++) = (byte)((vali >> 16) & 0xFF);

									vali = (_subFrameData[1].DestinationBuffer[i]);
									*(ptr++) = (byte)(vali & 0xFF);
									*(ptr++) = (byte)((vali >> 8) & 0xFF);
									*(ptr++) = (byte)((vali >> 16) & 0xFF);

									vali = (_subFrameData[2].DestinationBuffer[i]);
									*(ptr++) = (byte)(vali & 0xFF);
									*(ptr++) = (byte)((vali >> 8) & 0xFF);
									*(ptr++) = (byte)((vali >> 16) & 0xFF);

								}
								break;
							case 4:
								for (int i = 0; i < Header.BlockSize; i++)
								{
									vali = (_subFrameData[0].DestinationBuffer[i]);
									*(ptr++) = (byte)(vali & 0xFF);
									*(ptr++) = (byte)((vali >> 8) & 0xFF);
									*(ptr++) = (byte)((vali >> 16) & 0xFF);

									vali = (_subFrameData[1].DestinationBuffer[i]);
									*(ptr++) = (byte)(vali & 0xFF);
									*(ptr++) = (byte)((vali >> 8) & 0xFF);
									*(ptr++) = (byte)((vali >> 16) & 0xFF);

									vali = (_subFrameData[2].DestinationBuffer[i]);
									*(ptr++) = (byte)(vali & 0xFF);
									*(ptr++) = (byte)((vali >> 8) & 0xFF);
									*(ptr++) = (byte)((vali >> 16) & 0xFF);

									vali = (_subFrameData[3].DestinationBuffer[i]);
									*(ptr++) = (byte)(vali & 0xFF);
									*(ptr++) = (byte)((vali >> 8) & 0xFF);
									*(ptr++) = (byte)((vali >> 16) & 0xFF);

								}
								break;
							case 5:
								for (int i = 0; i < Header.BlockSize; i++)
								{
									vali = (_subFrameData[0].DestinationBuffer[i]);
									*(ptr++) = (byte)(vali & 0xFF);
									*(ptr++) = (byte)((vali >> 8) & 0xFF);
									*(ptr++) = (byte)((vali >> 16) & 0xFF);

									vali = (_subFrameData[1].DestinationBuffer[i]);
									*(ptr++) = (byte)(vali & 0xFF);
									*(ptr++) = (byte)((vali >> 8) & 0xFF);
									*(ptr++) = (byte)((vali >> 16) & 0xFF);

									vali = (_subFrameData[2].DestinationBuffer[i]);
									*(ptr++) = (byte)(vali & 0xFF);
									*(ptr++) = (byte)((vali >> 8) & 0xFF);
									*(ptr++) = (byte)((vali >> 16) & 0xFF);

									vali = (_subFrameData[3].DestinationBuffer[i]);
									*(ptr++) = (byte)(vali & 0xFF);
									*(ptr++) = (byte)((vali >> 8) & 0xFF);
									*(ptr++) = (byte)((vali >> 16) & 0xFF);

									vali = (_subFrameData[4].DestinationBuffer[i]);
									*(ptr++) = (byte)(vali & 0xFF);
									*(ptr++) = (byte)((vali >> 8) & 0xFF);
									*(ptr++) = (byte)((vali >> 16) & 0xFF);

								}
								break;
							case 6:
								for (int i = 0; i < Header.BlockSize; i++)
								{
									vali = (_subFrameData[0].DestinationBuffer[i]);
									*(ptr++) = (byte)(vali & 0xFF);
									*(ptr++) = (byte)((vali >> 8) & 0xFF);
									*(ptr++) = (byte)((vali >> 16) & 0xFF);

									vali = (_subFrameData[1].DestinationBuffer[i]);
									*(ptr++) = (byte)(vali & 0xFF);
									*(ptr++) = (byte)((vali >> 8) & 0xFF);
									*(ptr++) = (byte)((vali >> 16) & 0xFF);

									vali = (_subFrameData[2].DestinationBuffer[i]);
									*(ptr++) = (byte)(vali & 0xFF);
									*(ptr++) = (byte)((vali >> 8) & 0xFF);
									*(ptr++) = (byte)((vali >> 16) & 0xFF);

									vali = (_subFrameData[3].DestinationBuffer[i]);
									*(ptr++) = (byte)(vali & 0xFF);
									*(ptr++) = (byte)((vali >> 8) & 0xFF);
									*(ptr++) = (byte)((vali >> 16) & 0xFF);

									vali = (_subFrameData[4].DestinationBuffer[i]);
									*(ptr++) = (byte)(vali & 0xFF);
									*(ptr++) = (byte)((vali >> 8) & 0xFF);
									*(ptr++) = (byte)((vali >> 16) & 0xFF);

									vali = (_subFrameData[5].DestinationBuffer[i]);
									*(ptr++) = (byte)(vali & 0xFF);
									*(ptr++) = (byte)((vali >> 8) & 0xFF);
									*(ptr++) = (byte)((vali >> 16) & 0xFF);

								}
								break;
							case 7:
								for (int i = 0; i < Header.BlockSize; i++)
								{
									vali = (_subFrameData[0].DestinationBuffer[i]);
									*(ptr++) = (byte)(vali & 0xFF);
									*(ptr++) = (byte)((vali >> 8) & 0xFF);
									*(ptr++) = (byte)((vali >> 16) & 0xFF);

									vali = (_subFrameData[1].DestinationBuffer[i]);
									*(ptr++) = (byte)(vali & 0xFF);
									*(ptr++) = (byte)((vali >> 8) & 0xFF);
									*(ptr++) = (byte)((vali >> 16) & 0xFF);

									vali = (_subFrameData[2].DestinationBuffer[i]);
									*(ptr++) = (byte)(vali & 0xFF);
									*(ptr++) = (byte)((vali >> 8) & 0xFF);
									*(ptr++) = (byte)((vali >> 16) & 0xFF);

									vali = (_subFrameData[3].DestinationBuffer[i]);
									*(ptr++) = (byte)(vali & 0xFF);
									*(ptr++) = (byte)((vali >> 8) & 0xFF);
									*(ptr++) = (byte)((vali >> 16) & 0xFF);

									vali = (_subFrameData[4].DestinationBuffer[i]);
									*(ptr++) = (byte)(vali & 0xFF);
									*(ptr++) = (byte)((vali >> 8) & 0xFF);
									*(ptr++) = (byte)((vali >> 16) & 0xFF);

									vali = (_subFrameData[5].DestinationBuffer[i]);
									*(ptr++) = (byte)(vali & 0xFF);
									*(ptr++) = (byte)((vali >> 8) & 0xFF);
									*(ptr++) = (byte)((vali >> 16) & 0xFF);

									vali = (_subFrameData[6].DestinationBuffer[i]);
									*(ptr++) = (byte)(vali & 0xFF);
									*(ptr++) = (byte)((vali >> 8) & 0xFF);
									*(ptr++) = (byte)((vali >> 16) & 0xFF);

								}
								break;
							case 8:
								for (int i = 0; i < Header.BlockSize; i++)
								{
									vali = (_subFrameData[0].DestinationBuffer[i]);
									*(ptr++) = (byte)(vali & 0xFF);
									*(ptr++) = (byte)((vali >> 8) & 0xFF);
									*(ptr++) = (byte)((vali >> 16) & 0xFF);

									vali = (_subFrameData[1].DestinationBuffer[i]);
									*(ptr++) = (byte)(vali & 0xFF);
									*(ptr++) = (byte)((vali >> 8) & 0xFF);
									*(ptr++) = (byte)((vali >> 16) & 0xFF);

									vali = (_subFrameData[2].DestinationBuffer[i]);
									*(ptr++) = (byte)(vali & 0xFF);
									*(ptr++) = (byte)((vali >> 8) & 0xFF);
									*(ptr++) = (byte)((vali >> 16) & 0xFF);

									vali = (_subFrameData[3].DestinationBuffer[i]);
									*(ptr++) = (byte)(vali & 0xFF);
									*(ptr++) = (byte)((vali >> 8) & 0xFF);
									*(ptr++) = (byte)((vali >> 16) & 0xFF);

									vali = (_subFrameData[4].DestinationBuffer[i]);
									*(ptr++) = (byte)(vali & 0xFF);
									*(ptr++) = (byte)((vali >> 8) & 0xFF);
									*(ptr++) = (byte)((vali >> 16) & 0xFF);

									vali = (_subFrameData[5].DestinationBuffer[i]);
									*(ptr++) = (byte)(vali & 0xFF);
									*(ptr++) = (byte)((vali >> 8) & 0xFF);
									*(ptr++) = (byte)((vali >> 16) & 0xFF);

									vali = (_subFrameData[6].DestinationBuffer[i]);
									*(ptr++) = (byte)(vali & 0xFF);
									*(ptr++) = (byte)((vali >> 8) & 0xFF);
									*(ptr++) = (byte)((vali >> 16) & 0xFF);

									vali = (_subFrameData[7].DestinationBuffer[i]);
									*(ptr++) = (byte)(vali & 0xFF);
									*(ptr++) = (byte)((vali >> 8) & 0xFF);
									*(ptr++) = (byte)((vali >> 16) & 0xFF);

								}
								break;
							default:
								for (int i = 0; i < Header.BlockSize; i++)
								{
									for (int c = 0; c < Header.Channels; c++)
									{
										int val = (_subFrameData[c].DestinationBuffer[i]);
										*(ptr++) = (byte)(val & 0xFF);
										*(ptr++) = (byte)((val >> 8) & 0xFF);
										*(ptr++) = (byte)((val >> 16) & 0xFF);
									}
								}
								break;
						}
						break;
#endregion
					default: //default bits per sample
						throw new FlacException(String.Format("FlacFrame::GetBuffer: Invalid BitsPerSample value: {0}", Header.BitsPerSample), FlacLayer.Frame);
				}
				return (int)(ptr - ptrBuffer);
			}
		}
    }
}