﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OggVorbisEncoder;
using System.IO;

namespace WheelTTSLinesCreatorLibrary
{
    internal static class WavToOggConverter
    {
        // The following code shown below is based upon the code from https://stackoverflow.com/questions/57288427/convert-audio-files-to-ogg-in-net-core
        // created by Steve Todd and is being used here to convert the wav files generated by the speech synthesizer to the ogg format used in the game.
        public static void EncodeWavToOgg(string path, string fileName)
        {
            FileStream inputWav = new FileStream($@"{ path }\{ fileName }.wav", FileMode.Open, FileAccess.Read);
            FileStream outputOgg = new FileStream($@"{ path }\{ fileName }.ogg", FileMode.Create, FileAccess.Write);

            StripWavHeader(inputWav);

            VorbisInfo info = VorbisInfo.InitVariableBitRate(2, 44100, 0.85f);

            int serial = new Random().Next();
            OggStream oggStream = new OggStream(serial);

            // =========================================================
            // HEADER
            // =========================================================
            // Vorbis streams begin with three headers; the initial header (with
            // most of the codec setup parameters) which is mandated by the Ogg
            // bitstream spec.  The second header holds any comment fields.  The
            // third header holds the bitstream codebook.

            Comments comments = new Comments();

            OggPacket infoPacket = HeaderPacketBuilder.BuildInfoPacket(info);
            OggPacket commentsPacket = HeaderPacketBuilder.BuildCommentsPacket(comments);
            OggPacket booksPacket = HeaderPacketBuilder.BuildBooksPacket(info);

            oggStream.PacketIn(infoPacket);
            oggStream.PacketIn(commentsPacket);
            oggStream.PacketIn(booksPacket);

            // Flush to force audio data onto its own page per the spec
            OggPage page;

            while (oggStream.PageOut(out page, true))
            {
                outputOgg.Write(page.Header, 0, page.Header.Length);
                outputOgg.Write(page.Body, 0, page.Body.Length);
            }

            // =========================================================
            // BODY (Audio Data)
            // =========================================================
            ProcessingState processingState = ProcessingState.Create(info);

            float[][] buffer = new float[info.Channels][];
            buffer[0] = new float[1024];
            buffer[1] = new float[1024];

            byte[] readbuffer = new byte[4096];

            while (!oggStream.Finished)
            {
                var bytes = inputWav.Read(readbuffer, 0, readbuffer.Length);

                if (bytes == 0)
                {
                    processingState.WriteEndOfStream();
                }
                else
                {
                    var samples = bytes / 4;

                    for (var i = 0; i < samples; i++)
                    {
                        // uninterleave samples
                        buffer[0][i] = (short)((readbuffer[i * 4 + 1] << 8) | (0x00ff & readbuffer[i * 4])) / 32768f;
                        buffer[1][i] = (short)((readbuffer[i * 4 + 3] << 8) | (0x00ff & readbuffer[i * 4 + 2])) / 32768f;
                    }

                    processingState.WriteData(buffer, samples);
                }

                OggPacket packet;

                while (!oggStream.Finished
                       && processingState.PacketOut(out packet))
                {
                    oggStream.PacketIn(packet);

                    while (!oggStream.Finished
                           && oggStream.PageOut(out page, false))
                    {
                        outputOgg.Write(page.Header, 0, page.Header.Length);
                        outputOgg.Write(page.Body, 0, page.Body.Length);
                    }
                }
            }

            inputWav.Close();
            outputOgg.Close();

            //delete the old wav file
            File.Delete($@"{ path }\{ fileName }.wav");
        }

        /// <summary>
        ///     We cheat on the WAV header; we just bypass the header and never
        ///     verify that it matches 16bit/stereo/44.1kHz.This is just an
        ///     example, after all.
        /// </summary>
        private static void StripWavHeader(FileStream stdin)
        {
            byte[] tempBuffer = new byte[6];
            for (var i = 0; (i < 30) && (stdin.Read(tempBuffer, 0, 2) > 0); i++)
            {
                if ((tempBuffer[0] == 'd') && (tempBuffer[1] == 'a'))
                {
                    stdin.Read(tempBuffer, 0, 6);
                    break;
                }
            }
        }
    }
}
