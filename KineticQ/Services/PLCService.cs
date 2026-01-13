using KinetiCUE.Modules.Cueing.Models;
using KinetiCUE.Modules.Machines.Models;
using NModbus.Device;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinetiCUE.Services
{
    //handles modbus requests
    public class PLCService
    {
        private readonly PlcConnectionManager _connectionManager;

        // Inject the manager so we can look up sockets whenever we need them
        public PLCService(PlcConnectionManager connectionManager)
        {
            _connectionManager = connectionManager;
        }

        public async Task ExecuteCueAsync(Cue cue)
        {
            foreach(MoveInstruction moveInstruction in cue.Instructions.Values)
            {
                Debug.Write(moveInstruction.TargetPosition);

            }
            // 1. Get the LIVE master from the manager
            // (Do not store this in a variable; always ask for the fresh one)
            //var master = _connectionManager.GetMaster(machine.PLCId);

            //if (master == null)
            //{
            //    Debug.WriteLine($"Cannot execute cue. PLC {machine.PLCId} is disconnected.");
            //    return;
            //}

            try
            {
                // 2. Translate your Cue Model into Modbus Logic
                // Example: Writing the Target Position to the machine's assigned register

                
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Modbus Error: {ex.Message}");
            }
        }
    }
}
