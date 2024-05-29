using EKtu.Domain.Entities;
using IdentityServer4.Models;
using static IdentityServer4.IdentityServerConstants;

namespace EKtu.AuthServer
{
    public class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources() => new List<IdentityResource>()
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Email(),
            new IdentityResource("custom.profile","Custom profile",new[]{"name","classname","classId"}),

        };
        public static IEnumerable<ApiScope> GetApiScopes() => new List<ApiScope>()
        {
            new ApiScope("exam.update","not girişi"),
            new ApiScope("exam.read","notları görme"),
            new ApiScope("exam.list","not sonuçlarını listeleme"),

            new ApiScope("student.delete","öğrenci silme"),
            new ApiScope("student.update","öğrenci bilgi güncelleme"),
            new ApiScope("student.added","öğrenci ekleme"),

            new ApiScope("absence.entry","devamsızlık giriş"),
            new ApiScope("base.token","client credentials"),
            new ApiScope("choose.lesson","öğrenci ders seçimi"),
            new ApiScope("lesson.approve","seçilen dersleri onaylama"),
            new ApiScope("lesson.added","ders ekleme"),
            new ApiScope("teacher.classlesson","öğretmene ders ve sınıf atama"),
            new ApiScope("student.calculateexamgrande","öğrencilerin harf notunu hesaplama"),
            new ApiScope("teacher.classlessonlist","öğretmenin girdiği dersleri listeleme"),
            new ApiScope("student.absence","öğrenci devamsızlıklarını görme"),
            new ApiScope("student.certificate","öğrenci belgesi çıkarma"),
            new ApiScope("student.updatechooselesson","öğrenci seçtiği dersi güncelleyebilir"),
            new ApiScope("student.getchooselesson","öğrenci seçtiği dersleri görüntüleyebilir"),
            new ApiScope("student.enteringgrades","öğrencilere not girme yetkisi")

        };
        public static IEnumerable<Client> GetClients() => new List<Client>()
        {
            new Client()
            {
                ClientId="ResourceOwnerTeacher",
                AllowedGrantTypes=GrantTypes.ResourceOwnerPassword,
                ClientSecrets=new[]{new Secret("secret".Sha256())},
                AllowedScopes={"exam.update","exam.read","teacher.classlessonlist","student.enteringgrades","custom.profile","openid"},
                RefreshTokenUsage=TokenUsage.ReUse,
                RefreshTokenExpiration=TokenExpiration.Absolute,
                AccessTokenLifetime=3,
                AbsoluteRefreshTokenLifetime=25
            },
            new Client()
            {
                ClientId="ResourceOwnerPrincipal",
                AllowedGrantTypes=GrantTypes.ResourceOwnerPassword,
                ClientSecrets=new[]{new Secret("secret".Sha256())},
                AllowedScopes={"student.delete","student.update","student.added","exam.read","lesson.approve","lesson.added","teacher.classlesson","student.calculateexamgrande","absence.entry","custom.profile","openid"},
                RefreshTokenUsage=TokenUsage.ReUse,
                RefreshTokenExpiration=TokenExpiration.Absolute,
                AccessTokenLifetime=1,
                AbsoluteRefreshTokenLifetime=25,
                UpdateAccessTokenClaimsOnRefresh=true,
            },
            new Client()
            {
                ClientId="ResourceOwnerStudent",
                AllowedGrantTypes=GrantTypes.ResourceOwnerPassword,
                ClientSecrets=new[]{new Secret("secret".Sha256())},
                AllowedScopes={"exam.list","choose.lesson","student.absence","student.certificate","student.updatechooselesson","student.getchooselesson","openid","custom.profile"},
                AllowOfflineAccess=true,
                RefreshTokenUsage=TokenUsage.ReUse,
                RefreshTokenExpiration=TokenExpiration.Absolute,
                AccessTokenLifetime=3600,
                AbsoluteRefreshTokenLifetime=7200
            },
            new Client()
            {
                ClientId="ClientCredentials",
                AllowedGrantTypes=GrantTypes.ClientCredentials,
                ClientSecrets=new[]{new Secret("secret".Sha256())},
                AllowedScopes={ "base.token" }

            }
        };
        public static IEnumerable<ApiResource> GetApiResources() => new List<ApiResource>()
        {
            
            new ApiResource()
            {
                Name="BaseApi",
                Scopes={"exam.update","exam.read","exam.list", "student.delete", "student.update", "student.added", "exam.read", "absence.entry", "base.token", "choose.lesson","lesson.approve", "lesson.added", "teacher.classlessonlist", "student.calculateexamgrande", "teacher.classlesson", "student.absence", "student.certificate", "student.updatechooselesson", "student.getchooselesson", "student.enteringgrades" }
            }
        };
    }
}
