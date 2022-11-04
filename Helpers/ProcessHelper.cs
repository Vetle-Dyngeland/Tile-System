using System.Diagnostics;
using System;

namespace TileSystem2.Helpers
{
    public static class ProcessHelper
    {
        public static bool IsRunning(this Process process)
        {
            if(process == null)
                throw new ArgumentNullException(nameof(process));

            try {
                Process.GetProcessById(process.Id);
            }
            catch(ArgumentException) {
                return false;
            }
            return true;
        }
    }
}