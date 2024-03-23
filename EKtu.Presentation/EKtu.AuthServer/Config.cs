using IdentityServer4.Models;

namespace EKtu.AuthServer
{
    public class Config
    {
        public static IEnumerable<ApiScope> GetApiScopes() => new List<ApiScope>()
        {
            new ApiScope("student.read","öğrenci için okuma izni"),
            new ApiScope("teacher.read","öğretmen için okuma izni"),
            new ApiScope("teacher.update","öğretmen için güncelleme izni"),
            new ApiScope("teacher.delete","öğretmen için silme izni")

        };
        public static IEnumerable<Client> GetClients() => new List<Client>()
        {
            new Client()
            {
                ClientId="StudentClient",
                AllowedGrantTypes=GrantTypes.ClientCredentials,
                ClientSecrets =
                {
                    new Secret("secret".Sha256())
                },
                AllowedScopes={"student.read"}
            },
            new Client()
            {
                ClientId="TeacherClient",
                AllowedGrantTypes=GrantTypes.ClientCredentials,
                ClientSecrets =
                {
                    new Secret("secret".Sha256())
                },
                AllowedScopes={"teacher.read","teacher.update"}
            },
            new Client()
            {
                ClientId="PrincipalClient",
                AllowedGrantTypes=GrantTypes.ClientCredentials,
                ClientSecrets =
                {
                    new Secret("secret".Sha256())
                },
                AllowedScopes={"teacher.delete"}
            }
        };
        public static IEnumerable<ApiResource> GetApiResources() => new List<ApiResource>()
        {
            new ApiResource("Api")
            {
                Scopes = new[] {"student.read","teacher.read","teacher.update","teacher.delete"}
            },
        };
    }
}
