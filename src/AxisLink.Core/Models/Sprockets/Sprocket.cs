namespace AxisLink.Core.Models.Sprockets;

public enum TransportProtocol
{
    ModbusTcp,
    ModbusRtu,
    AsciiTcp,
    AsciiSerial
}

public enum ShortcutType
{
    Read,
    Write,
    ReadWrite
}

public enum ControlType
{
    Button,
    Toggle,
    Slider,
    TextLabel,
    NumericLabel,
    StatusIndicator
}

public enum ResponseDataType
{
    String,
    Integer,
    Float,
    Boolean
}

public enum ModbusDataType
{
    Coil,
    Register16,
    Register32,
    Unassigned
}

public class SprocketModbusTarget
{
    public required string Address { get; set; }
    public ModbusDataType ModbusDataType { get; set; }
}

public class SprocketStep
{
    // Command template for ASCII to format and transmit (e.g. "D{Target.PositionPulses}").
    public string? Command { get; set; }

    // Modbus Address
    public string? Target { get; set; }

    // Expression to evaluate (e.g. "{Target.PositionPulses}" or "1").
    public string? Value { get; set; }

    // Delay in ms before executing this step.
    public int DelayMs { get; set; } = 0;

    // If > 0, signals a pulse operation (e.g. set high, wait pulse_ms, set low).
    public int PulseMs { get; set; } = 0;
    // Value to reset to for a pulse
    public string? PulseLowValue { get; set; }
}

public class SprocketShortcut
{
    public string Name { get; set; } = string.Empty;
    public ShortcutType Type { get; set; }

    public List<SprocketStep> Steps { get; set; } = new();

    public bool? ParseResponse {get; set; }

    public string? ParseExpression { get; set; } = string.Empty;

    public ResponseDataType? ResponseType { get; set; }
}

public class SprocketControl
{
    public string Label { get; set; } = string.Empty;
    public string? ShortcutName { get; set; }
    public ControlType ControlType { get; set; }
    public string? Value { get; set; }
    public float? Width { get; set; }
    public float? Height { get; set; }
    public float PosX { get; set; }
    public float PosY { get; set; }
}

public class SprocketWindow
{
    public string Name { get; set; } = string.Empty;
    public List<SprocketControl> Controls { get; set; } = new();
    public float? Width { get; set; }
    public float? Height { get; set; }
}

public class Sprocket
{
    public string Id { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public TransportProtocol Protocol { get; set; } = TransportProtocol.ModbusTcp;

    public float DefaultDriveScaleFactor { get; set; } = 1;

    // Modbus register address aliases (optional for pure ASCII devices)
    public Dictionary<string, SprocketModbusTarget> AddressAliases { get; set; } = new();

    // The sequence executed when the axis comes online
    public List<SprocketStep> InitSequence { get; set; } = new();

    // The sequence executed when an operator presses HOME
    public List<SprocketStep> HomeSequence { get; set; } = new();

    // The sequence executed when an operator presses GO
    public List<SprocketStep> RunCueSequence { get; set; } = new();

    // The sequence executed when an operator presses JOG
    public List<SprocketStep> RunJogSequence { get; set; } = new();

    // The sequence executed when an operator presses STOP
    public List<SprocketStep> StopSequence { get; set; } = new();

    // The sequence executed when an operator presses ESTOP - The ESTOP logic should be hardware level, this is just for sending any additional commands to the device if needed.
    public List<SprocketStep> EStopSequence { get; set; } = new();

    // List of shortcuts for common operations or read values
    public List<SprocketShortcut> Shortcuts { get; set; } = new();

    // List of windows for the UI
    public List<SprocketWindow> Windows { get; set; } = new();

    public Sprocket()
    {
        // Default constructor
    }
    
    public Sprocket(string sprocketId, string displayName, TransportProtocol protocol = TransportProtocol.ModbusTcp)
    {
        Id = sprocketId;
        DisplayName = displayName;
        Protocol = protocol;
    }
}