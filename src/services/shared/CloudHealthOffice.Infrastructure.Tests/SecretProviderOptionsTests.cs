using HealthCoreX.Infrastructure.Configuration;

namespace HealthCoreX.Infrastructure.Tests;

public class SecretProviderOptionsTests
{
    [Fact]
    public void Defaults_AreCorrect()
    {
        var options = new SecretProviderOptions();

        options.Provider.Should().Be(SecretProviderType.None);
        options.AzureKeyVaultUri.Should().BeNull();
        options.HashiCorpVaultAddress.Should().BeNull();
        options.HashiCorpVaultMountPoint.Should().BeNull();
        options.ReloadIntervalSeconds.Should().Be(300);
        options.GracefulDegradation.Should().BeTrue();
    }

    [Fact]
    public void SectionName_IsSecretProvider()
    {
        SecretProviderOptions.SectionName.Should().Be("SecretProvider");
    }
}
