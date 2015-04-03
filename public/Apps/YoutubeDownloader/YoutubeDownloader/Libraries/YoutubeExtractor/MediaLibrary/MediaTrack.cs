using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoutubeDownloader.Libraries.YoutubeExtractor.MediaLibrary 
{
    public enum DownloadStatus
    {
        Downloading = 0x01,
        Canceled = 0x02,
        Completed = 0x04,
    }

    public enum MediaItemType
    {
        Audio = 0x01,
        Video = 0x02,
        Unknown = 0x04,
    }

    public class MediaTrack
    {
        // Our metadata
        private uint m_id = 0;
        private string m_url;
        private string m_title;
        private int m_size;
        private MediaItemType m_mediatype;
        private DownloadStatus m_downloadStatus;

        /// <summary>
        /// Creates a new audio track and sets metadata at initalization time.
        /// </summary>
        public MediaTrack(string url, string title, MediaItemType mediatype)
        {
            m_url = url; m_title = title;
            m_mediatype = mediatype;
            m_downloadStatus = DownloadStatus.Downloading;
        }

        public string Source
        {
            get { return m_url; }
            set { m_url = value; }
        }

        public string Title
        {
            get { return m_title; }
            set { m_title = value; }
        }

        public int Size
        {
            get { return m_size; }
            set { m_size = value; }
        }

        public DownloadStatus DownldStatus
        {
            get { return m_downloadStatus; }
            set { m_downloadStatus = value; }
        }

        public MediaItemType MediaType
        {
            get { return m_mediatype; }
            set { m_mediatype = value; }
        }
    }
}
