﻿
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage;

namespace YoutubeExtractor
{
    internal class FlvFile : IDisposable
    {
        private long fileLength;
        private readonly string inputPath;
        private readonly string outputPath;
        private IAudioExtractor audioExtractor;
        private long fileOffset;
        private Stream fileStream;

        /// <summary>
        /// Initializes a new instance of the <see cref="FlvFile"/> class.
        /// </summary>
        /// <param name="inputPath">The path of the input.</param>
        /// <param name="outputPath">The path of the output without extension.</param>
        public FlvFile(string inputPath, string outputPath)
        {
            this.inputPath = inputPath;
            this.outputPath = outputPath;
        }

        async Task PerformSyncFileRead(string filename)
        {
            using (ManualResetEvent completedEvent = new ManualResetEvent(false))
            {
                //IAsyncOperation<StorageFile> asyncOp = ApplicationData.Current.LocalFolder.GetFileAsync(filename);
                //asyncOp.Completed = new AsyncOperationCompletedHandler<StorageFile>(async (IAsyncOperation<StorageFile> operation, AsyncStatus status) =>
                //{
                //    if (asyncOp.Status == AsyncStatus.Completed)
                //    {
                //        StorageFile fileStorage = asyncOp.GetResults();
                //        fileStream = await fileStorage.OpenStreamForReadAsync();
                //        completedEvent.Set();
                //    }
                //});

                StorageFile fileStorage = await ApplicationData.Current.LocalFolder.GetFileAsync(filename);
                fileStream = await fileStorage.OpenStreamForReadAsync();
                completedEvent.Set();
                completedEvent.WaitOne();
            }
        }

        public event EventHandler<ProgressEventArgs> ConversionProgressChanged;

        public bool ExtractedAudio { get; private set; }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <exception cref="AudioExtractionException">The input file is not an FLV file.</exception>
        public async void ExtractStreams()
        {
            await PerformSyncFileRead(this.inputPath);
            this.fileOffset = 0;
            this.fileLength = fileStream.Length;

            this.Seek(0);

            if (this.ReadUInt32() != 0x464C5601)
            {
                // not a FLV file
                throw new AudioExtractionException("Invalid input file. Impossible to extract audio track.");
            }

            this.ReadUInt8();
            uint dataOffset = this.ReadUInt32();

            this.Seek(dataOffset);

            this.ReadUInt32();

            while (fileOffset < fileLength)
            {
                if (!ReadTag())
                {
                    break;
                }

                if (fileLength - fileOffset < 4)
                {
                    break;
                }

                this.ReadUInt32();

                double progress = (this.fileOffset * 1.0 / this.fileLength) * 100;

                if (this.ConversionProgressChanged != null)
                {
                    this.ConversionProgressChanged(this, new ProgressEventArgs(progress));
                }
            }

            this.CloseOutput(false);
        }

        private void CloseOutput(bool disposing)
        {
            if (this.audioExtractor != null)
            {
                if (disposing && this.audioExtractor.VideoPath != null)
                {
                    try
                    {
                        // TODO anichopr
                        // Stream.Delete(this.audioExtractor.VideoPath);
                    }
                    catch { }
                }

                this.audioExtractor.Dispose();
                this.audioExtractor = null;
            }
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.fileStream != null)
                {
                    this.fileStream.Dispose();
                    this.fileStream = null;
                }

                this.CloseOutput(true);
            }
        }

        private IAudioExtractor GetAudioWriter(uint mediaInfo)
        {
            uint format = mediaInfo >> 4;

            switch (format)
            {
                case 14:
                case 2:
                    return new Mp3AudioExtractor(this.outputPath);

                case 10:
                    return new AacAudioExtractor(this.outputPath);
            }

            string typeStr;

            switch (format)
            {
                case 1:
                    typeStr = "ADPCM";
                    break;

                case 6:
                case 5:
                case 4:
                    typeStr = "Nellymoser";
                    break;

                default:
                    typeStr = "format=" + format;
                    break;
            }

            throw new AudioExtractionException("Unable to extract audio (" + typeStr + " is unsupported).");
        }

        private byte[] ReadBytes(int length)
        {
            var buff = new byte[length];

            this.fileStream.Read(buff, 0, length);
            this.fileOffset += length;

            return buff;
        }

        private bool ReadTag()
        {
            if (this.fileLength - this.fileOffset < 11)
                return false;

            // Read tag header
            uint tagType = ReadUInt8();
            uint dataSize = ReadUInt24();
            uint timeStamp = ReadUInt24();
            timeStamp |= this.ReadUInt8() << 24;
            this.ReadUInt24();

            // Read tag data
            if (dataSize == 0)
                return true;

            if (this.fileLength - this.fileOffset < dataSize)
                return false;

            uint mediaInfo = this.ReadUInt8();
            dataSize -= 1;
            byte[] data = this.ReadBytes((int)dataSize);

            if (tagType == 0x8)
            {
                // If we have no audio writer, create one
                if (this.audioExtractor == null)
                {
                    this.audioExtractor = this.GetAudioWriter(mediaInfo);
                    this.ExtractedAudio = this.audioExtractor != null;
                }

                if (this.audioExtractor == null)
                {
                    throw new InvalidOperationException("No supported audio writer found.");
                }

                this.audioExtractor.WriteChunk(data, timeStamp);
            }

            return true;
        }

        private uint ReadUInt24()
        {
            var x = new byte[4];

            this.fileStream.Read(x, 1, 3);
            this.fileOffset += 3;

            return BigEndianBitConverter.ToUInt32(x, 0);
        }

        private uint ReadUInt32()
        {
            var x = new byte[4];

            this.fileStream.Read(x, 0, 4);
            this.fileOffset += 4;

            return BigEndianBitConverter.ToUInt32(x, 0);
        }

        private uint ReadUInt8()
        {
            this.fileOffset += 1;
            return (uint)this.fileStream.ReadByte();
        }

        private void Seek(long offset)
        {
            this.fileStream.Seek(offset, SeekOrigin.Begin);
            this.fileOffset = offset;
        }
    }
}