﻿using System;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;
using SikkerDigitalPost.Domene.Entiteter.AsicE.Manifest;
using SikkerDigitalPost.Domene.Entiteter.AsicE.Signatur;
using SikkerDigitalPost.Domene.Entiteter.Interface;
using SikkerDigitalPost.Domene.Entiteter.Post;

namespace SikkerDigitalPost.Klient
{
    internal class AsicEArkiv : ISoapVedlegg
    {
        public readonly Manifest Manifest;
        public readonly Signatur Signatur;
        private readonly Dokumentpakke _dokumentpakke;
        private byte[] _bytes, _krypterteBytes;
        private string _contentId;

        public AsicEArkiv(Dokumentpakke dokumentpakke, Signatur signatur, Manifest manifest)
        {
            Signatur = signatur;
            Manifest = manifest;
            _dokumentpakke = dokumentpakke;
        }


        public string Filnavn
        {
            get { return "post.asice.zip"; }
        }


        public byte[] Bytes
        {
            get { return _bytes ?? LagBytes(); }
        }


        public string Innholdstype
        {
            get { return "application/cms"; }
        }

        public string ContentId
        {
            get { return _contentId ?? (_contentId = String.Format("{0}@meldingsformidler.sdp.difi.no", Guid.NewGuid())); }
        }

        public string TransferEncoding
        {
            get { return "binary"; }
        }

        private byte[] LagBytes()
        {
            if (_bytes != null)
                return _bytes;

            var stream = new MemoryStream();
            using (var archive = new ZipArchive(stream, ZipArchiveMode.Create))
            {
                LeggFilTilArkiv(archive, Manifest.Filnavn, Manifest.Bytes);
                LeggFilTilArkiv(archive, Signatur.Filnavn, Signatur.Bytes);

                foreach (var dokument in _dokumentpakke.Vedlegg)
                    LeggFilTilArkiv(archive, dokument.Filnavn, dokument.Bytes);

            }

            return _bytes = stream.ToArray();
        }

        private static void LeggFilTilArkiv(ZipArchive archive, string filename, byte[] data)
        {
            var entry = archive.CreateEntry(filename, CompressionLevel.Optimal);
            using (Stream s = entry.Open())
            {
                s.Write(data, 0, data.Length);
                s.Close();
            }
        }

        public byte[] KrypterteBytes(X509Certificate2 sertifikat)
        {
            if (_krypterteBytes != null)
                return _krypterteBytes;
            var contentInfo = new ContentInfo(Bytes);
            var encryptAlgoOid = new Oid("2.16.840.1.101.3.4.1.42"); // AES-256-CBC            
            var envelopedCms = new EnvelopedCms(contentInfo, new AlgorithmIdentifier(encryptAlgoOid));
            var recipient = new CmsRecipient(sertifikat);
            envelopedCms.Encrypt(recipient);
            return _krypterteBytes = envelopedCms.Encode();
        }

        public static byte[] Dekrypter(byte[] kryptertData)
        {
            var envelopedCms = new EnvelopedCms();
            envelopedCms.Decode(kryptertData);
            envelopedCms.Decrypt(envelopedCms.RecipientInfos[0]);
            return envelopedCms.ContentInfo.Content;
        }
    }
}
