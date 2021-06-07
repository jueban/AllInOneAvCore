using Microsoft.Win32.TaskScheduler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ScheduleService
    {
        public static void CreateOneTimeScheduler(string name, string desc, string exeLocation, string param)
        {
            using (TaskService ts = new())
            {
                var task = ts.FindTask(name);

                // Create a new task definition and assign properties
                TaskDefinition td = ts.NewTask();
                td.RegistrationInfo.Description = desc;

                // Create a trigger that will fire the task at this time every other day
                //td.Triggers.Add(new DailyTrigger { DaysInterval = 2 });

                // Create an action that will launch Notepad whenever the trigger fires
                td.Actions.Add(new ExecAction(exeLocation, param, null));

                // Register the task in the root folder
                ts.RootFolder.RegisterTaskDefinition(name, td);
            }
        }

        public static void CreateIntervalTimeScheduler(string name, string desc, string exeLocation, string param, int mintutes)
        {
            using (TaskService ts = new())
            {
                var task = ts.FindTask(name);

                // Create a new task definition and assign properties
                TaskDefinition td = ts.NewTask();
                td.RegistrationInfo.Description = desc;

                // Create a trigger that will fire the task at this time every other day
                //td.Triggers.Add(new DailyTrigger { DaysInterval = 2 });

                // Create an action that will launch Notepad whenever the trigger fires
                td.Actions.Add(new ExecAction(exeLocation, param, null));

                TimeTrigger tt = new TimeTrigger();
                tt.Repetition.Interval = TimeSpan.FromMinutes(mintutes);

                td.Triggers.Add(tt);

                // Register the task in the root folder
                ts.RootFolder.RegisterTaskDefinition(name, td);
            }
        }

        public static void RunScheduler(string name)
        {
            using (TaskService ts = new())
            {
                var task = ts.FindTask(name);

                if (task != null && task.State == TaskState.Ready)
                {
                    task.Run();
                }
            }
        }
    }
}
