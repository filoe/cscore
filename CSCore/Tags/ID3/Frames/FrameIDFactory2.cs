//#define generate

using System;
using System.Collections.Generic;
using System.Linq;

namespace CSCore.Tags.ID3.Frames
{
    public static class FrameIDFactory2
    {
        private static List<ID3v2FrameEntry> _entries = new List<ID3v2FrameEntry>();

        public static List<ID3v2FrameEntry> Frames
        {
            get { return _entries; }
        }

        public static ID3v2FrameEntry GetFrameEntry(string id, ID3Version version)
        {
            switch (version)
            {
                case ID3Version.ID3v1:
                    throw new ArgumentException("version");
                case ID3Version.ID3v2_2:
                    return Frames.Where((x) => !String.IsNullOrEmpty(x.ID3v2ID) && x.ID3v2ID.Equals(id, StringComparison.InvariantCultureIgnoreCase)).First();

                case ID3Version.ID3v2_3:
                    return Frames.Where((x) => !String.IsNullOrEmpty(x.ID3v3ID) && x.ID3v3ID.Equals(id, StringComparison.InvariantCultureIgnoreCase)).First();

                case ID3Version.ID3v2_4:
                    return Frames.Where((x) => !String.IsNullOrEmpty(x.ID3v4ID) && x.ID3v4ID.Equals(id, StringComparison.InvariantCultureIgnoreCase)).First();

                default:
                    throw new ArgumentException("Unknown version");
            }
        }

        static FrameIDFactory2()
        {
            CreateEntries();
#if generate
            var writer = new System.IO.StreamWriter(@"C:\Temp\table.txt");
            _entries = new List<ID3v2FrameEntry>();

            var elements = Enum.GetNames(typeof(ID3v2FrameID));
            foreach (var element in elements)
            {
                var id = Enum.Parse(typeof(ID3v2FrameID), element);
                var id3v4ID = ID3v2FrameIDFactory.GetID((ID3v2FrameID)id, ID3Version.ID3v2_4);
                string id3v3ID;
                try
                {
                    id3v3ID = ID3v2FrameIDFactory.GetID((ID3v2FrameID)id, ID3Version.ID3v2_3);
                }
                catch (Exception)
                {
                    id3v3ID = null;
                }
                string id3v2ID;
                try
                {
                    id3v2ID = ID3v2FrameIDFactory.GetID((ID3v2FrameID)id, ID3Version.ID3v2_2);
                }
                catch (Exception)
                {
                    id3v2ID = null;
                }

                StringBuilder builder = new StringBuilder();
                builder.AppendLine("                entry = new ID3v2FrameEntry()");
                builder.AppendLine("                {");
                builder.AppendLine(String.Format("                    ID = ID3v2FrameID.{0},", ((ID3v2FrameID)id).ToString()));
                builder.AppendLine(String.Format("                    ID3v4ID = {0},", id3v4ID == null ? "null" : "\"" + id3v4ID + "\""));
                builder.AppendLine(String.Format("                    ID3v3ID = {0},", id3v3ID == null ? "null" : "\"" + id3v3ID + "\""));
                builder.AppendLine(String.Format("                    ID3v2ID = {0},", id3v2ID == null ? "null" : "\"" + id3v2ID + "\""));
                builder.AppendLine(String.Format("                    Desc = \"{0}\"", element));
                builder.AppendLine("                };");
                builder.AppendLine("                _entries.Add(entry);");
                writer.WriteLine(builder.ToString());
                writer.WriteLine();
                writer.WriteLine();
            }
            writer.Flush();
            writer.Dispose();
#endif
        }

        private static void CreateEntries()
        {
            //----

            var entry = new ID3v2FrameEntry()
            {
                ID = FrameID.RecordingTime,
                ID3v4ID = "TDRC",
                ID3v3ID = null,
                ID3v2ID = null,
                Desc = "RecordingTime"
            };
            _entries.Add(entry);

            entry = new ID3v2FrameEntry()
            {
                ID = FrameID.OriginalReleaseTime,
                ID3v4ID = "TDOR",
                ID3v3ID = null,
                ID3v2ID = null,
                Desc = "OriginalReleaseTime"
            };
            _entries.Add(entry);

            entry = new ID3v2FrameEntry()
            {
                ID = FrameID.EncodingTime,
                ID3v4ID = "TDEN",
                ID3v3ID = null,
                ID3v2ID = null,
                Desc = "EncodingTime"
            };
            _entries.Add(entry);

            entry = new ID3v2FrameEntry()
            {
                ID = FrameID.TaggingTime,
                ID3v4ID = "TDTG",
                ID3v3ID = null,
                ID3v2ID = null,
                Desc = "TaggingTime"
            };
            _entries.Add(entry);

            entry = new ID3v2FrameEntry()
            {
                ID = FrameID.ReleaseTime,
                ID3v4ID = "TDRL",
                ID3v3ID = null,
                ID3v2ID = null,
                Desc = "ReleaseTime"
            };
            _entries.Add(entry);

            entry = new ID3v2FrameEntry()
            {
                ID = FrameID.EncryptedMetaData,
                ID3v4ID = null,
                ID3v3ID = null,
                ID3v2ID = "CRM",
                Desc = "EncryptedMetaData"
            };
            _entries.Add(entry);

            entry = new ID3v2FrameEntry()
            {
                ID = FrameID.AudioSeekPointIndex,
                ID3v4ID = "ASPI",
                ID3v3ID = null,
                ID3v2ID = null,
                Desc = "AudioSeekPointIndex"
            };
            _entries.Add(entry);

            entry = new ID3v2FrameEntry()
            {
                ID = FrameID.RelativeVolumeAdjustmentOld,
                ID3v4ID = null,
                ID3v3ID = "RVAD",
                ID3v2ID = "RVA",
                Desc = "RelativeVolumeAdjustmentOld"
            };
            _entries.Add(entry);

            entry = new ID3v2FrameEntry()
            {
                ID = FrameID.RelativeVolumeAdjustment,
                ID3v4ID = "RVA2",
                ID3v3ID = null,
                ID3v2ID = null,
                Desc = "RelativeVolumeAdjustment"
            };
            _entries.Add(entry);

            entry = new ID3v2FrameEntry()
            {
                ID = FrameID.EqualizationOld,
                ID3v4ID = null,
                ID3v3ID = "EQUA",
                ID3v2ID = "EQU",
                Desc = "EqualizationOld"
            };
            _entries.Add(entry);

            //----

            entry = new ID3v2FrameEntry()
            {
                ID = FrameID.AudioEnctryption,
                ID3v4ID = "AENC",
                ID3v3ID = "AENC",
                ID3v2ID = "CRA",
                Desc = "AudioEnctryption"
            };
            _entries.Add(entry);

            entry = new ID3v2FrameEntry()
            {
                ID = FrameID.AttachedPicutre,
                ID3v4ID = "APIC",
                ID3v3ID = "APIC",
                ID3v2ID = "PIC",
                Desc = "AttachedPicutre"
            };
            _entries.Add(entry);

            entry = new ID3v2FrameEntry()
            {
                ID = FrameID.Comments,
                ID3v4ID = "COMM",
                ID3v3ID = "COMM",
                ID3v2ID = "COM",
                Desc = "Comments"
            };
            _entries.Add(entry);

            entry = new ID3v2FrameEntry()
            {
                ID = FrameID.CommercialFrame,
                ID3v4ID = "COMR",
                ID3v3ID = "COMR",
                ID3v2ID = null,
                Desc = "CommercialFrame"
            };
            _entries.Add(entry);

            entry = new ID3v2FrameEntry()
            {
                ID = FrameID.EncryptionMethodRegistration,
                ID3v4ID = "ENCR",
                ID3v3ID = "ENCR",
                ID3v2ID = null,
                Desc = "EncryptionMethodRegistration"
            };
            _entries.Add(entry);

            entry = new ID3v2FrameEntry()
            {
                ID = FrameID.Equalization,
                ID3v4ID = "EQU2",
                ID3v3ID = null,
                ID3v2ID = null,
                Desc = "Equalization"
            };
            _entries.Add(entry);

            entry = new ID3v2FrameEntry()
            {
                ID = FrameID.EventTimeingCodes,
                ID3v4ID = "ETCO",
                ID3v3ID = "ETCO",
                ID3v2ID = "ETC",
                Desc = "EventTimeingCodes"
            };
            _entries.Add(entry);

            entry = new ID3v2FrameEntry()
            {
                ID = FrameID.GeneralEncapsulatedObject,
                ID3v4ID = "GEOB",
                ID3v3ID = "GEOB",
                ID3v2ID = "GEO",
                Desc = "GeneralEncapsulatedObject"
            };
            _entries.Add(entry);

            entry = new ID3v2FrameEntry()
            {
                ID = FrameID.GroupIdentificationRegistration,
                ID3v4ID = "GRID",
                ID3v3ID = "GRID",
                ID3v2ID = null,
                Desc = "GroupIdentificationRegistration"
            };
            _entries.Add(entry);

            entry = new ID3v2FrameEntry()
            {
                ID = FrameID.InvolvedPeopleList,
                ID3v4ID = "TIPL",
                ID3v3ID = null,
                ID3v2ID = null,
                Desc = "InvolvedPeopleList"
            };
            _entries.Add(entry);

            entry = new ID3v2FrameEntry()
            {
                ID = FrameID.InvolvedPeopleListOld,
                ID3v4ID = null,
                ID3v3ID = "IPLS",
                ID3v2ID = "IPL",
                Desc = "InvolvedPeopleListOld"
            };

            entry = new ID3v2FrameEntry()
            {
                ID = FrameID.LinkedInformation,
                ID3v4ID = "LINK",
                ID3v3ID = "LINK",
                ID3v2ID = "LNK",
                Desc = "LinkedInformation"
            };
            _entries.Add(entry);

            entry = new ID3v2FrameEntry()
            {
                ID = FrameID.MusicCDIdentifier,
                ID3v4ID = "MCDI",
                ID3v3ID = "MCDI",
                ID3v2ID = "MCI",
                Desc = "MusicCDIdentifier"
            };
            _entries.Add(entry);

            entry = new ID3v2FrameEntry()
            {
                ID = FrameID.MPEGLocationLookupTable,
                ID3v4ID = "MLLT",
                ID3v3ID = "MLLT",
                ID3v2ID = "MLL",
                Desc = "MPEGLocationLookupTable"
            };
            _entries.Add(entry);

            entry = new ID3v2FrameEntry()
            {
                ID = FrameID.OwnershipFrame,
                ID3v4ID = "OWNE",
                ID3v3ID = "OWNE",
                ID3v2ID = null,
                Desc = "OwnershipFrame"
            };
            _entries.Add(entry);

            entry = new ID3v2FrameEntry()
            {
                ID = FrameID.PrivateFrame,
                ID3v4ID = "PRIV",
                ID3v3ID = "PRIV",
                ID3v2ID = null,
                Desc = "PrivateFrame"
            };
            _entries.Add(entry);

            entry = new ID3v2FrameEntry()
            {
                ID = FrameID.PlayCounter,
                ID3v4ID = "PCNT",
                ID3v3ID = "PCNT",
                ID3v2ID = "CNT",
                Desc = "PlayCounter"
            };
            _entries.Add(entry);

            entry = new ID3v2FrameEntry()
            {
                ID = FrameID.Popularimeter,
                ID3v4ID = "POPM",
                ID3v3ID = "POPM",
                ID3v2ID = "POP",
                Desc = "Popularimeter"
            };
            _entries.Add(entry);

            entry = new ID3v2FrameEntry()
            {
                ID = FrameID.PositionSynchronisationFrame,
                ID3v4ID = "POSS",
                ID3v3ID = "POSS",
                ID3v2ID = null,
                Desc = "PositionSynchronisationFrame"
            };
            _entries.Add(entry);

            entry = new ID3v2FrameEntry()
            {
                ID = FrameID.RecommendedBufferSize,
                ID3v4ID = "RBUF",
                ID3v3ID = "RBUF",
                ID3v2ID = "BUF",
                Desc = "RecommendedBufferSize"
            };
            _entries.Add(entry);

            entry = new ID3v2FrameEntry()
            {
                ID = FrameID.RelativeVolumeAdjustment,
                ID3v4ID = null,
                ID3v3ID = "RVAD",
                ID3v2ID = "RVA",
                Desc = "RelativeVolumeAdjustment"
            };
            _entries.Add(entry);

            entry = new ID3v2FrameEntry()
            {
                ID = FrameID.Reverb,
                ID3v4ID = "RVRB",
                ID3v3ID = "RVRB",
                ID3v2ID = "REV",
                Desc = "Reverb"
            };
            _entries.Add(entry);

            entry = new ID3v2FrameEntry()
            {
                ID = FrameID.SynchronizedLyrics,
                ID3v4ID = "SYLT",
                ID3v3ID = "SYLT",
                ID3v2ID = "SLT",
                Desc = "SynchronizedLyrics"
            };
            _entries.Add(entry);

            entry = new ID3v2FrameEntry()
            {
                ID = FrameID.SynchronizedTempoCodes,
                ID3v4ID = "SYTC",
                ID3v3ID = "SYTC",
                ID3v2ID = "STC",
                Desc = "SynchronizedTempoCodes"
            };
            _entries.Add(entry);

            entry = new ID3v2FrameEntry()
            {
                ID = FrameID.Album,
                ID3v4ID = "TALB",
                ID3v3ID = "TALB",
                ID3v2ID = "TAL",
                Desc = "Album"
            };
            _entries.Add(entry);

            entry = new ID3v2FrameEntry()
            {
                ID = FrameID.BeatsPerMinute,
                ID3v4ID = "TBPM",
                ID3v3ID = "TBPM",
                ID3v2ID = "TBP",
                Desc = "BeatsPerMinute"
            };
            _entries.Add(entry);

            entry = new ID3v2FrameEntry()
            {
                ID = FrameID.Composer,
                ID3v4ID = "TCOM",
                ID3v3ID = "TCOM",
                ID3v2ID = "TCM",
                Desc = "Composer"
            };
            _entries.Add(entry);

            entry = new ID3v2FrameEntry()
            {
                ID = FrameID.ContentType,
                ID3v4ID = "TCON",
                ID3v3ID = "TCON",
                ID3v2ID = "TCO",
                Desc = "ContentType"
            };
            _entries.Add(entry);

            entry = new ID3v2FrameEntry()
            {
                ID = FrameID.CopyrightMessage,
                ID3v4ID = "TCOP",
                ID3v3ID = "TCOP",
                ID3v2ID = "TCP",
                Desc = "CopyrightMessage"
            };
            _entries.Add(entry);

            entry = new ID3v2FrameEntry()
            {
                ID = FrameID.Date,
                ID3v4ID = null,
                ID3v3ID = "TDAT",
                ID3v2ID = "TDA",
                Desc = "Date"
            };
            _entries.Add(entry);

            entry = new ID3v2FrameEntry()
            {
                ID = FrameID.PlaylistDelay,
                ID3v4ID = "TDLY",
                ID3v3ID = "TDLY",
                ID3v2ID = "TDY",
                Desc = "PlaylistDelay"
            };
            _entries.Add(entry);

            entry = new ID3v2FrameEntry()
            {
                ID = FrameID.EncodedBy,
                ID3v4ID = "TENC",
                ID3v3ID = "TENC",
                ID3v2ID = "TEN",
                Desc = "EncodedBy"
            };
            _entries.Add(entry);

            entry = new ID3v2FrameEntry()
            {
                ID = FrameID.TextWriter,
                ID3v4ID = "TEXT",
                ID3v3ID = "TEXT",
                ID3v2ID = "TXT",
                Desc = "TextWriter"
            };
            _entries.Add(entry);

            entry = new ID3v2FrameEntry()
            {
                ID = FrameID.FileType,
                ID3v4ID = "TFLT",
                ID3v3ID = "TFLT",
                ID3v2ID = "TFT",
                Desc = "FileType"
            };
            _entries.Add(entry);

            entry = new ID3v2FrameEntry()
            {
                ID = FrameID.Time,
                ID3v4ID = null,
                ID3v3ID = "TIME",
                ID3v2ID = "TIM",
                Desc = "Time"
            };
            _entries.Add(entry);

            entry = new ID3v2FrameEntry()
            {
                ID = FrameID.ContentGroupDescription,
                ID3v4ID = "TIT1",
                ID3v3ID = "TIT1",
                ID3v2ID = "TT1",
                Desc = "ContentGroupDescription"
            };
            _entries.Add(entry);

            entry = new ID3v2FrameEntry()
            {
                ID = FrameID.Title,
                ID3v4ID = "TIT2",
                ID3v3ID = "TIT2",
                ID3v2ID = "TT2",
                Desc = "Title"
            };
            _entries.Add(entry);

            entry = new ID3v2FrameEntry()
            {
                ID = FrameID.Subtitle,
                ID3v4ID = "TIT3",
                ID3v3ID = "TIT3",
                ID3v2ID = "TT3",
                Desc = "Subtitle"
            };
            _entries.Add(entry);

            entry = new ID3v2FrameEntry()
            {
                ID = FrameID.InitialKey,
                ID3v4ID = "TKEY",
                ID3v3ID = "TKEY",
                ID3v2ID = "TKE",
                Desc = "InitialKey"
            };
            _entries.Add(entry);

            entry = new ID3v2FrameEntry()
            {
                ID = FrameID.Languages,
                ID3v4ID = "TLAN",
                ID3v3ID = "TLAN",
                ID3v2ID = "TLA",
                Desc = "Languages"
            };
            _entries.Add(entry);

            entry = new ID3v2FrameEntry()
            {
                ID = FrameID.Length,
                ID3v4ID = "TLEN",
                ID3v3ID = "TLEN",
                ID3v2ID = "TLE",
                Desc = "Length"
            };
            _entries.Add(entry);

            entry = new ID3v2FrameEntry()
            {
                ID = FrameID.MediaType,
                ID3v4ID = "TMED",
                ID3v3ID = "TMED",
                ID3v2ID = "TMT",
                Desc = "MediaType"
            };
            _entries.Add(entry);

            entry = new ID3v2FrameEntry()
            {
                ID = FrameID.OriginalAlbum,
                ID3v4ID = "TOAL",
                ID3v3ID = "TOAL",
                ID3v2ID = "TOT",
                Desc = "OriginalAlbum"
            };
            _entries.Add(entry);

            entry = new ID3v2FrameEntry()
            {
                ID = FrameID.OriginalFileName,
                ID3v4ID = "TOFN",
                ID3v3ID = "TOFN",
                ID3v2ID = "TOF",
                Desc = "OriginalFileName"
            };
            _entries.Add(entry);

            entry = new ID3v2FrameEntry()
            {
                ID = FrameID.OriginalTextWriter,
                ID3v4ID = "TOLY",
                ID3v3ID = "TOLY",
                ID3v2ID = "TOL",
                Desc = "OriginalTextWriter"
            };
            _entries.Add(entry);

            entry = new ID3v2FrameEntry()
            {
                ID = FrameID.OriginalArtist,
                ID3v4ID = "TOPE",
                ID3v3ID = "TOPE",
                ID3v2ID = "TOA",
                Desc = "OriginalArtist"
            };
            _entries.Add(entry);

            entry = new ID3v2FrameEntry()
            {
                ID = FrameID.OriginalReleaseYear,
                ID3v4ID = "TORY", //not supported in specification
                ID3v3ID = "TORY",
                ID3v2ID = "TOR",
                Desc = "OriginalReleaseYear"
            };
            _entries.Add(entry);

            entry = new ID3v2FrameEntry()
            {
                ID = FrameID.FileOwner,
                ID3v4ID = "TOWN",
                ID3v3ID = "TOWN",
                ID3v2ID = null,
                Desc = "FileOwner"
            };
            _entries.Add(entry);

            entry = new ID3v2FrameEntry()
            {
                ID = FrameID.LeadPerformers,
                ID3v4ID = "TPE1",
                ID3v3ID = "TPE1",
                ID3v2ID = "TP1",
                Desc = "LeadPerformers"
            };
            _entries.Add(entry);

            entry = new ID3v2FrameEntry()
            {
                ID = FrameID.Band,
                ID3v4ID = "TPE2",
                ID3v3ID = "TPE2",
                ID3v2ID = "TP2",
                Desc = "Band"
            };
            _entries.Add(entry);

            entry = new ID3v2FrameEntry()
            {
                ID = FrameID.Conductor,
                ID3v4ID = "TPE3",
                ID3v3ID = "TPE3",
                ID3v2ID = "TP3",
                Desc = "Conductor"
            };
            _entries.Add(entry);

            entry = new ID3v2FrameEntry()
            {
                ID = FrameID.Interpreted,
                ID3v4ID = "TPE4",
                ID3v3ID = "TPE4",
                ID3v2ID = "TP4",
                Desc = "Interpreted"
            };
            _entries.Add(entry);

            entry = new ID3v2FrameEntry()
            {
                ID = FrameID.PartOfASet,
                ID3v4ID = "TPOS",
                ID3v3ID = "TPOS",
                ID3v2ID = "TPA",
                Desc = "PartOfASet"
            };
            _entries.Add(entry);

            entry = new ID3v2FrameEntry()
            {
                ID = FrameID.Publisher,
                ID3v4ID = "TPUB",
                ID3v3ID = "TPUB",
                ID3v2ID = "TPB",
                Desc = "Publisher"
            };
            _entries.Add(entry);

            entry = new ID3v2FrameEntry()
            {
                ID = FrameID.TrackNumber,
                ID3v4ID = "TRCK",
                ID3v3ID = "TRCK",
                ID3v2ID = "TRK",
                Desc = "TrackNumber"
            };
            _entries.Add(entry);

            entry = new ID3v2FrameEntry()
            {
                ID = FrameID.RecordingDates,
                ID3v4ID = null,
                ID3v3ID = "TRDA",
                ID3v2ID = "TRD",
                Desc = "RecordingDates"
            };
            _entries.Add(entry);

            entry = new ID3v2FrameEntry()
            {
                ID = FrameID.InternetRadioStationName,
                ID3v4ID = "TRSN",
                ID3v3ID = "TRSN",
                ID3v2ID = null,
                Desc = "InternetRadioStationName"
            };
            _entries.Add(entry);

            entry = new ID3v2FrameEntry()
            {
                ID = FrameID.InternetRadioStationOwner,
                ID3v4ID = "TRSO",
                ID3v3ID = "TRSO",
                ID3v2ID = null,
                Desc = "InternetRadioStationOwner"
            };
            _entries.Add(entry);

            entry = new ID3v2FrameEntry()
            {
                ID = FrameID.Size,
                ID3v4ID = "TSIZ",
                ID3v3ID = "TSIZ",
                ID3v2ID = "TSI",
                Desc = "Size"
            };
            _entries.Add(entry);

            entry = new ID3v2FrameEntry()
            {
                ID = FrameID.ISRC,
                ID3v4ID = "TSRC",
                ID3v3ID = "TSRC",
                ID3v2ID = "TRC",
                Desc = "ISRC"
            };
            _entries.Add(entry);

            entry = new ID3v2FrameEntry()
            {
                ID = FrameID.EncodingSettings,
                ID3v4ID = "TSSE",
                ID3v3ID = "TSSE",
                ID3v2ID = "TSS",
                Desc = "EncodingSettings"
            };
            _entries.Add(entry);

            entry = new ID3v2FrameEntry()
            {
                ID = FrameID.Year,
                ID3v4ID = "TYER",
                ID3v3ID = "TYER",
                ID3v2ID = "TYE",
                Desc = "Year"
            };
            _entries.Add(entry);

            entry = new ID3v2FrameEntry()
            {
                ID = FrameID.UserTextInformation,
                ID3v4ID = "TXXX",
                ID3v3ID = "TXXX",
                ID3v2ID = "TXX",
                Desc = "UserTextInformation"
            };
            _entries.Add(entry);

            entry = new ID3v2FrameEntry()
            {
                ID = FrameID.UniqueFileIdentifier,
                ID3v4ID = "UFID",
                ID3v3ID = "UFID",
                ID3v2ID = "UFI",
                Desc = "UniqueFileIdentifier"
            };
            _entries.Add(entry);

            entry = new ID3v2FrameEntry()
            {
                ID = FrameID.TermsOfUse,
                ID3v4ID = "USER",
                ID3v3ID = "USER",
                ID3v2ID = null,
                Desc = "TermsOfUse"
            };
            _entries.Add(entry);

            entry = new ID3v2FrameEntry()
            {
                ID = FrameID.UnsynchronizedLyris,
                ID3v4ID = "USLT",
                ID3v3ID = "USLT",
                ID3v2ID = "ULT",
                Desc = "UnsynchronizedLyris"
            };
            _entries.Add(entry);

            entry = new ID3v2FrameEntry()
            {
                ID = FrameID.CommercialInformationURL,
                ID3v4ID = "WCOM",
                ID3v3ID = "WCOM",
                ID3v2ID = "WCM",
                Desc = "CommercialInformationURL"
            };
            _entries.Add(entry);

            entry = new ID3v2FrameEntry()
            {
                ID = FrameID.CopyrightURL,
                ID3v4ID = "WCOP",
                ID3v3ID = "WCOP",
                ID3v2ID = "WCP",
                Desc = "CopyrightURL"
            };
            _entries.Add(entry);

            entry = new ID3v2FrameEntry()
            {
                ID = FrameID.OfficialAudioFileWebpage,
                ID3v4ID = "WOAF",
                ID3v3ID = "WOAF",
                ID3v2ID = "WAF",
                Desc = "OfficialAudioFileWebpage"
            };
            _entries.Add(entry);

            entry = new ID3v2FrameEntry()
            {
                ID = FrameID.OfficialArtistWebpage,
                ID3v4ID = "WOAR",
                ID3v3ID = "WOAR",
                ID3v2ID = "WAR",
                Desc = "OfficialArtistWebpage"
            };
            _entries.Add(entry);

            entry = new ID3v2FrameEntry()
            {
                ID = FrameID.OfficialAudioSourceWebpage,
                ID3v4ID = "WOAS",
                ID3v3ID = "WOAS",
                ID3v2ID = "WAS",
                Desc = "OfficialAudioSourceWebpage"
            };
            _entries.Add(entry);

            entry = new ID3v2FrameEntry()
            {
                ID = FrameID.InternetRadioStationWebpage,
                ID3v4ID = "WORS",
                ID3v3ID = "WORS",
                ID3v2ID = null,
                Desc = "InternetRadioStationWebpage"
            };
            _entries.Add(entry);

            entry = new ID3v2FrameEntry()
            {
                ID = FrameID.PaymentURL,
                ID3v4ID = "WPAY",
                ID3v3ID = "WPAY",
                ID3v2ID = null,
                Desc = "PaymentURL"
            };
            _entries.Add(entry);

            entry = new ID3v2FrameEntry()
            {
                ID = FrameID.PublishersOfficialWebpage,
                ID3v4ID = "WPUB",
                ID3v3ID = "WPUB",
                ID3v2ID = "WPB",
                Desc = "PublishersOfficialWebpage"
            };
            _entries.Add(entry);

            entry = new ID3v2FrameEntry()
            {
                ID = FrameID.UserURLLinkFrame,
                ID3v4ID = "WXXX",
                ID3v3ID = "WXXX",
                ID3v2ID = "WXX",
                Desc = "UserURLLinkFrame"
            };
            _entries.Add(entry);

            entry = new ID3v2FrameEntry()
            {
                ID = FrameID.MusicicanCreditsList,
                ID3v4ID = "TMCL",
                ID3v3ID = "TMCL",
                ID3v2ID = null,
                Desc = "MusicicanCreditsList"
            };
            _entries.Add(entry);

            entry = new ID3v2FrameEntry()
            {
                ID = FrameID.Mood,
                ID3v4ID = "TMOO",
                ID3v3ID = "TMOO",
                ID3v2ID = null,
                Desc = "Mood"
            };
            _entries.Add(entry);

            entry = new ID3v2FrameEntry()
            {
                ID = FrameID.ProducedNotice,
                ID3v4ID = "TPRO",
                ID3v3ID = "TPRO",
                ID3v2ID = null,
                Desc = "ProducedNotice"
            };
            _entries.Add(entry);

            entry = new ID3v2FrameEntry()
            {
                ID = FrameID.AlbumSortOrder,
                ID3v4ID = "TSOA",
                ID3v3ID = "TSOA",
                ID3v2ID = null,
                Desc = "AlbumSortOrder"
            };
            _entries.Add(entry);

            entry = new ID3v2FrameEntry()
            {
                ID = FrameID.PerformerSortOrder,
                ID3v4ID = "TSOP",
                ID3v3ID = "TSOP",
                ID3v2ID = null,
                Desc = "PerformerSortOrder"
            };
            _entries.Add(entry);

            entry = new ID3v2FrameEntry()
            {
                ID = FrameID.TitleSortOrder,
                ID3v4ID = "TSOT",
                ID3v3ID = "TSOT",
                ID3v2ID = null,
                Desc = "TitleSortOrder"
            };
            _entries.Add(entry);

            entry = new ID3v2FrameEntry()
            {
                ID = FrameID.SetSubtitle,
                ID3v4ID = "TSST",
                ID3v3ID = "TSST",
                ID3v2ID = null,
                Desc = "SetSubtitle"
            };
            _entries.Add(entry);
        }

        public class ID3v2FrameEntry
        {
            public FrameID ID { get; set; }

            public string ID3v4ID { get; set; }

            public string ID3v3ID { get; set; }

            public string ID3v2ID { get; set; }

            public string Desc { get; set; }
        }

        internal static string GetID(FrameID id, ID3Version version)
        {
            var entry = Frames.Where((x) => x.ID == id);
            if (entry != null && entry.Count() > 0)
            {
                var e = entry.First();
                string sid = null;
                if (version == ID3Version.ID3v2_2)
                    sid = e.ID3v2ID;
                else if (version == ID3Version.ID3v2_3)
                    sid = e.ID3v3ID;
                else if (version == ID3Version.ID3v2_4)
                    sid = e.ID3v4ID;
                else
                    throw new InvalidOperationException(String.Format("FrameID {0} is not supported on version {1}", id, version));

                if (sid == null)
                    throw new InvalidOperationException(String.Format("FrameID {0} is not supported on version {1}", id, version));
                return sid;
            }

            throw new ArgumentException("Invalid FrameID: " + id.ToString());
        }
    }
}