using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using YoutubeDownloader.ViewModels;

namespace YoutubeDownloader.Libraries.YoutubeExtractor.MediaLibrary
{
    class MediaLogger
    {
        public static List<MediaTrack> m_audioTracks = new List<MediaTrack>();
        public static List<MediaTrack> m_videoTracks = new List<MediaTrack>();

        public static List<MediaTrack> m_downloadingVideoTracks = new List<MediaTrack>();
        public static List<MediaTrack> m_downloadingAudioTracks = new List<MediaTrack>();

        public async static Task OpenLogFileAndLoadTracks(bool fLoadVideoTracks) 
        {
            Stream stream = await GetStreamOfHistoryFile(fLoadVideoTracks);

            StreamReader streamreader = new StreamReader(stream);
            string str = streamreader.ReadToEnd();
            List<MediaTrack> tracks = (List<MediaTrack>)JsonConvert.DeserializeObject(str, typeof(List<MediaTrack>));
            if (fLoadVideoTracks && tracks != null)
                m_videoTracks = tracks;
            else if (tracks != null)
                m_audioTracks = tracks;
            stream.Dispose();
        }

        public async static Task SaveTracksBackToFile(bool fSaveVideoTracks)
        {
            Stream stream = await GetStreamOfHistoryFile(fSaveVideoTracks);
            string str = JsonConvert.SerializeObject(fSaveVideoTracks ? m_videoTracks : m_audioTracks);

            StreamWriter strwriter = new StreamWriter(stream);
            strwriter.Write(str);

            strwriter.Dispose();
            stream.Dispose();
        }

        private async static Task<Stream> GetStreamOfHistoryFile(bool fVideoTracks) 
        {
            string filename = fVideoTracks ? "videoDownloads.txt" : "audioDownloads.txt";

            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            StorageFile storageFile = await localFolder.CreateFileAsync(filename, CreationCollisionOption.OpenIfExists);
            Stream stream = await storageFile.OpenStreamForWriteAsync();
            return stream;
        }

        public static void OnMediaDownloadStarted(MediaTrack track)
        {
            if (track.MediaType == MediaItemType.Video)
            {
                m_downloadingVideoTracks.Insert(0, track);
                m_videoTracks.Insert(0, track);
            }
            else if (track.MediaType == MediaItemType.Audio)
            {
                m_downloadingAudioTracks.Insert(0, track);
                m_audioTracks.Insert(0, track);
            }
        }

        public static void OnMediaDownloadCompleted(MediaTrack track, bool fcancel, int downloadedBytes)
        {
            track.Size = downloadedBytes;
            track.DownldStatus = fcancel ? DownloadStatus.Canceled : DownloadStatus.Completed;
            DownloadHistoryPageModel.UpdateMediaDownloadProgress(track, 100);

            // Save the tracks to history files
            if (track.MediaType == MediaItemType.Video)
            {
                m_downloadingVideoTracks.Remove(track);
                SaveTracksBackToFile(true);
            }
            else if (track.MediaType == MediaItemType.Audio)
            {
                m_downloadingAudioTracks.Remove(track);
                SaveTracksBackToFile(false);
            }
        }
    }
}
