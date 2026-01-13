using KinetiCUE.Models;
using KinetiCUE.Modules.PLC.Models;
using KinetiCUE.Services;
using NModbus;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
namespace KinetiCUE.Services
{ 
    public class PlcConnectionManager
    {
        // Hidden storage for the open sockets/masters
        public Dictionary<int, IModbusMaster> _masters = new Dictionary<int, IModbusMaster>();
        private Dictionary<int, TcpClient> _clients = new Dictionary<int, TcpClient>();

        private bool _isRunning = true;
        public event Action<bool, bool> OnStatusChanged;

        public PlcConnectionManager(ShowFile show)
        {
            // 2. Start ONE background loop for all heartbeats
            Task.Run(HeartbeatLoop);

            // 3. Listen for changes (User adds/removes PLC)
            show.PLCs.CollectionChanged += async (s, e) =>
            {
                if (e.NewItems != null)
                {
                    foreach (PLCModel m in e.NewItems)
                    {
                        // Now you can await safely. 
                        // The UI won't freeze, but the handler will wait for the connection logic.
                        await Connect(m);
                    }
                }

                if (e.OldItems != null)
                {
                    foreach (PLCModel m in e.OldItems)
                    {
                        // Disconnect is likely still synchronous (void), so no await needed.
                        Disconnect(m);
                    }
                }
            };
        }

        private async Task Connect(PLCModel plc)
        {
            try
            {
                var client = new TcpClient();
                if (!IPAddress.TryParse(plc.IpAddress, out IPAddress ipObj))
                {
                    Debug.WriteLine($"Invalid IP Format: {plc.IpAddress}");
                    return;
                }
                // 2. Create the connection task
                var connectTask = client.ConnectAsync(ipObj, plc.Port);

                // 3. Wait for either the connection OR a 2-second delay
                var completedTask = await Task.WhenAny(connectTask, Task.Delay(2000));

                if (completedTask == connectTask)
                {
                    // Connection finished within 2 seconds. 
                    // Now await it to catch any socket errors (like "Connection Refused")
                    await connectTask;
                }
                else
                {
                    // Timeout happened!
                    client.Dispose(); // Kill the pending connection
                    throw new TimeoutException("PLC took too long to respond.");
                }

                var factory = new ModbusFactory();
                var master = factory.CreateMaster(client);

                _clients[plc.Id] = client;
                _masters[plc.Id] = master;
                plc.IsConnected = true;
                
            }
            catch (Exception ex) 
            {
                Debug.WriteLine(ex);
                plc.IsConnected = false;
            }
        }

        private void Disconnect(PLCModel plc)
        {
            if (_clients.ContainsKey(plc.Id))
            {
                _clients[plc.Id].Dispose();
                _clients.Remove(plc.Id);
                _masters.Remove(plc.Id);
                plc.IsConnected = false;
            }
        }

        public IModbusMaster GetMaster(int plcId)
        {
            if (_masters.ContainsKey(plcId)) return _masters[plcId];
            return null;
        }

        private async Task HeartbeatLoop()
        {
            while (_isRunning)
            {

                foreach (var plc in FileManager.Instance.CurrentShow.PLCs)
                {
                    if (!_masters.ContainsKey(plc.Id))
                    {
                        
                        await Connect(plc);
                        continue;
                    }

                    try
                    {
                        var master = _masters[plc.Id];

                        bool[] inputs = await master.ReadInputsAsync(1, (ushort)(plc.EStopSignalAddress - 1), 1);

                        plc.IsEStopped = inputs[0];;
                        plc.IsConnected = true;
                    }
                    catch(Exception ex) 
                    {
                        plc.IsConnected = false;
                        plc.IsEStopped = true;
                        Disconnect(plc);
                    }
                }
                await Task.Delay(250); // 4 times a second is plenty for UI
            }
        }
    }
}