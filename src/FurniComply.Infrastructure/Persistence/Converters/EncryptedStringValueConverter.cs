using FurniComply.Infrastructure.Services;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace FurniComply.Infrastructure.Persistence.Converters;

public sealed class EncryptedStringValueConverter : ValueConverter<string?, string?>
{
    public EncryptedStringValueConverter(IEncryptionService encryptionService)
        : base(
            value => value == null ? null : encryptionService.EncryptSensitiveData(value),
            value => value == null ? null : encryptionService.DecryptSensitiveData(value))
    {
    }
}
