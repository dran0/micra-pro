using System.Reactive.Linq;
using System.Linq;
using MicraPro.ScaleManagement.DataDefinition;
using MicraPro.ScaleManagement.DataDefinition.ValueObjects;
using MicraPro.ScaleManagement.Domain.BluetoothAccess;
using MicraPro.Shared.UtilsDotnet;

namespace MicraPro.ScaleManagement.Domain.ScaleImplementations.Felicita;

public class ScaleConnection(
    IBleCharacteristic commandCharacteristic,
    IObservable<byte[]> weightDataCharacteristicObservable,
    IBleDeviceConnection connection
) : IScaleConnection
{

    private static DateTime _lastWeightTimestamp = DateTime.MinValue;
    private static  double _lastWeight = 0;
    private static double[] _flowAverage = new double[] { 0, 0, 0, 0 };
    private const double FlowThreshold = 10;

    public async Task DisconnectAsync(CancellationToken ct)
    {
        await connection.Disconnect(ct);
    }

    public Task TareAsync(CancellationToken ct) =>
        commandCharacteristic.SendCommandAsync(new byte[] { Constants.CMD_TARE }, ct);

    private static ScaleDataPoint? FromFeliciateWeightData(byte[] data)
    {
        if (data.Length != 18)
            return null;
        var weightSymbolDataPointsRaw = data.Skip(3).Take(6);
        var weightString = from b in weightSymbolDataPointsRaw
            select (b - 48).ToString();
        if (!double.TryParse(string.Concat(weightString), out var weightFloat))
            return null;
        String scaleUnit = Convert.ToBase64String(data.Skip(9).Take(2).ToArray());
        return new ScaleDataPoint(DateTime.Now, CalculateFlow(weightFloat), weightFloat);
    }

    public IObservable<ScaleDataPoint> Data =>
        weightDataCharacteristicObservable.Select(FromFeliciateWeightData).Where(v => v != null)!;

    private static double CalculateFlow(double weight)
    {
        var now = DateTime.Now;
        var diffTime = now.Subtract(_lastWeightTimestamp).TotalSeconds;
        var diffWeight = weight - _lastWeight;
        _lastWeightTimestamp = now;
        _lastWeight = weight;
        if (diffTime is > 2 or <= 0)
            return 0;
        var flow = diffWeight / diffTime;
        if (flow < FlowThreshold)
            _flowAverage = _flowAverage.Skip(1).Append(diffWeight / diffTime).ToArray();
        return _flowAverage.Sum() / _flowAverage.Length;
    }

}
