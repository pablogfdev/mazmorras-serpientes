using System;
using System.Security.Cryptography;
using System.Text;

public static class CifradoAES
{
    private const string CONTRASENIA_BASE = "82b2154ae01cb861d1e08ebf19b7b76f9b9b03c1a8626ee72490d7007eec8cd3";

    
    private static Aes CrearInstanciaAES()
    {
        Aes aes = Aes.Create();
        aes.KeySize = 256;
        aes.BlockSize = 128;
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;
        using (SHA256 sha256 = SHA256.Create()) aes.Key = sha256.ComputeHash(Encoding.UTF8.GetBytes(CONTRASENIA_BASE));
        return aes;
    }


    public static byte[] Cifrar(string textoPlano)
    {
        using Aes aes = CrearInstanciaAES();
        aes.GenerateIV();
        byte[] textoPlanoBytes = Encoding.UTF8.GetBytes(textoPlano);
        using ICryptoTransform cifrador = aes.CreateEncryptor();
        byte[] datosCifrados = cifrador.TransformFinalBlock(textoPlanoBytes, 0, textoPlanoBytes.Length);
        byte[] resultado = new byte[aes.IV.Length + datosCifrados.Length];
        Buffer.BlockCopy(aes.IV, 0, resultado, 0, aes.IV.Length);
        Buffer.BlockCopy(datosCifrados, 0, resultado, aes.IV.Length, datosCifrados.Length);
        return resultado;
    }


    public static string Descifrar(byte[] datosCifradosConIV)
    {
        using Aes aes = CrearInstanciaAES();
        int tamanoIV = aes.BlockSize / 8;
        byte[] iv = new byte[tamanoIV];
        byte[] datosCifrados = new byte[datosCifradosConIV.Length - tamanoIV];
        Buffer.BlockCopy(datosCifradosConIV, 0, iv, 0, tamanoIV);
        Buffer.BlockCopy(datosCifradosConIV, tamanoIV, datosCifrados, 0, datosCifrados.Length);
        aes.IV = iv;
        using ICryptoTransform descifrador = aes.CreateDecryptor();
        byte[] textoPlanoBytes = descifrador.TransformFinalBlock(datosCifrados, 0, datosCifrados.Length);
        return Encoding.UTF8.GetString(textoPlanoBytes);
    }
}