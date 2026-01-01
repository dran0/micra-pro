using MicraPro.ScaleManagement.DataDefinition;
using MicraPro.ScaleManagement.Domain.BluetoothAccess;

namespace MicraPro.ScaleManagement.Domain.ScaleImplementations.Felicita;

public class Scale(string identifier, IBluetoothService bluetoothService) : IScale
{
    public static string[] RequiredServiceIds => [ServiceId];

    private const string ServiceId = "ffe0";
    private const string CharacteristicId = "ffe1";

    public async Task<IScaleConnection> ConnectAsync(CancellationToken ct)
    {
        var bleConnection = await bluetoothService.ConnectDeviceAsync(identifier, ct);
        var bleService = await bleConnection.GetServiceAsync(ServiceId, ct);
        var connection = new ScaleConnection(
            await bleService.GetCharacteristicAsync(ServiceId, ct),
            await (
                await bleService.GetCharacteristicAsync(CharacteristicId, ct)
            ).GetValueObservableAsync(ct),
            bleConnection
        );
        return connection;
    }
}
