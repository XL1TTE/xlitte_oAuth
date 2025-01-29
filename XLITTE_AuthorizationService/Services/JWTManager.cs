using Microsoft.AspNetCore.Authentication;
using System;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Text;

namespace XLITTE_AuthorizationService.Services
{
    public class JWTManager
    {
        private string _token;

        public JWTManager(string token)
        {
            _token = token;
        }

        public static string CreateToken(string headers, string payload, string key)
        {
            byte[] headers_bytes = Encoding.UTF8.GetBytes(headers);
            byte[] payload_bytes = Encoding.UTF8.GetBytes(payload);

            string Token_headers = Base64UrlTextEncoder.Encode(headers_bytes);
            string Token_payloads = Base64UrlTextEncoder.Encode(payload_bytes);

            string Token_signature = SignToken(Token_headers, Token_payloads, key);

            string token = $"{Token_headers}.{Token_payloads}.{Token_signature}";
            return token;
        }

        private static string SignToken(string headers, string payload, string key)
        {
            string Token_signature = "";

            using (HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key)))
            {
                byte[] signature_temp = Encoding.UTF8.GetBytes(headers + '.' + payload);

                signature_temp = hmac.ComputeHash(signature_temp);

                Token_signature = Base64UrlTextEncoder.Encode(signature_temp);
            }

            return Token_signature;
        }


        public void SetToken(string token)
        {
            _token = token;
        }        
        public string GetHeaders()
        {
            string headersBase64Url= _token.Split('.')[0];
            string headers = Encoding.UTF8.GetString(Base64UrlTextEncoder.Decode(headersBase64Url));

            return headers;
        }

        public string GetPayload()
        {
            string PayloadBase64Url = _token.Split('.')[1];
            string payload = Encoding.UTF8.GetString(Base64UrlTextEncoder.Decode(PayloadBase64Url));

            return payload;
        }

        public string GetSignature()
        {
            string SignatureBase64Url = _token.Split('.')[2];
            
            return SignatureBase64Url;
        }
        public bool VerifyTokenSignature(string secretKey)
        {
            try
            {
                string Signature = GetSignature();

                string HeadersBase64Url = _token.Split('.')[0];
                string PayloadBase64Url = _token.Split('.')[1];

                string Legal_signature = SignToken(HeadersBase64Url, PayloadBase64Url, secretKey);

                if (Legal_signature == Signature)
                {
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }

        }
    }
}
