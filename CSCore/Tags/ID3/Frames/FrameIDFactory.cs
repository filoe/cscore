using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCore.Tags.ID3.Frames
{
    public static class FrameIDFactory
    {
        static Dictionary<string, string> _version2To3Map;
        private static Dictionary<string, string> Version2To3Map
        {
            get { return _version2To3Map ?? (_version2To3Map = CreateVersion2To3Map()); }
        }

        static Dictionary<string, string> _version3To2Map;
        private static Dictionary<string, string> Version3To2Map
        {
            get { return _version3To2Map ?? (_version3To2Map = CreateVersion3To2Map()); }
        }

        private static Dictionary<string, string> CreateVersion2To3Map()
        {
            Dictionary<string, string> map = new Dictionary<string, string>();
            map.Add("CRA", "AENC");
            map.Add("PIC", "APIC");
            map.Add("COM", "COMM");
            map.Add("EQU", "EQUA");
            map.Add("ETC", "ETCO");
            map.Add("GEO", "GEOB");
            map.Add("IPL", "IPLS");
            map.Add("TIP", "TIPL");
            map.Add("LNK", "LINK");
            map.Add("MCI", "MCDI");
            map.Add("MLL", "MLLT");
            map.Add("CNT", "PCNT");
            map.Add("POP", "POPM");
            map.Add("BUF", "RBUF");
            map.Add("RVA", "RVAD");
            map.Add("REV", "RVRB");
            map.Add("SLT", "SYLT");
            map.Add("STC", "SYTC");
            map.Add("TAL", "TALB");
            map.Add("TBP", "TBPM");
            map.Add("TCM", "TCOM");
            map.Add("TCO", "TCON");
            map.Add("TCP", "TCOP");
            map.Add("TDA", "TDAT");
            map.Add("TDY", "TDLY");
            map.Add("TEN", "TENC");
            map.Add("TXT", "TEXT");
            map.Add("TFT", "TFLT");
            map.Add("TIM", "TIME");
            map.Add("TT1", "TIT1");
            map.Add("TT2", "TIT2");
            map.Add("TT3", "TIT3");
            map.Add("TKE", "TKEY");
            map.Add("TLA", "TLAN");
            map.Add("TLE", "TLEN");
            map.Add("TMT", "TMED");
            map.Add("TOT", "TOAL");
            map.Add("TOF", "TOFN");
            map.Add("TOL", "TOLY");
            map.Add("TOA", "TOPE");
            map.Add("TOR", "TORY");
            map.Add("TP1", "TPE1");
            map.Add("TP2", "TPE2");
            map.Add("TP3", "TPE3");
            map.Add("TP4", "TPE4");
            map.Add("TPA", "TPOS");
            map.Add("TPB", "TPUB");
            map.Add("TRK", "TRCK");
            map.Add("TRD", "TRDA");
            map.Add("TSI", "TSIZ");
            map.Add("TRC", "TSRC");
            map.Add("TSS", "TSSE");
            map.Add("TYE", "TYER");
            map.Add("TXX", "TXXX");
            map.Add("UFI", "UFID");
            map.Add("ULT", "USLT");
            map.Add("WCM", "WCOM");
            map.Add("WCP", "WCOP");
            map.Add("WAF", "WOAF");
            map.Add("WAR", "WOAR");
            map.Add("WAS", "WOAS");
            map.Add("WPB", "WPUB");
            map.Add("WXX", "WXXX");

            return map;
        }

        private static Dictionary<string, string> CreateVersion3To2Map()
        {
            Dictionary<string, string> map = new Dictionary<string, string>();
            map.Add("AENC", "CRA");
            map.Add("APIC", "PIC");
            map.Add("COMM", "COM");
            map.Add("EQUA", "EQU");
            map.Add("ETCO", "ETC");
            map.Add("GEOB", "GEO");
            map.Add("IPLS", "IPL");
            map.Add("TIPL", "IPL");
            map.Add("LINK", "LNK");
            map.Add("MCDI", "MCI");
            map.Add("MLLT", "MLL");
            map.Add("PCNT", "CNT");
            map.Add("POPM", "POP");
            map.Add("RBUF", "BUF");
            map.Add("RVAD", "RVA");
            map.Add("RVRB", "REV");
            map.Add("SYLT", "SLT");
            map.Add("SYTC", "STC");
            map.Add("TALB", "TAL");
            map.Add("TBPM", "TBP");
            map.Add("TCOM", "TCM");
            map.Add("TCON", "TCO");
            map.Add("TCOP", "TCP");
            map.Add("TDAT", "TDA");
            map.Add("TDLY", "TDY");
            map.Add("TENC", "TEN");
            map.Add("TEXT", "TXT");
            map.Add("TFLT", "TFT");
            map.Add("TIME", "TIM");
            map.Add("TIT1", "TT1");
            map.Add("TIT2", "TT2");
            map.Add("TIT3", "TT3");
            map.Add("TKEY", "TKE");
            map.Add("TLAN", "TLA");
            map.Add("TLEN", "TLE");
            map.Add("TMED", "TMT");
            map.Add("TOAL", "TOT");
            map.Add("TOFN", "TOF");
            map.Add("TOLY", "TOL");
            map.Add("TOPE", "TOA");
            map.Add("TORY", "TOR");
            map.Add("TPE1", "TP1");
            map.Add("TPE2", "TP2");
            map.Add("TPE3", "TP3");
            map.Add("TPE4", "TP4");
            map.Add("TPOS", "TPA");
            map.Add("TPUB", "TPB");
            map.Add("TRCK", "TRK");
            map.Add("TRDA", "TRD");
            map.Add("TSIZ", "TSI");
            map.Add("TSRC", "TRC");
            map.Add("TSSE", "TSS");
            map.Add("TYER", "TYE");
            map.Add("TXXX", "TXX");
            map.Add("UFID", "UFI");
            map.Add("USLT", "ULT");
            map.Add("WCOM", "WCM");
            map.Add("WCOP", "WCP");
            map.Add("WOAF", "WAF");
            map.Add("WOAR", "WAR");
            map.Add("WOAS", "WAS");
            map.Add("WPUB", "WPB");
            map.Add("WXXX", "WXX");

            return map;
        }

        public static string GetID(FrameID frameId, ID3Version version)
        {
            if (version == ID3Version.ID3v1)
                throw new ArgumentOutOfRangeException("ID3v1 does not support frames");

            string id = String.Empty;
            //http://id3.org/id3v2.3.0
            switch (frameId)
            {
                case FrameID.AudioEnctryption:
                    id = "AENC"; break;
                case FrameID.AttachedPicutre:
                    id = "APIC"; break;
                case FrameID.Comments:
                    id = "COMM"; break;
                case FrameID.CommercialFrame:
                    id = "COMR"; break;
                case FrameID.EncryptionMethodRegistration:
                    id = "ENCR"; break;
                case FrameID.Equalization:
                    id = "EQUA"; break;
                case FrameID.EventTimeingCodes:
                    id = "ETCO"; break;
                case FrameID.GeneralEncapsulatedObject:
                    id = "GEOB"; break;
                case FrameID.GroupIdentificationRegistration:
                    id = "GRID"; break;
                case FrameID.InvolvedPeopleList:
                    if (version == ID3Version.ID3v2_4)
                        id = "TIPL"; break;
                    id = "IPLS"; break;
                case FrameID.LinkedInformation:
                    id = "LINK"; break;
                case FrameID.MusicCDIdentifier:
                    id = "MCDI"; break;
                case FrameID.MPEGLocationLookupTable:
                    id = "MLLT"; break;
                case FrameID.OwnershipFrame:
                    id = "OWNE"; break;
                case FrameID.PrivateFrame:
                    id = "PRIV"; break;
                case FrameID.PlayCounter:
                    id = "PCNT"; break;
                case FrameID.Popularimeter:
                    id = "POPM"; break;
                case FrameID.PositionSynchronisationFrame:
                    id = "POSS"; break;
                case FrameID.RecommendedBufferSize:
                    id = "RBUF"; break;
                case FrameID.RelativeVolumeAdjustment:
                    id = "RVAD"; break;
                case FrameID.Reverb:
                    id = "RVRB"; break;
                case FrameID.SynchronizedLyrics:
                    id = "SYLT"; break;
                case FrameID.SynchronizedTempoCodes:
                    id = "SYTC"; break;
                case FrameID.Album:
                    id = "TALB"; break;
                case FrameID.BeatsPerMinute:
                    id = "TBPM"; break;
                case FrameID.Composer:
                    id = "TCOM"; break;
                case FrameID.ContentType:
                    id = "TCON"; break;
                case FrameID.CopyrightMessage:
                    id = "TCOP"; break;
                case FrameID.Date:
                    id = "TDAT"; break;
                case FrameID.PlaylistDelay:
                    id = "TDLY"; break;
                case FrameID.EncodedBy:
                    id = "TENC"; break;
                case FrameID.TextWriter:
                    id = "TEXT"; break;
                case FrameID.FileType:
                    id = "TFLT"; break;
                case FrameID.Time:
                    id = "TIME"; break;
                case FrameID.ContentGroupDescription:
                    id = "TIT1"; break;
                case FrameID.Title:
                    id = "TIT2"; break;
                case FrameID.Subtitle:
                    id = "TIT3"; break;
                case FrameID.InitialKey:
                    id = "TKEY"; break;
                case FrameID.Languages:
                    id = "TLAN"; break;
                case FrameID.Length:
                    id = "TLEN"; break;
                case FrameID.MediaType:
                    id = "TMED"; break;
                case FrameID.OriginalAlbum:
                    id = "TOAL"; break;
                case FrameID.OriginalFileName:
                    id = "TOFN"; break;
                case FrameID.OriginalTextWriter:
                    id = "TOLY"; break;
                case FrameID.OriginalArtist:
                    id = "TOPE"; break;
                case FrameID.OriginalReleaseYear:
                    id = "TORY"; break;
                case FrameID.FileOwner:
                    id = "TOWN"; break;
                case FrameID.LeadPerformers:
                    id = "TPE1"; break;
                case FrameID.Band:
                    id = "TPE2"; break;
                case FrameID.Conductor:
                    id = "TPE3"; break;
                case FrameID.Interpreted:
                    id = "TPE4"; break;
                case FrameID.PartOfASet:
                    id = "TPOS"; break;
                case FrameID.Publisher:
                    id = "TPUB"; break;
                case FrameID.TrackNumber:
                    id = "TRCK"; break;
                case FrameID.RecordingDates:
                    id = "TRDA"; break;
                case FrameID.InternetRadioStationName:
                    id = "TRSN"; break;
                case FrameID.InternetRadioStationOwner:
                    id = "TRSO"; break;
                case FrameID.Size:
                    id = "TSIZ"; break;
                case FrameID.ISRC:
                    id = "TSRC"; break;
                case FrameID.EncodingSettings:
                    id = "TSSE"; break;
                case FrameID.Year:
                    id = "TYER"; break;
                case FrameID.UserTextInformation:
                    id = "TXXX"; break;
                case FrameID.UniqueFileIdentifier:
                    id = "UFID"; break;
                case FrameID.TermsOfUse:
                    id = "USER"; break;
                case FrameID.UnsynchronizedLyris:
                    id = "USLT"; break;
                case FrameID.CommercialInformationURL:
                    id = "WCOM"; break;
                case FrameID.CopyrightURL:
                    id = "WCOP"; break;
                case FrameID.OfficialAudioFileWebpage:
                    id = "WOAF"; break;
                case FrameID.OfficialArtistWebpage:
                    id = "WOAR"; break;
                case FrameID.OfficialAudioSourceWebpage:
                    id = "WOAS"; break;
                case FrameID.InternetRadioStationWebpage:
                    id = "WORS"; break;
                case FrameID.PaymentURL:
                    id = "WPAY"; break;
                case FrameID.PublishersOfficialWebpage:
                    id = "WPUB"; break;
                case FrameID.UserURLLinkFrame:
                    id = "WXXX"; break;
                case FrameID.MusicicanCreditsList:
                    id = "TMCL"; break;
                case FrameID.Mood:
                    id = "TMOO"; break;
                case FrameID.ProducedNotice:
                    id = "TPRO"; break;
                case FrameID.AlbumSortOrder:
                    id = "TSOA"; break;
                case FrameID.PerformerSortOrder:
                    id = "TSOP"; break;
                case FrameID.TitleSortOrder:
                    id = "TSOT"; break;
                case FrameID.SetSubtitle:
                    id = "TSST"; break;
                default:
                    throw new ArgumentOutOfRangeException("frameid", "Invalid FrameID");
            }

            if (version == ID3Version.ID3v2_2)
            {
                id = ConvertVersion3IDToVersion2(id);
            }
            return id;
        }

        public static FrameID GetFrameType(string frameID, ID3Version version)
        {
            if (version == ID3Version.ID3v1)
                throw new ArgumentOutOfRangeException("ID3v1 does not support frames");

            if (version == ID3Version.ID3v2_2)
                frameID = ConvertVersion2IDToVersion3(frameID);

            switch (frameID)
            {
                case "AENC":
                    return FrameID.AudioEnctryption;
                case "APIC":
                    return FrameID.AttachedPicutre;
                case "COMM":
                    return FrameID.Comments;
                case "COMR":
                    return FrameID.CommercialFrame;
                case "ENCR":
                    return FrameID.EncryptionMethodRegistration;
                case "EQUA":
                    return FrameID.Equalization;
                case "ETCO":
                    return FrameID.EventTimeingCodes;
                case "GEOB":
                    return FrameID.GeneralEncapsulatedObject;
                case "GRID":
                    return FrameID.GroupIdentificationRegistration;
                case "IPLS":
                case "TIPL":
                    return FrameID.InvolvedPeopleList;
                case "LINK":
                    return FrameID.LinkedInformation;
                case "MCDI":
                    return FrameID.MusicCDIdentifier;
                case "MLLT":
                    return FrameID.MPEGLocationLookupTable;
                case "OWNE":
                    return FrameID.OwnershipFrame;
                case "PRIV":
                    return FrameID.PrivateFrame;
                case "PCNT":
                    return FrameID.PlayCounter;
                case "POPM":
                    return FrameID.Popularimeter;
                case "POSS":
                    return FrameID.PositionSynchronisationFrame;
                case "RBUF":
                    return FrameID.RecommendedBufferSize;
                case "RVAD":
                    return FrameID.RelativeVolumeAdjustment;
                case "RVRB":
                    return FrameID.Reverb;
                case "SYLT":
                    return FrameID.SynchronizedLyrics;
                case "SYTC":
                    return FrameID.SynchronizedTempoCodes;
                case "TALB":
                    return FrameID.Album;
                case "TBPM":
                    return FrameID.BeatsPerMinute;
                case "TCOM":
                    return FrameID.Composer;
                case "TCON":
                    return FrameID.ContentType;
                case "TCOP":
                    return FrameID.CopyrightMessage;
                case "TDAT":
                    return FrameID.Date;
                case "TDLY":
                    return FrameID.PlaylistDelay;
                case "TENC":
                    return FrameID.EncodedBy;
                case "TEXT":
                    return FrameID.TextWriter;
                case "TFLT":
                    return FrameID.FileType;
                case "TIME":
                    return FrameID.Time;
                case "TIT1":
                    return FrameID.ContentGroupDescription;
                case "TIT2":
                    return FrameID.Title;
                case "TIT3":
                    return FrameID.Subtitle;
                case "TKEY":
                    return FrameID.InitialKey;
                case "TLAN":
                    return FrameID.Languages;
                case "TLEN":
                    return FrameID.Length;
                case "TMED":
                    return FrameID.MediaType;
                case "TOAL":
                    return FrameID.OriginalAlbum;
                case "TOFN":
                    return FrameID.OriginalFileName;
                case "TOLY":
                    return FrameID.OriginalTextWriter;
                case "TOPE":
                    return FrameID.OriginalArtist;
                case "TORY":
                    return FrameID.OriginalReleaseYear;
                case "TPE1":
                    return FrameID.LeadPerformers;
                case "TPE2":
                    return FrameID.Band;
                case "TPE3":
                    return FrameID.Conductor;
                case "TPE4":
                    return FrameID.Interpreted;
                case "TPOS":
                    return FrameID.PartOfASet;
                case "TPUB":
                    return FrameID.Publisher;
                case "TRCK":
                    return FrameID.TrackNumber;
                case "TRDA":
                    return FrameID.RecordingDates;
                case "TRSN":
                    return FrameID.InternetRadioStationName;
                case "TRSO":
                    return FrameID.InternetRadioStationOwner;
                case "TSIZ":
                    return FrameID.Size;
                case "TSRC":
                    return FrameID.ISRC;
                case "TSSE":
                    return FrameID.EncodingSettings;
                case "TYER":
                    return FrameID.Year;
                case "TXXX":
                    return FrameID.UserTextInformation;
                case "UFID":
                    return FrameID.UniqueFileIdentifier;
                case "USER":
                    return FrameID.TermsOfUse;
                case "USLT":
                    return FrameID.UnsynchronizedLyris;
                case "WCOM":
                    return FrameID.CommercialInformationURL;
                case "WCOP":
                    return FrameID.CopyrightURL;
                case "WOAF":
                    return FrameID.OfficialAudioFileWebpage;
                case "WOAR":
                    return FrameID.OfficialArtistWebpage;
                case "WOAS":
                    return FrameID.OfficialAudioSourceWebpage;
                case "WORS":
                    return FrameID.InternetRadioStationWebpage;
                case "WPAY":
                    return FrameID.PaymentURL;
                case "WPUB":
                    return FrameID.PublishersOfficialWebpage;
                case "WXXX":
                    return FrameID.UserURLLinkFrame;
                case "TMCL":
                    return FrameID.MusicicanCreditsList;
                case "TMOO":
                    return FrameID.Mood;
                case "TPRO":
                    return FrameID.ProducedNotice;
                case "TSOA":
                    return FrameID.AlbumSortOrder;
                case "TSOP":
                    return FrameID.PerformerSortOrder;
                case "TSOT":
                    return FrameID.TitleSortOrder;
                case "TSST":
                    return FrameID.SetSubtitle;
                default:
                    throw new ArgumentOutOfRangeException("frameid", "Invalid FrameID");
            }
        }

        public static string ConvertVersion3IDToVersion2(string version3ID)
        {
            string result;
            bool success = Version3To2Map.TryGetValue(version3ID, out result);
            if (!success)
            {
                throw new ArgumentException("FrameID is not supported on ID3v2.2", "version3ID");
            }

            return result;
        }

        public static string ConvertVersion2IDToVersion3(string version2ID)
        {
            string result;
            bool success = Version2To3Map.TryGetValue(version2ID, out result);
            if (!success)
            {
                throw new ArgumentException("FrameID is not supported on ID3v2.3 or ID3v2.4", "version3ID");
            }

            return result;
        }
    }
}
