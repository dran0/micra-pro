namespace MicraPro.ScaleManagement.Domain.ScaleImplementations.Felicita;

public static class Constants
{

    public const byte CMD_WEIGHT_ONLY_MODE = 0x31;
    public const byte CMD_WEIGHT_AND_TIMER_MODE = 0x32;
    public const byte CMD_START_TIMER_BY_FLOW_MODE = 0x33;
    public const byte CMD_START_TIMER_BY_FLOW_AUTOTARE_MODE = 0x34;
    public const byte CMD_START_TIMER_BY_TARE_AUTOTARE_MODE = 0x35;
    public const byte CMD_TOGGLE_BEEP = 0x42;
    public const byte CMD_RESET_TIMER = 0x43;
    public const byte CMD_SET_MAX_WEIGHT_2KG = 0x4d;
    public const byte CMD_START_TIMER = 0x52;
    public const byte CMD_STOP_TIMER = 0x53;
    public const byte CMD_TARE = 0x54;
    public const byte CMD_TOGGLE_UNIT = 0x55;
    public const byte CMD_SET_MAX_WEIGHT_1KG = 0x6d;

    public const int MIN_BATTERY_LEVEL = 137;
    public const int MAX_BATTERY_LEVEL = 161;

    public static readonly string[] SCALE_START_NAMES = ["FELICITA"];


}
