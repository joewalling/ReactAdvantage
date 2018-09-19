using System.Security.Cryptography.X509Certificates;

namespace ReactAdvantage.IdentityServer.Startup
{
    public static class CertificateHelper
    {
        public static X509Certificate2 GetCertificateFromStore(string thumbprint)
        {
            if (string.IsNullOrEmpty(thumbprint))
            {
                return null;
            }

            using (var certStore = new X509Store(StoreName.My, StoreLocation.CurrentUser))
            {
                certStore.Open(OpenFlags.ReadOnly);
                var certCollection = certStore.Certificates
                    .Find(X509FindType.FindByThumbprint, thumbprint, false);

                if (certCollection.Count > 0)
                {
                    return certCollection[0];
                }
            }

            return null;
        }
    }
}
