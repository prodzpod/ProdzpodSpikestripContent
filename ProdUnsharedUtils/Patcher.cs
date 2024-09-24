using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using BepInEx;
using BepInEx.Logging;
using Mono.Cecil;

public static class Patcher
{
    public const string GUID = "_com.ProdUnsharedUtils";
    public static ManualLogSource Log;
    public static void Initialize()
    {
        Log = Logger.CreateLogSource(GUID);
        // load rest of dlls
        var sdname = Paths.PatcherPluginPath + "/prodzpod-ProdzpodSpikestripContent";
        var dname = Paths.PluginPath + "/SpikestripModding-Spikestrip2_0";
        if (!Directory.Exists(dname)) { Log.LogWarning("Spikestrip does not exist."); return; }
        var sfiles = Directory.GetFiles(sdname).Select(Path.GetFileName).ToArray();
        var files = Directory.GetFiles(dname).Select(Path.GetFileName).ToArray();
        if (files.Contains("ProdzpodSpikestripContent.dll")) { Log.LogInfo(":3"); return; }
        else foreach (var fname in files)
        {
            if (fname.StartsWith("MMHOOK")) { Log.LogInfo("Deleting " + fname); File.Delete(fname); }
            if (sfiles.Contains(fname + ".xdelta"))
            {
                ProcessStartInfo startInfo = new()
                {
                    CreateNoWindow = false,
                    UseShellExecute = false,
                    FileName = Path.Combine(sdname, "xdelta.exe"),
                    WindowStyle = ProcessWindowStyle.Hidden,
                    Arguments = $"-f -d -s {Path.Combine(dname, fname)} {Path.Combine(sdname, fname + ".xdelta")} {Path.Combine(dname, "patched_" + fname)}"
                };
                try
                {
                    Log.LogInfo("Patching " + fname);
                    using Process exeProcess = Process.Start(startInfo);
                    exeProcess.WaitForExit();
                    File.Delete(Path.Combine(dname, fname));
                    File.Move(Path.Combine(dname, "patched_" + fname), Path.Combine(dname, fname));
                }
                catch (Exception ex) { Log.LogError(ex); }
            }
        }
        Log.LogInfo("Adding " + "ProdzpodSpikestripContent.dll");
        File.Copy(Path.Combine(sdname, "ProdzpodSpikestripContent.dll"), Path.Combine(dname, "ProdzpodSpikestripContent.dll"));
    }
    public static IEnumerable<string> TargetDLLs { get; } = [];
    public static void Patch(AssemblyDefinition _) { }
}