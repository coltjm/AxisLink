using AxisLink.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using AxisLink.Core.Models.Show;

namespace AxisLink.Infrastructure.Pollers
{
    public class SimulatedPoller : IPoller
    {
        public event Action<IEnumerable<Axis>>? PollCompleted;
        public event Action<Exception>? PollFailed;
        public bool IsPolling { get; private set; } = false;

        private CancellationTokenSource? _cts;
        private Task? _pollingTask;

        public void Start(IEnumerable<Axis> axes, TimeSpan interval)
        {
            // Only start polling if we arent already
            if (IsPolling) 
            {
                return;
            }

            // Set flag
            IsPolling = true;
            // Create cancellation token
            _cts = new CancellationTokenSource();
            // Ru the polling loops for all axes
            _pollingTask = Task.Run(() => PollLoopAsync(axes, interval, _cts.Token));
        }
        public void Stop()
        {
            // Only stop polling if we are currently polling
            if(!IsPolling)
            {
                return;
            }

            // Set flag
            IsPolling = false;
            // Cancel token
            _cts?.Cancel();

            try
            {
                // Finish polling task
                _pollingTask?.Wait();
            }
            catch (AggregateException)
            {
                // Handle exceptions
            }
            finally
            {
                // Dispose of created tokens and tasks
                _cts?.Dispose();
                _pollingTask?.Dispose();
                _cts = null;
                _pollingTask = null;
            }
        }

        protected async Task PollLoopAsync(IEnumerable<Axis> axes, TimeSpan interval, CancellationToken token)
        {

            Random random = new();
            // Poll until stop is called
            while (!token.IsCancellationRequested && IsPolling)
            {
                try
                {
                    Debug.WriteLine("Simulated Polling...");
                    foreach (var axis in axes) {
                        //simulate positions
                        
                    }
                    // Notify subscribers (UI) with updated the axes
                    PollCompleted?.Invoke(axes);
                    // Wait the given interval
                    await Task.Delay(interval, token); 
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception e)
                {
                    // Mark poll as failed
                    PollFailed?.Invoke(e);
                    // Wait 1s before trying again
                    await Task.Delay(1000, token);
                }
            }
        }
    }
}
