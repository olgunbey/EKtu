using IdentityServer4.Models;

namespace EKtu.AuthServer
{
    public class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources() => new List<IdentityResource>()
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Email(),
        };
        public static IEnumerable<ApiScope> GetApiScopes() => new List<ApiScope>()
        {
            new ApiScope("exam.update","not girişi"),
            new ApiScope("exam.read","notları görme"),
            new ApiScope("exam.list","sınav sonuçlarını listeleme"),

            new ApiScope("student.delete","öğrenci silme"),
            new ApiScope("student.update","öğrenci bilgi güncelleme"),
            new ApiScope("student.added","öğrenci ekleme"),

            new ApiScope("absence.entry","devamsızlık giriş")
            

        };
        public static IEnumerable<Client> GetClients() => new List<Client>()
        {
            new Client()
            {
                ClientId="ResourceOwnerTeacher",
                AllowedGrantTypes=GrantTypes.ResourceOwnerPassword,
                ClientSecrets=new[]{new Secret("secret".Sha256())},
                AllowedScopes={"exam.update","exam.read"},
                RefreshTokenUsage=TokenUsage.ReUse,
                RefreshTokenExpiration=TokenExpiration.Absolute,
                AccessTokenLifetime=3,
                AbsoluteRefreshTokenLifetime=900
            },
            new Client()
            {
                ClientId="ResourceOwnerPrincipal",
                AllowedGrantTypes=GrantTypes.ResourceOwnerPassword,
                ClientSecrets=new[]{new Secret("secret".Sha256())},
                AllowedScopes={"student.delete","student.update","student.added","exam.read", "absence.entry" },
                RefreshTokenUsage=TokenUsage.ReUse,
                RefreshTokenExpiration=TokenExpiration.Absolute,
                AccessTokenLifetime=3,
                AbsoluteRefreshTokenLifetime=900
            },
            new Client()
            {
                ClientId="ResourceOwnerStudent",
                AllowedGrantTypes=GrantTypes.ResourceOwnerPassword,
                ClientSecrets=new[]{new Secret("secret".Sha256())},
                AllowedScopes={"exam.list"},
                RefreshTokenUsage=TokenUsage.ReUse,
                RefreshTokenExpiration=TokenExpiration.Absolute,
                AccessTokenLifetime=3,
                AbsoluteRefreshTokenLifetime=900
            }
        };
        public static IEnumerable<ApiResource> GetApiResources() => new List<ApiResource>()
        {
            #region ApiResourceler
            //new ApiResource()
            //{
            //    Name="TeacherApi",
            //    Scopes={"exam.update","exam.read"},
            //},
            //new ApiResource()
            //{
            //    Name="StudentApi",
            //    Scopes={"exam.list"}
            //},
            //new ApiResource()
            //{
            //    Name="PrincipalApi",
            //    Scopes={ "student.delete", "student.update", "student.added", "exam.read", "absence.entry" }
            //},
            #endregion 
            
            new ApiResource()
            {
                Name="BaseApi",
                Scopes={"exam.update","exam.read","exam.list", "student.delete", "student.update", "student.added", "exam.read", "absence.entry" }
            }
        };
    }
}
