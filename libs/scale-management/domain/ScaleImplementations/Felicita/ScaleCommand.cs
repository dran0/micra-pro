namespace MicraPro.ScaleManagement.Domain.ScaleImplementations.Felicita;

public abstract record ScaleCommand
{
    protected abstract byte[] Data { get; }

 
    public record Tare : ScaleCommand
    {
        protected override byte[] Data => [Constants.CMD_TARE];
    }
}