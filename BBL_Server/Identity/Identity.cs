using System;
using System.Collections.Generic;
using JWT;
using JWT.Builder;

namespace BBL.Utils.Identity
{
    public class Identity
    {
        private string secret = "mylittlesecret";
        public IDictionary<string, object> payload;

        public bool Parse(string token)
        {
            try
            {

                this.payload = new JwtBuilder()
                    .WithSecret(secret)
                    .MustVerifySignature()
                    .Decode<IDictionary<string, object>>(token);
                return true;
            }
            catch (TokenExpiredException)
            {
                Console.WriteLine("Token has expired");
            }
            catch (SignatureVerificationException)
            {
                Console.WriteLine("Token has invalid signature");
            }
            return false;
        }
    }
}
