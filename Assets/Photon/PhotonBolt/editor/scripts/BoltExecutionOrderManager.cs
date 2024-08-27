using System;
using System.Collections.Generic;
using UnityEditor;

[InitializeOnLoad]
public static class BoltExecutionOrderManager
{
    static BoltExecutionOrderManager()
    {
        var monoScripts = new Dictionary<string, MonoScript>();

        foreach (MonoScript monoScript in MonoImporter.GetAllRuntimeMonoScripts())
        {
            try
            {
                switch (monoScript.name)
                {
                    case "BoltPoll":
                        SetExecutionOrder(monoScript, -10000);
                        break;

                    case "BoltSend":
                        SetExecutionOrder(monoScript, +10000);
                        break;

                    case "BoltEntity":
                        SetExecutionOrder(monoScript, -2500);
                        break;

                    default:
                        if (monoScripts.ContainsKey(monoScript.name))
                        {
                            monoScripts[monoScript.name] = monoScript;
                        }
                        else
                        {
                            monoScripts.Add(monoScript.name, monoScript);
                        }
                        break;
                }
            }
            catch { }
        }

        foreach (System.Reflection.Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
        {
            foreach (Type type in asm.GetTypes())
            {
                if (monoScripts.ContainsKey(type.Name))
                {
                    try
                    {
                        foreach (BoltExecutionOrderAttribute attribute in type.GetCustomAttributes(typeof(BoltExecutionOrderAttribute), false))
                        {
                            SetExecutionOrder(monoScripts[type.Name], attribute.executionOrder);
                        }
                    }
                    catch (Exception exn)
                    {
                        BoltLog.Exception(exn);
                    }
                }
            }
        }
    }

    private static void SetExecutionOrder(MonoScript script, Int32 executionOrder)
    {
        if (MonoImporter.GetExecutionOrder(script) != executionOrder)
        {
            MonoImporter.SetExecutionOrder(script, executionOrder);
        }
    }
}