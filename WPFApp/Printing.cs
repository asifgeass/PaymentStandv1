using ExceptionManager;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Printing;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WPFApp
{
    public static class PrintersDll
    {
        [DllImport("winspool.drv", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool SetDefaultPrinter(string Name);

    }
    public static class Printing
    {
        private static readonly string MonitorName = "CePrnStatusMonitor";
        private static readonly string PrinterName = "vkp80";
        public static string Message { get; set; } = "Ожидание статуса принтера...\n Извините, оплата не доступна.";
        public static PrintQueue PrinterQueue { get; set; }

        [HandleProcessCorruptedStateExceptions]
        public static async Task<bool> IsPrinterReady()
        {
            try
            {
                var printServer = new LocalPrintServer();
                PrintQueue queue = printServer.DefaultPrintQueue;
                var status = queue.QueueStatus;
                
                //var notCritical = PrintQueueStatus.OutputBinFull || PrintQueueStatus.Printing;
                bool isFail = status == PrintQueueStatus.None || status.HasFlag(PrintQueueStatus.PaperOut);
                bool isOK = status == PrintQueueStatus.TonerLow || (status.HasFlag(PrintQueueStatus.TonerLow) && !isFail);
                if (isOK)
                {
                    return true;
                }
                else //isFail
                {
                    SetMsg(queue, "Извините, возникли проблемы с принтером\nОплата не доступна.");
                    if (status == PrintQueueStatus.None)
                    {
                        SetMsg(queue, "Ожидание статуса принтера...\n Извините, оплата не доступна.");
                        CheckWhyNone().RunAsync();
                    }
                    if (status.HasFlag(PrintQueueStatus.PaperOut)) SetMsg(queue, "Извините, в принтере закончилась бумага.\nОплата не доступна.");
                    return false;
                }
            }
            catch (Exception ex) 
            { 
                ex.Log();
                SetMsg(null, $"Извините, возникла непредвиденная ошибка\n({ex.Message})\nОплата не доступна.");
                return false; 
            }
        }

        [HandleProcessCorruptedStateExceptions]
        private static async Task CheckWhyNone()
        {
            try
            {
                bool isMonitorWorks = CheckMonitorProc();
                if (!isMonitorWorks)
                {
                    Ex.Log($"Printing.CheckWhyNone(): Monitor NOT works");
                    LaunchMonitor();
                    return;
                }

                Ex.Log($"Printing.CheckWhyNone(): VKP80 count={GetAllPrinters()?.Count}");
                PrintQueue one = GetOneFromAllPrinters();
                if (one == null)
                { Ex.Log($"Printing.CheckWhyNone(): All founded VKP80 has = State.None;"); return; }
                Ex.Log($"Printing.CheckWhyNone(): SET DEFAULT PRINTER = {one.FullName}");
                PrintersDll.SetDefaultPrinter(one.FullName);
            }
            catch (Exception ex)
            { ex.Log(); }
        }

        public static void LaunchMonitor()
        {
            try
            {
                Process.Start(MonitorName);
            }
            catch (Exception ex)
            {
                ex.Log();                
            }            
        }

        private static List<PrintQueue> GetAllPrinters()
        {
            var server = new PrintServer();
            var printQueues = server.GetPrintQueues(new[] { EnumeratedPrintQueueTypes.Local, EnumeratedPrintQueueTypes.Connections });
            List<PrintQueue> allVKP80 = printQueues?.Where(x => x.FullName.ToLower().Contains(PrinterName))?.ToList();
            return allVKP80;
        }

        private static PrintQueue GetOneFromAllPrinters()
        {
            List<PrintQueue> allVKP80 = GetAllPrinters();
            PrintQueue one = allVKP80?.FirstOrDefault(x => x.QueueStatus != PrintQueueStatus.None);
            return one;
        }


        private static bool CheckMonitorProc()
        {
            var monitor = Process.GetProcessesByName(MonitorName)?.FirstOrDefault();
            return monitor != null;
        }

        private static void SetMsg(PrintQueue queue, string v)
        {
            PrinterQueue = queue;
            Message = v;
        }
    }
}
