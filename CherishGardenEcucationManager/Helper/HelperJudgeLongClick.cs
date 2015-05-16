using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace CherishGardenEducationManager.Helper
{
    class HelperJudgeLongClick
    {
        public static Task<bool> MouseDown(FrameworkElement element, TimeSpan duration)
        {
            DispatcherTimer timer = new DispatcherTimer();
            TaskCompletionSource<bool> task = new TaskCompletionSource<bool>();
            timer.Interval = duration;

            MouseButtonEventHandler touchUpHandler = delegate
            {
                timer.Stop();
                if (task.Task.Status == TaskStatus.Running)
                {
                    task.SetResult(false);
                }
            };

            element.PreviewMouseUp += touchUpHandler;

            timer.Tick += delegate
            {
                element.PreviewMouseUp -= touchUpHandler;
                timer.Stop();
                task.SetResult(true);
            };

            timer.Start();
            return task.Task;
        }
    }
}
